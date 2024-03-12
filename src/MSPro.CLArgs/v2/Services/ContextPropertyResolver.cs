using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;



namespace MSPro.CLArgs;

public class ContextPropertyResolver(Settings2 settings, ILogger<ContextPropertyResolver> logger)
{
    /// <summary>
    ///     Options with any of these Tags will not be marked as unresolved.
    /// </summary>
    private static readonly HashSet<string> _wellKnownOptions = ["clArgsTrace"];



    /// <summary>
    ///     Map the command line arguments to an option collection and set each option's (string) values.
    /// </summary>
    public void ResolvePropertyValues(ContextPropertyCollection contextProperties,
        IArgumentCollection arguments,
        ErrorDetailList errors)
    {
        //
        // Collect options by tag (as provided in the command-line Arguments)
        // and store them under option[ name]
        // 
        foreach (KeyValuePair<string, string> argumentOption in arguments.Options)
        {
            ContextProperty? contextProperty = contextProperties.FirstOrDefault(
                desc => settings.Equals(desc.OptionName, argumentOption.Key)
                        || (desc.Tags != null && desc.Tags.Any(
                            t => settings.Equals(t, argumentOption.Key))));

            if (contextProperty != null)
            {
                contextProperty.SetValue(argumentOption.Value);
            }
            else if (settings.IgnoreUnknownOptions)
            {
                logger.LogWarning($"Argument({argumentOption.Key}): Unknown, ignored.");
            }
            else if (_wellKnownOptions.Contains(argumentOption.Key))
            {
                logger.LogDebug($"Argument({argumentOption.Key}): CLArgs well-known options.");
            }
            else
            {
                errors.AddError(argumentOption.Key, $"Unknown argument '{argumentOption.Key}'");
            }
        }


        // 
        // check mandatory has to happen
        // before default values are assigned
        //
        IEnumerable<ContextProperty> missingMandatoryOptions = contextProperties.Where(opt => opt.Required && !opt.HasValue);
        foreach (ContextProperty opt in missingMandatoryOptions)
        {
            errors.AddError(opt.OptionName, $"Missing mandatory Option: '{opt.OptionName}'");
        }


        // Set default values   
        foreach (ContextProperty notProvidedOption in contextProperties.Where(
                     opt => !opt.Required && !opt.HasValue && opt.Default != null))
        {
            logger.LogDebug($"Option({notProvidedOption.OptionName}): Default={notProvidedOption.Default}");
            notProvidedOption.UseDefault();
        }
    }
}