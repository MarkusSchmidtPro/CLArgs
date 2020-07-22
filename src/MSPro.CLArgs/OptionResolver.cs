using System;
using System.Collections.Generic;
using System.Linq;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     Provides support for Option Descriptors.
    /// </summary>
    /// <remarks>
    ///     An option as it is parsed from command-line is of type <see cref="Option" />.<br />
    /// </remarks>
    class OptionResolver
    {
        /// <summary>
        ///     Options with any of these Tags will not be marked as unresolved.
        /// </summary>
        private static readonly HashSet<string> _wellKnownOptions = new HashSet<string> {"clArgsTrace"};

        private readonly IEnumerable<OptionDescriptorAttribute> _descriptors;



        public OptionResolver(IEnumerable<OptionDescriptorAttribute> descriptors)
        {
            _descriptors = descriptors;
        }



        /// <summary>
        ///     Resolve all options by tag to options by name.
        /// </summary>
        /// <remarks>
        ///     All <see cref="Arguments.Options" /> are resolved into an
        ///     OptionByName list, based on the provided list of <see cref="OptionDescriptorAttribute" />s.
        /// </remarks>
        /// <param name="arguments"></param>
        /// <param name="errors"></param>
        /// <param name="ignoreUnknownTags"></param>
        /// <returns>A unique (by name) list of Options.</returns>
        public IEnumerable<Option> ResolveOptions(Arguments arguments, 
                                                  ErrorDetailList errors, 
                                                  bool ignoreCase = false,
                                                  bool ignoreUnknownTags = false)
        {
            Dictionary<string, Option> optionsByName = new Dictionary<string, Option>();

            //
            // Collect options by tag (as provided in the command-line Arguments)
            // and store them under option[ name]
            // 
            foreach (var option in arguments.Options)
            {
                var d = _descriptors.FirstOrDefault(
                    i => i.Tags.Any(
                        t => t == option.Key));

                if (d != null)
                {
                    optionsByName[d.OptionName] = option;
                }
                else if (!ignoreUnknownTags && !_wellKnownOptions.Contains(option.Key))
                {
                    errors.AddError(option.Key, $"Unknown Option '{option.Key}' provided in the command-line");
                }
            }


            // 
            // check mandatory has to happen
            // before default values are assigned
            //
            var descriptorsMandatory = _descriptors.Where(od => od.Required);
            foreach (var d in descriptorsMandatory)
            {
                if (!optionsByName.ContainsKey(d.OptionName))
                    errors.AddError(d.OptionName, $"Missing mandatory Option: '{d.OptionName}'");
            }


            // All required options must be there already (provided in the command-line)
            //  otherwise there is already an error item.
            // Now we iterate through all not-required option descriptors:
            // If there is no option yet, we populate it with the default value,
            // if available or we collect it in the unresolved Dictionary.     

            // Iterate through all optional and not yet resolved descriptors
            var descriptorsOptional = _descriptors.Where(od => !od.Required);
            foreach (var d in descriptorsOptional.Where(i => !optionsByName.ContainsKey(i.OptionName)))
            {
                optionsByName.Add(d.OptionName, new Option(d.OptionName, d.Default?.ToString()));
            }

            // Trace Debug
            if (arguments.OptionTagProvided("clArgsTrace") && !errors.HasErrors())
            {
                string resolved = string.Join(", ",
                                              optionsByName.Values.Where(o => o.IsResolved).Select(o => o.Key));
                string unresolved = string.Join(", ",
                                                optionsByName.Values.Where(o => !o.IsResolved).Select(o => o.Key));
                Console.WriteLine($"CLArgs: Resolved Options: '{resolved}'");
                Console.WriteLine($"CLArgs: Unresolved Options: '{unresolved}'");
            }

            // return a unique (by name) list of Options.
            return optionsByName.Values;
        }
    }
}