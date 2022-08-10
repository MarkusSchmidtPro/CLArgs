using System;
using System.Collections.Generic;
using System.Linq;



namespace MSPro.CLArgs;

/// <summary>
///     Provides support for Option Descriptors.
/// </summary>
/// <remarks>
///     An option as it is parsed from command-line is of type <see cref="Option" />.<br />
/// </remarks>
internal class OptionResolver
{
    /// <summary>
    ///     Options with any of these Tags will not be marked as unresolved.
    /// </summary>
    private static readonly HashSet<string> _wellKnownOptions = new() {"clArgsTrace"};

    private readonly IEnumerable<OptionDescriptorAttribute> _descriptors;


    public OptionResolver(IEnumerable<OptionDescriptorAttribute> descriptors)
    {
        _descriptors = descriptors;
    }


    /// <summary>
    ///     Resolve all options from command-line by tag into options by name.
    /// </summary>
    /// <remarks>
    ///     All <see cref="CommandLineArguments.Options" /> are resolved into an
    ///     OptionByName list, based on the provided list of <see cref="OptionDescriptorAttribute" />s.
    /// </remarks>
    /// <param name="commandLineArguments"></param>
    /// <param name="errors"></param>
    /// <param name="ignoreCase">If <c>true</c> cases will be ignored when parsing tags.</param>
    /// <param name="ignoreUnknownTags">
    ///     If <c>true</c> unknown tags provided in the command-line will be ignored.<br />
    ///     If set to <c>false</c> options provided in the command-line where there is no matching OptionDescriptor
    ///     will be recognized as 'too much' (not known). If there is any, <paramref name="errors" /> will contain
    ///     the corresponding messages.
    /// </param>
    /// <returns>A unique (by name) list of Options.</returns>
    public IEnumerable<Option> ResolveOptions(CommandLineArguments commandLineArguments,
        ErrorDetailList errors,
        bool ignoreCase,
        bool ignoreUnknownTags = false)
    {
        StringComparison stringComparison =
            ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
        // With AllowMultiple options with the same name can occur more than once
        List<Option> optionsByName = new();

        //
        // Collect options by tag (as provided in the command-line Arguments)
        // and store them under option[ name]
        // 
        foreach (var option in commandLineArguments.Options)
        {
            // Find an OptionDescriptor by searching in all Tags and in the Options name
            var optionDescriptor = _descriptors.FirstOrDefault(
                desc =>
                    desc.Tags!= null && desc.Tags.Any(t => string.Equals(t, option.Key, stringComparison))
                    || string.Equals(desc.OptionName, option.Key, stringComparison));

            if (optionDescriptor != null)
            {
                optionsByName.Add(new Option(optionDescriptor.OptionName, option.Value));
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
            if (!optionsByName.Any(o => o.Key.Equals(d.OptionName, stringComparison)))
                errors.AddError(d.OptionName, $"Missing mandatory Option: '{d.OptionName}'");
        }


        // All required options must be there already (provided in the command-line)
        //  otherwise there is already an error item.
        // Now we iterate through all not-required option descriptors:
        // If there is no option yet, we populate it with the default value,
        // if available or we collect it in the unresolved Dictionary.     

        // Iterate through all optional and not yet resolved descriptors
        var descriptorsOptional = _descriptors.Where(od => !od.Required);
        foreach (var d in descriptorsOptional.Where(i =>
                     !optionsByName.Any(o => o.Key.Equals(i.OptionName, stringComparison))))
        {
            optionsByName.Add(new Option(d.OptionName, d.Default?.ToString()));
        }

        // return a list of Options.
        return optionsByName;
    }
}