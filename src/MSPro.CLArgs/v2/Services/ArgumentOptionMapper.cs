using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;



namespace MSPro.CLArgs;

public class ArgumentOptionMapper
{
    /// <summary>
    ///     Options with any of these Tags will not be marked as unresolved.
    /// </summary>
    private static readonly HashSet<string> _wellKnownOptions = new() { "clArgsTrace" };

    private readonly ILogger<ArgumentOptionMapper> _logger;
    private readonly Settings2 _settings;



    public ArgumentOptionMapper(Settings2 settings, ILogger<ArgumentOptionMapper> logger)
    {
        _settings = settings;
        _logger   = logger;
    }



    /// <summary>
    ///     Map the command line arguments to an option collection and set each option's (string) values.
    /// </summary>
    /// <param name="arguments"></param>
    /// <param name="commandOptions"></param>
    /// <param name="errors"></param>
    public void SetOptionValues(
        [NotNull] IArgumentCollection arguments,
        [NotNull] IOptionCollection commandOptions,
        [NotNull] ErrorDetailList errors)
    {
        //
        // Collect options by tag (as provided in the command-line Arguments)
        // and store them under option[ name]
        // 
        foreach (var argumentOption in arguments.Options)
        {
            var knownOption = commandOptions.FirstOrDefault(
                desc => _settings.Equals(desc.OptionName, argumentOption.Key)
                        || (desc.Tags != null && desc.Tags.Any(
                            t => _settings.Equals(t, argumentOption.Key))));

            if (knownOption != null)
            {
                knownOption.Values.Add(argumentOption.Value);
            }
            else if (_settings.IgnoreUnknownOptions)
            {
                _logger.LogWarning($"Argument({argumentOption.Key}): Unknown, ignored.");
            }
            else if (_wellKnownOptions.Contains(argumentOption.Key))
            {
                _logger.LogDebug($"Argument({argumentOption.Key}): CLArgs well-known options.");
            }
            else
            {
                errors.AddError(argumentOption.Key, $"Argument({argumentOption.Key}): not known!");
            }
        }


        // 
        // check mandatory has to happen
        // before default values are assigned
        //
        var missingMandatoryOptions = commandOptions.Where(opt => opt.Required && !opt.HasValue);
        foreach (var opt in missingMandatoryOptions)
        {
            errors.AddError(opt.OptionName, $"Missing mandatory Option: '{opt.OptionName}'");
        }


        // Set default values   
        foreach (var notProvidedOption in commandOptions.Where(
                     opt => !opt.Required && !opt.HasValue && opt.Default != null))
        {
            _logger.LogDebug($"Option({notProvidedOption.OptionName}): Default={notProvidedOption.Default}");
            notProvidedOption.Values.Add(notProvidedOption.Default.ToString());
        }
    }
}