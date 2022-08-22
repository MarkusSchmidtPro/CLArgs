using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;



namespace MSPro.CLArgs;

/// <summary>
///     Turns Arguments into a parameter object of a specified type.
/// </summary>
[PublicAPI]
public class ContextBuilder
{
    private readonly List<Action<IOptionValueConverterCollection>> _configureOptionValueConvertersActions = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly Settings2 _settings;
    private IOptionValueConverterCollection _optionValueConverters;



    public ContextBuilder(IServiceProvider serviceProvider, Settings2 settings)
    {
        _serviceProvider = serviceProvider;
        _settings        = settings;
    }



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
            { typeof(bool), new BoolConverter() },
            { typeof(DateTime), new DateTimeConverter() },
            { typeof(Enum), new EnumConverter() }
        };
        return result;
    }



    public TContext Build<TContext>(IArgumentCollection arguments,
                                    IOptionCollection commandOptions,
                                    out HashSet<string> unresolvedPropertyNames,
                                    out ErrorDetailList errors)
    {
        _optionValueConverters = createDefaultConverters();
        foreach (var build in _configureOptionValueConvertersActions) build(_optionValueConverters);

        errors                  = new();
        unresolvedPropertyNames = new();

        // contains all options for which a Property is defined
        // check .IsResolved flag if a value has been resolved
        ArgumentOptionMapper argumentOptionMapper = _serviceProvider.GetRequiredService<ArgumentOptionMapper>();
        argumentOptionMapper.SetOptionValues(arguments, commandOptions, errors);
        if (errors.HasErrors()) return default;

        TContext context = createContext<TContext>(commandOptions, unresolvedPropertyNames, errors);
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
                    foreach (string target in arguments.Targets)
                        targetsList.Add(target);
            }
        }

        return context;
    }



    #region Create instance and populate Command parameters

    private TContext createContext<TContext>([NotNull] IOptionCollection options,
                                             [NotNull] ISet<string> unresolvedPropertyNames,
                                             [NotNull] ErrorDetailList errors)
        => (TContext)_createContext(typeof(TContext), options, unresolvedPropertyNames, errors);



    private object _createContext([NotNull] Type executionContextType,
                                  [NotNull] IOptionCollection options,
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

                string optionName = optionDescriptor.OptionName;
                string targetPropertyName = propInfo.Name;
                var targetType = propInfo.PropertyType;


                // Check if the Option is defined. It is defined when it was in the
                // OptionDescriptorList that was used to ResolveOptions. 
                // With AllowMultiple, options can be specified more than once 
                var commandOption = options.FirstOrDefault(opt => _settings.Equals(opt.OptionName, optionName));
                Debug.Assert(commandOption != null);
                if (!commandOption.HasValue) //|| !providedOptions.IsResolved)
                {
                    // Should not happen because ResolveOptions should have added
                    // and Option for each item in the OptionDescriptorList.
                    // Console.WriteLine("WARN");
                    // Anyways: we cannot set the target property's value
                    // when there is no matching option.  
                    unresolvedPropertyNames.Add(targetPropertyName);
                }
                else if (!_optionValueConverters.ContainsKey(targetType))
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
                                        $"The property {collectionPropertyName} specified as 'AllowMultiple' on property {targetPropertyName} does not exist.");
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

                    foreach (string providedOptionValue in commandOption.Values)
                    {
                        if (!string.IsNullOrWhiteSpace(optionDescriptor.AllowMultipleSplit))
                        {
                            string[] providedValuesString =
                                providedOptionValue.Split(optionDescriptor.AllowMultipleSplit.ToCharArray());
                            foreach (string valueString in providedValuesString)
                                contextList.Add(
                                    _optionValueConverters[targetType].Convert(valueString, optionName, errors, targetType));
                        }
                        else
                        {
                            contextList.Add(providedOptionValue);
                        }
                    }

                    // the first list item will also be set at the current properties value
                    // there should be at least one resolved option 
                    propInfo.SetValue(executionContext, commandOption.Values[0]);
                }
                else // AllowMultiple = false 
                if (commandOption.Values.Count > 1)
                {
                    errors.AddError(targetPropertyName,
                                    $"'AllowMultiple' is not specified on property {targetPropertyName} however it was provided {commandOption.Values.Count} times.");
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
                        _optionValueConverters[targetType].Convert(commandOption.Values[0], optionName, errors, targetType);
                    propInfo.SetValue(executionContext, propertyValue);
                }
            }

        return executionContext;
    }

    #endregion
}