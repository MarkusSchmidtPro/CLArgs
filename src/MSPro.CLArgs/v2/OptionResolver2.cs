using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;


namespace MSPro.CLArgs
{
    /// <summary>
    ///     Provides support for Option Descriptors.
    /// </summary>
    /// <remarks>
    ///     An option as it is parsed from command-line is of type <see cref="Option" />.<br />
    /// </remarks>
    public class OptionResolver2
    {
        /// <summary>
        ///     Options with any of these Tags will not be marked as unresolved.
        /// </summary>
        private static readonly HashSet<string> _wellKnownOptions = new() { "clArgsTrace" };

        private readonly IArgumentCollection _argumentCollection;
        private readonly Settings2 _settings;

        public OptionResolver2(IArgumentCollection argumentCollection, Settings2 settings)
        {
            _argumentCollection = argumentCollection;
            _settings = settings;
        }


        public List<Option> Resolve(
            [NotNull] List<OptionDescriptorAttribute> descriptors, 
            [NotNull] ErrorDetailList errors)
        {
            // With AllowMultiple options with the same name can occur more than once
            List<Option> optionsByName = new();
            StringComparison stringComparison =
                _settings.IgnoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
            
            //
            // Collect options by tag (as provided in the command-line Arguments)
            // and store them under option[ name]
            // 
            foreach (var option in _argumentCollection.Options)
            {
                // Find an OptionDescriptor by searching in all Tags and in the Options name
                var optionDescriptor = descriptors.FirstOrDefault(
                    desc =>
                        desc.Tags != null && desc.Tags.Any(t => string.Equals(t, option.Key, stringComparison))
                        || string.Equals(desc.OptionName, option.Key, stringComparison));

                if (optionDescriptor != null)
                {
                    optionsByName.Add(new Option(optionDescriptor.OptionName, option.Value));
                }
                else if (!_settings.IgnoreUnknownOptions && !_wellKnownOptions.Contains(option.Key))
                {
                    errors.AddError(option.Key, $"Unknown Option '{option.Key}' provided in the command-line");
                }
            }


            // 
            // check mandatory has to happen
            // before default values are assigned
            //
            var descriptorsMandatory = descriptors.Where(od => od.Required);
            foreach (var d in descriptorsMandatory)
            {
                if (!optionsByName.Any(o => o.Key.Equals(d.OptionName, stringComparison)))
                    errors.AddError(d.OptionName, $"Missing mandatory Option: '{d.OptionName}'");
            }


            // All required options must be there already (provided in the command-line)
            //  otherwise there is already an error item.
            // Now we iterate through all not-required option descriptors:
            // If there is no option yet, we populate it with the default value,
            // if available or we collect it in the unresolved Dictionary.     

            // Iterate through all optional and not yet resolved descriptors
            var descriptorsOptional = descriptors.Where(od => !od.Required);
            foreach (var d in descriptorsOptional.Where(i =>
                !optionsByName.Any(o => o.Key.Equals(i.OptionName, stringComparison))))
            {
                optionsByName.Add(new Option(d.OptionName, d.Default?.ToString()));
            }

            // return a list of Options.
            return optionsByName;
        }
    }
}