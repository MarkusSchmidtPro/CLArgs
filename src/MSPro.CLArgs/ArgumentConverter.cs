using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     Turns Arguments into a parameter object of a specified type..
    /// </summary>
    internal class ArgumentConverter<TTarget> where TTarget : class, new()
    {
        private readonly ErrorDetailList _errors = new ErrorDetailList();
        private readonly Settings _settings;

        /// <summary>
        ///     Get a list of Options unique by their name.
        /// </summary>
        private List<Option> _resolvedOptions;



        public ArgumentConverter([NotNull] Settings settings)
        {
            _settings = settings;
        }



        /// <summary>
        ///     Execute the command that is resolved by the verbs passed in the command-line.
        /// </summary>
        public ErrorDetailList TryConvert(CommandLineArguments commandLineArguments,
                                          IEnumerable<OptionDescriptorAttribute> optionDescriptors,
                                          out TTarget target,
                                          out HashSet<string> unresolvedPropertyNames)
        {
            _settings.RunIf(TraceLevel.Verbose, () =>
            {
                foreach (var optionDescriptorAttribute in optionDescriptors)
                {
                    _settings.Trace(optionDescriptorAttribute.ToString());
                }
            });

            var optionResolver = new OptionResolver(optionDescriptors);
            _resolvedOptions = optionResolver.ResolveOptions(
                commandLineArguments,
                _errors,
                _settings.IgnoreCase,
                _settings.IgnoreUnknownOptions).ToList();

            _settings.RunIf(TraceLevel.Verbose, () =>
            {
                if (_errors.HasErrors()) return;
                string resolved = string.Join(", ", _resolvedOptions.Where(o => o.IsResolved).Select(o => o.Key));
                string unresolved = string.Join(", ", _resolvedOptions.Where(o => !o.IsResolved).Select(o => o.Key));
                _settings.Trace($"CLArgs: Resolved Options: '{resolved}'");
                _settings.Trace($"CLArgs: Unresolved Options: '{unresolved}'");
            });


            if (_errors.HasErrors())
            {
                target                  = null;
                unresolvedPropertyNames = null;
            }
            else
            {
                unresolvedPropertyNames = new HashSet<string>();
                target = (TTarget)
                    resolvePropertyValue(typeof(TTarget), _resolvedOptions, unresolvedPropertyNames);
            }
            return _errors;
        }



        #region Create instance and populate Command parameters

        /// <summary>
        ///     Resolves a single property's value. If property is an annotated class it will
        ///     recursively resolve all properties of this class.
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="options"></param>
        /// <param name="unresolvedPropertyNames"></param>
        private object resolvePropertyValue([NotNull] Type instanceType,
                                            [NotNull] IReadOnlyCollection<Option> options,
                                            [NotNull] ISet<string> unresolvedPropertyNames)
        {
            object instance = Activator.CreateInstance(instanceType);
            foreach (var pi in instanceType.GetProperties())
            {
                if (pi.GetFirst<OptionSetAttribute>() != null)
                {
                    var o = resolvePropertyValue(pi.PropertyType, options, unresolvedPropertyNames);
                    pi.SetValue(instance, o);
                }
                else
                {
                    var optionDescriptor = pi.GetFirst<OptionDescriptorAttribute>();
                    if (optionDescriptor == null) continue;


                    // the name of the Option that is bound to the property 
                    string boundOptionName = optionDescriptor.OptionName;
                    // the name and the of the property
                    string targetPropertyName = pi.Name;
                    Type targetType = pi.PropertyType;


                    // Check if the Option is defined. It is defined when it was in the
                    // OptionDescriptorList that was used to ResolveOptions. 
                    var option = options.FirstOrDefault(o => string.Equals(o.Key, boundOptionName));
                    if (option == null || !option.IsResolved)
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
                    else
                    {
                        object propertyValue =
                            _settings.ValueConverters.Convert(option.Value, boundOptionName, _errors, targetType);
                        pi.SetValue(instance, propertyValue);
                    }
                }
            }
            return instance;
        }

        #endregion
    }
}