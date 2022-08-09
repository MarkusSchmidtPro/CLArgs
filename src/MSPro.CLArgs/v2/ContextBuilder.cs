using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs;

/// <summary>
///     Turns Arguments into a parameter object of a specified type..
/// </summary>
public class ContextBuilder
{
    private readonly IArgumentCollection _argumentCollection;
    private readonly IArgumentConverterCollection _argumentConverterCollection;
    private readonly OptionResolver2 _optionsResolver;




    public ContextBuilder(
        IArgumentCollection argumentCollection
        , IArgumentConverterCollection argumentConverterCollection
        , OptionResolver2 optionsResolver)
    {
        _argumentCollection = argumentCollection;
        _argumentConverterCollection = argumentConverterCollection;
        _optionsResolver = optionsResolver;
    }



    /// <summary>
    ///     Execute the command that is resolved by the verbs passed in the command-line.
    /// </summary>
    public TContext Build<TContext>(
        out HashSet<string> unresolvedPropertyNames,
        out ErrorDetailList errors)
    {
        errors = new();
        unresolvedPropertyNames = new HashSet<string>();

        // contains all options for which a Property is defined
        // check .IsResolved flag if a value has been resolved
        List<OptionDescriptorAttribute> optionDescriptors = getDescriptors(typeof(TContext));
        List<Option> allOptions = _optionsResolver.Resolve(optionDescriptors, errors);
        if (errors.HasErrors()) return default;

        TContext context = createContext<TContext>(allOptions, unresolvedPropertyNames, errors);
        var contextProperties = context.GetType().GetProperties();
        // Find first property with a TargetAttribute 
        var targetsPropertyInfo = contextProperties.FirstOrDefault(pi => pi.GetFirst<TargetsAttribute>() != null);
        if (targetsPropertyInfo != null)
        {
            if (!typeof(IList<string>).IsAssignableFrom(targetsPropertyInfo.PropertyType))
            {
                errors.AddError(targetsPropertyInfo.Name,
                    $"The property {targetsPropertyInfo.Name} cannot be used for Targets because it does not inherit from type IList<string>");
            }
            else
            {
                var targetsList = (IList)targetsPropertyInfo.GetValue(context);
                if (targetsList == null)
                    errors.AddError(targetsPropertyInfo.Name,
                        $"The List property {targetsPropertyInfo.Name} must not be null. Use: public List<T> {targetsPropertyInfo.Name} {{ get; }} =new();");
                else
                    foreach (string target in _argumentCollection.Targets)
                        targetsList.Add(target);
            }
        }

        return context;
    }



    private List<OptionDescriptorAttribute> getDescriptors(Type contextType)
    {
        List<OptionDescriptorAttribute> result = new();
        foreach (var pi in contextType.GetProperties())
        {
            if (pi.GetFirst<OptionSetAttribute>() != null)
            {
                result.AddRange(getDescriptors(pi.PropertyType));
            }
            else
            {
                var optionDescriptor = pi.GetFirst<OptionDescriptorAttribute>();
                if (optionDescriptor != null) result.Add(optionDescriptor);
            }
        }

        return result;
    }



    #region Create instance and populate Command parameters

    private TContext createContext<TContext>([NotNull] IReadOnlyCollection<Option> options,
        [NotNull] ISet<string> unresolvedPropertyNames,
        [NotNull] ErrorDetailList errors)
        => (TContext)_createContext(typeof(TContext), options, unresolvedPropertyNames, errors);



