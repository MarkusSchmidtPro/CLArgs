using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;



namespace MSPro.CLArgs;

/// <summary>
///     Turns Arguments into a parameter object of a specified type.
/// </summary>
public class ContextBuilder(IServiceProvider serviceProvider, Settings2 settings)
{
    private readonly List<Action<IOptionValueConverterCollection>> _configureOptionValueConvertersActions = [];
    private IOptionValueConverterCollection _optionValueConverters = null!;



    public void ConfigureConverters(Action<IOptionValueConverterCollection> action)
    {
        _configureOptionValueConvertersActions.Add(action);
    }



    private IOptionValueConverterCollection createDefaultConverters()
    {
        var result = new OptionValueConverterCollection
        {
            { typeof(string), new StringConverter() },
            { typeof(int), new IntConverter() },
            { typeof(decimal), new DecimalConverter() },
            { typeof(bool), new BoolConverter() },
            { typeof(DateTime), new DateTimeConverter() },
            { typeof(Enum), new EnumConverter() }
        };
        return result;
    }



    public TContext Build<TContext>(IArgumentCollection arguments,
        ContextPropertyCollection contextProperties,
        ErrorDetailList errors) where TContext: notnull
    {
        _optionValueConverters = createDefaultConverters();
        foreach (Action<IOptionValueConverterCollection> build in _configureOptionValueConvertersActions) build(_optionValueConverters);


        // contains all options for which a Property is defined
        // check .IsResolved flag if a value has been resolved
        var contextPropertyResolver = serviceProvider.GetRequiredService<ContextPropertyResolver>();
        contextPropertyResolver.ResolvePropertyValues(contextProperties, arguments, errors);
        if (errors.HasErrors()) return default!;

        var context = (TContext) createContext(typeof(TContext), contextProperties, errors);
        PropertyInfo[] targetPropertyInfos = context.GetType().GetProperties();
        // Find first property with a TargetAttribute 
        PropertyInfo? targetsPropertyInfo = targetPropertyInfos.FirstOrDefault(pi => pi.GetFirst<TargetsAttribute>() != null);
        if (targetsPropertyInfo == null) return context;


        if (!typeof(IList<string>).IsAssignableFrom(targetsPropertyInfo.PropertyType))
        {
            errors.AddError(targetsPropertyInfo.Name,
                $"The property {targetsPropertyInfo.Name} cannot be used for Targets because it does not inherit from type IList<string>");
        }
        else
        {
            var targetsList = (IList?)targetsPropertyInfo.GetValue(context);
            if (targetsList == null)
                errors.AddError(targetsPropertyInfo.Name,
                    $"The List property {targetsPropertyInfo.Name} must not be null. Use: public List<T> {targetsPropertyInfo.Name} {{ get; }} =new();");
            else
                foreach (string target in arguments.Targets)
                    targetsList.Add(target);
        }

        return context;
    }



