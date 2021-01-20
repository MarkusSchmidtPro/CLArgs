using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     Turns Arguments into a parameter object of a specified type..
    /// </summary>
    internal class ArgumentConverter<TExecutionContext> where TExecutionContext : class, new()
    {
        private readonly ErrorDetailList _errors = new();
        private readonly Settings _settings;

        /// <summary>
        ///     Get a list of Options unique by their name.
        /// </summary>
        private List<Option> _allOptions;


        public ArgumentConverter([NotNull] Settings settings)
        {
            _settings = settings;
        }


        /// <summary>
        ///     Execute the command that is resolved by the verbs passed in the command-line.
        /// </summary>
        public ErrorDetailList TryConvert(CommandLineArguments commandLineArguments,
            IEnumerable<OptionDescriptorAttribute> optionDescriptors,
            out TExecutionContext executionContext,
            out HashSet<string> unresolvedPropertyNames)
        {
            //_settings.RunIf(TraceLevel.Verbose, () =>
            //{
            //    foreach (var optionDescriptorAttribute in optionDescriptors)
            //    {
            //        _settings.Trace(optionDescriptorAttribute.ToString());
            //    }
            //});

            var optionResolver = new OptionResolver(optionDescriptors);
            // contains all options for which a Property is defined
            // check .IsResolved flag if a value has been resolved
            _allOptions = optionResolver.ResolveOptions(
                commandLineArguments,
                _errors,
                _settings.IgnoreCase,
                _settings.IgnoreUnknownOptions).ToList();

            //_settings.RunIf(TraceLevel.Verbose, () =>
            //{
            //    if (_errors.HasErrors()) return;
            //    string resolved = string.Join(", ", _allOptions.Where(o => o.IsResolved).Select(o => o.Key));
            //    string unresolved = string.Join(", ", _allOptions.Where(o => !o.IsResolved).Select(o => o.Key));
            //    _settings.Trace($"CLArgs: Resolved Options: '{resolved}'");
            //    _settings.Trace($"CLArgs: Unresolved Options: '{unresolved}'");
            //});


            if (_errors.HasErrors())
            {
                executionContext = null;
                unresolvedPropertyNames = null;
            }
            else
            {
                unresolvedPropertyNames = new HashSet<string>();
                executionContext = (TExecutionContext)
                    resolvePropertyValue(typeof(TExecutionContext), _allOptions, unresolvedPropertyNames);
            }

            return _errors;
        }



        #region Create instance and populate Command parameters

        /// <summary>
        ///     Resolves a single property's value. If property is an annotated class it will
        ///     recursively resolve all properties of this class.
        /// </summary>
        /// <param name="executionContextType"></param>
        /// <param name="options"></param>
        /// <param name="unresolvedPropertyNames"></param>
        private object resolvePropertyValue([NotNull] Type executionContextType,
            [NotNull] IReadOnlyCollection<Option> options,
            [NotNull] ISet<string> unresolvedPropertyNames)
        {
            // The instance of the command parameters object.
            // This is where we set the values
            object executionContext = Activator.CreateInstance(executionContextType);

            // Iterate over all properties of the command parameters type
            foreach (var propInfo in executionContextType.GetProperties())
            {
                // If a property is an OptionSet we must parse recursively
                if (propInfo.GetFirst<OptionSetAttribute>() != null)
                {
                    var o = resolvePropertyValue(propInfo.PropertyType, options, unresolvedPropertyNames);
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
                    Type targetType = propInfo.PropertyType;


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
                    else if (!_settings.ValueConverters.CanConvert(targetType))
                    {
                        _errors.AddError(targetPropertyName,
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
                            _errors.AddError(targetPropertyName,
                                $"The property {collectionPropertyName} specified as 'AllowMultiple' on property {targetPropertyName} does not exist.");
                            continue;
                        }

                        // The collection property name must implement IList
                        if (!typeof(IList).IsAssignableFrom(collectionPropertyInfo.PropertyType))
                        {
                            _errors.AddError(targetPropertyName,
                                $"The property {collectionPropertyName} specified as 'AllowMultiple' on property {targetPropertyName} is not of type IList");
                            continue;
                        }


                        // Create a new list instance and
                        // set it to the 'AllowMultiple' property (this does NOT add a list item!)
                        var listInstance = (IList) Activator.CreateInstance(collectionPropertyInfo.PropertyType);
                        // add options to the list
                        foreach (Option providedOption in providedOptions.Where(o => o.IsResolved))
                        {
                            // In case of AllowMultipleSplit each option's value
                            // will be probably split into n values
                            // by using the AllowMultipleSplit token.
                            List<string> allValues = new();
                            string providedOptionValue = providedOption.Value;
                            if (!string.IsNullOrWhiteSpace(optionDescriptor.AllowMultipleSplit))
                            {
                                allValues.AddRange(
                                    providedOptionValue.Split(optionDescriptor.AllowMultipleSplit.ToCharArray()));
                            }
                            else allValues.Add(providedOptionValue);

                            foreach (string value in allValues)
                            {
                                object currentPropertyValue =
                                    _settings.ValueConverters.Convert(value, boundOptionName, _errors,
                                        targetType);
                                listInstance.Add(currentPropertyValue);
                            }
                        }

                        collectionPropertyInfo.SetValue(executionContext, listInstance);
                        // the first list item will also be set at the current properties value
                        // there should be at least one resolved option 
                        propInfo.SetValue(executionContext, providedOptions[0].Value);
                    }
                    else // AllowMultiple = false 
                    if (providedOptions.Count > 1)
                    {
                        _errors.AddError(targetPropertyName,
                            $"'AllowMultiple' is not specified on property {targetPropertyName} however it was provided {providedOptions.Count} times.");
                    }
                    else
                    {
                        // Convert the string from the command line into the correct type so that the value
                        // can be assigned to the property.
                        object propertyValue =
                            _settings.ValueConverters.Convert(providedOptions[0].Value, boundOptionName, _errors,
                                targetType);
                        propInfo.SetValue(executionContext, propertyValue);
                    }
                }
            }

            return executionContext;
        }

        #endregion
    }
}