    private object _createContext([NotNull] Type executionContextType,
        [NotNull] IReadOnlyCollection<Option> options,
        [NotNull] ISet<string> unresolvedPropertyNames,
        [NotNull] ErrorDetailList errors)
    {
        // The instance of the command parameters object.
        // This is where we set the values
        object executionContext = Activator.CreateInstance(executionContextType);

        // Iterate over all properties of the command parameters type
        foreach (var propInfo in executionContextType.GetProperties())
            // If a property is an OptionSet we must parse recursively
            if (propInfo.GetFirst<OptionSetAttribute>() != null)
            {
                object o = _createContext(propInfo.PropertyType, options, unresolvedPropertyNames, errors);
                propInfo.SetValue(executionContext, o);
            }
            else
            {
                // If a property is not an OptionDescriptor there is nothing to do with it.
                var optionDescriptor = propInfo.GetFirst<OptionDescriptorAttribute>();
                if (optionDescriptor == null) continue;


                // the name of the Option that is bound to the property 
                string boundOptionName = optionDescriptor.OptionName;
                // the name and the of the property
                string targetPropertyName = propInfo.Name;
                var targetType = propInfo.PropertyType;


                // Check if the Option is defined. It is defined when it was in the
                // OptionDescriptorList that was used to ResolveOptions. 
                // With AllowMultiple, options can be specified more than once 
                var providedOptions = options.Where(o => string.Equals(o.Key, boundOptionName)).ToList();
                if (providedOptions.Count(o => o.IsResolved) == 0) //|| !providedOptions.IsResolved)
                {
                    // Should not happen because ResolveOptions should have added
                    // and Option for each item in the OptionDescriptorList.
                    // Console.WriteLine("WARN");
                    // Anyways: we cannot set the target property's value
                    // when there is no matching option.  
                    unresolvedPropertyNames.Add(targetPropertyName);
                }
                else if (!_argumentConverterCollection.ContainsKey(targetType))
                {
                    errors.AddError(targetPropertyName,
                        $"No type converter found for type {targetType} of property {targetPropertyName} ");
                }
                // When an option has the allow multiple specified, only the first value is assigned to the property
                // and all others are added to the property whose name is specified as 'AllowMultiple'.
                else if (!string.IsNullOrWhiteSpace(optionDescriptor.AllowMultiple))
                {
                    /* Example:
                        // currentPropertyValue the value of the current argument specified in the command line

                        [ ... optionDescriptor.AllowMultiple = nameof( ComponentIds)]
                        public string ComponentId;
                        
                        // collectionPropertyName = "ComponentIds"
                        public List<string> ComponentIds;   // must implement IList
                     */
                    string collectionPropertyName = optionDescriptor.AllowMultiple;
                    var collectionPropertyInfo = executionContextType.GetProperty(collectionPropertyName);
                    if (collectionPropertyInfo == null)
                    {
                        errors.AddError(targetPropertyName,
                            $"The property {collectionPropertyInfo.Name} specified as 'AllowMultiple' on property {targetPropertyName} does not exist.");
                        continue;
                    }

                    // The collection property name must implement IList
                    if (!typeof(IList<string>).IsAssignableFrom(collectionPropertyInfo.PropertyType))
                    {
                        errors.AddError(targetPropertyName,
                            $"The property {collectionPropertyName} specified as 'AllowMultiple' on property {targetPropertyName} is not of type IList<string>");
                        continue;
                    }

                    //if (collectionPropertyInfo.SetMethod == null)
                    //{
                    //    _errors.AddError(collectionPropertyInfo.Name,
                    //        $"There is no public setter on property {collectionPropertyInfo.Name}.");
                    //    continue;
                    //}
                    // add options to the list
                    foreach (var providedOption in providedOptions.Where(o => o.IsResolved))
                    {
                        // In case of AllowMultipleSplit each option's value
                        // will be probably split into n values
                        // by using the AllowMultipleSplit token.
                        var contextList = (IList)collectionPropertyInfo.GetValue(executionContext);
                        if (contextList == null)
                        {
                            errors.AddError(collectionPropertyInfo.Name,
                                $"The List property {collectionPropertyInfo.Name} must not be null. Use: public List<T> {collectionPropertyInfo.Name} {{ get; }} =new();");
                            continue;
                        }

                        string providedOptionValue = providedOption.Value;
                        if (!string.IsNullOrWhiteSpace(optionDescriptor.AllowMultipleSplit))
                        {
                            string[] providedValuesString =
                                providedOptionValue.Split(optionDescriptor.AllowMultipleSplit.ToCharArray());
                            foreach (string valueString in providedValuesString)
                                contextList.Add(
                                    _argumentConverterCollection[targetType].Convert(valueString, boundOptionName, errors, targetType));
                        }
                        else
                        {
                            contextList.Add(providedOptionValue);
                        }
                    }

                    // the first list item will also be set at the current properties value
                    // there should be at least one resolved option 
                    propInfo.SetValue(executionContext, providedOptions[0].Value);
                }
                else // AllowMultiple = false 
                if (providedOptions.Count > 1)
                {
                    errors.AddError(targetPropertyName,
                        $"'AllowMultiple' is not specified on property {targetPropertyName} however it was provided {providedOptions.Count} times.");
                }
                else if (propInfo.SetMethod == null)
                {
                    errors.AddError(propInfo.Name,
                        $"There is no public setter on property {propInfo.Name}.");
                }
                else
                {
                    // Convert the string from the command line into the correct type so that the value
                    // can be assigned to the property.
                    object propertyValue =
                        _argumentConverterCollection[targetType].Convert(providedOptions[0].Value, boundOptionName, errors, targetType);
                    propInfo.SetValue(executionContext, propertyValue);
                }
            }

        return executionContext;
    }

    #endregion
}