    /// <summary>
    ///     Iterate though all <see cref="OptionDescriptorAttribute">OptionDescriptor</see>
    ///     properties of a given context type and assign the values provided in the command-line
    ///     to each property.
    /// </summary>
    /// <param name="contextType">The type of the Context.</param>
    /// <param name="contextProperties">The options provided in the command-line</param>
    /// <param name="errors">The list of errors that occured during resolution.</param>
    /// <returns>An instance of <paramref name="contextType" />.</returns>
    private object createContext(Type contextType,
        ContextPropertyCollection contextProperties,
        ErrorDetailList errors)
    {
        // The instance of the command parameters object.
        // This is where we set the values
        object executionContext = Activator.CreateInstance(contextType)!;
        
        // Iterate over all properties of the command parameters type
        foreach (PropertyInfo propInfo in executionContext.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            // If a property is an OptionSet we must parse recursively
            if (propInfo.GetFirst<OptionSetAttribute>() != null)
            {
                object o = createContext(propInfo.PropertyType, contextProperties, errors);
                propInfo.SetValue(executionContext, o);
                continue;
            }

            // If a property is not an OptionDescriptor there is nothing to do with it.
            var optionDescriptor = propInfo.GetFirst<OptionDescriptorAttribute>();
            if (optionDescriptor == null) continue;


            string optionName = optionDescriptor.OptionName;
            string propertyName = propInfo.Name;
            Type propertyType = propInfo.PropertyType;
            if (propertyType.IsGenericType &&
                propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propertyType = propertyType.GetGenericArguments()[0];
            }


            // Check if the Option is defined. It is defined when it was in the
            // OptionDescriptorList that was used to ResolveOptions. 
            // With AllowMultiple, options can be specified more than once 
            ContextProperty? contextProperty = contextProperties.FirstOrDefault(opt => settings.Equals(opt.OptionName, optionName));
            Debug.Assert(contextProperty != null);
            if (!contextProperty.HasValue) //|| !providedOptions.IsResolved)
            {
                // Should not happen because ResolveOptions should have added
                // and Option for each item in the OptionDescriptorList.
                // Console.WriteLine("WARN");
                // Anyways: we cannot set the target property's value
                // when there is no matching option.  
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
                PropertyInfo? collectionPropertyInfo = contextType.GetProperty(collectionPropertyName);
                if (collectionPropertyInfo == null)
                {
                    errors.AddError(propertyName,
                        $"The property {collectionPropertyName} specified as 'AllowMultiple' on property {propertyName} does not exist.");
                    continue;
                }

                // The collection property name must implement IList
                if (!typeof(IList<string>).IsAssignableFrom(collectionPropertyInfo.PropertyType))
                {
                    errors.AddError(propertyName,
                        $"The property {collectionPropertyName} specified as 'AllowMultiple' on property {propertyName} is not of type IList<string>");
                    continue;
                }

                //if (collectionPropertyInfo.SetMethod == null)
                //{
                //    _errors.AddError(collectionPropertyInfo.Name,
                //        $"There is no public setter on property {collectionPropertyInfo.Name}.");
                //    continue;
                //}
                // add options to the list
                // In case of AllowMultipleSplit each option's value
                // will be probably split into n values
                // by using the AllowMultipleSplit token.
                var contextList = (IList?)collectionPropertyInfo.GetValue(executionContext);
                if (contextList == null)
                {
                    errors.AddError(collectionPropertyInfo.Name,
                        $"The List property {collectionPropertyInfo.Name} must not be null. Use: public List<T> {collectionPropertyInfo.Name} {{ get; }} =new();");
                    continue;
                }

                foreach (string providedOptionValue in contextProperty.ProvidedValues)
                {
                    if (!string.IsNullOrWhiteSpace(optionDescriptor.AllowMultipleSplit))
                    {
                        string[] providedValuesString =
                            providedOptionValue.Split(optionDescriptor.AllowMultipleSplit.ToCharArray());
                        foreach (string valueString in providedValuesString)
                            contextList.Add(
                                _optionValueConverters[propertyType]
                                    .Convert(valueString, optionName, errors, propertyType));
                    }
                    else
                    {
                        contextList.Add(providedOptionValue);
                    }
                }

                // the first list item will also be set at the current properties value
                // there should be at least one resolved option 
                propInfo.SetValue(executionContext, contextProperty.ProvidedValues[0]);
            }
            else if (contextProperty.ProvidedValues.Count > 1) // AllowMultiple = false 
            {
                errors.AddError(propertyName,
                    $"'AllowMultiple' is not specified on property {propertyName} however it was provided {contextProperty.ProvidedValues.Count} times.");
            }
            else if (propInfo.SetMethod == null)
            {
                errors.AddError(propInfo.Name,
                    $"There is no public setter on property {propInfo.Name}.");
            }
            else
            {
                if (!_optionValueConverters.TryGetValue(propertyType, out IArgumentConverter? converter))
                {
                    if (propertyType.BaseType != null)
                        _optionValueConverters.TryGetValue(propertyType.BaseType, out converter);

                    if (converter == null)
                    {
                        errors.AddError(propertyName,
                            $"No type converter found for type {propertyType} of property {propertyName} ");
                    }
                }

                if (converter != null)
                {
                    // Convert the string from the command line into the correct type so that the value
                    // can be assigned to the property.
                    object? propertyValue =
                        converter.Convert(contextProperty.ProvidedValues[0], optionName, errors, propertyType);
                    propInfo.SetValue(executionContext, propertyValue);
                }
            }
        }

        return executionContext;
    }
}