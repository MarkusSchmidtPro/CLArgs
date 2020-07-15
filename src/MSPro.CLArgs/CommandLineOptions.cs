using System.Collections.Generic;
using System.Linq;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public class CommandLineOptions
    {
        private readonly List<OptionDescriptorAttribute> _optionDescriptors;

        // By name 
        private readonly Dictionary<string, string> _optionsByName = new Dictionary<string, string>();
        public readonly List<string> UnresolvedOptionNames = new List<string>();



        private CommandLineOptions(List<OptionDescriptorAttribute> optionDescriptors)
        {
            _optionDescriptors = optionDescriptors;
        }



        public ErrorDetailList Errors { get; } = new ErrorDetailList();



        private OptionDescriptorAttribute findDescriptor(string optionArgumentTag)
            => _optionDescriptors.FirstOrDefault(cld => cld.Tags.Any(t => t == optionArgumentTag));



        private void upsertOption(string optionName, string value) => _optionsByName[optionName] = value;
        private void addUnresolvedOption(string optionName) => UnresolvedOptionNames.Add(optionName);



        public static CommandLineOptions FromArguments<TCommandOptions>(Arguments arguments) =>
            FromArguments(arguments, GetDescriptorsFromType<TCommandOptions>());



        public static List<OptionDescriptorAttribute> GetDescriptorsFromType<TCommandOptions>() =>
            CustomAttributes.getSingle<OptionDescriptorAttribute>(typeof(TCommandOptions)).Values.ToList();



        public static CommandLineOptions FromArguments(
            Arguments arguments,
            List<OptionDescriptorAttribute> commandLineOptionDescriptors)
        {
            CommandLineOptions instance = new CommandLineOptions(commandLineOptionDescriptors);

            //
            // Collect options by tag (as provided in the command-line Arguments)
            // and store them under option[ name]
            // 
            foreach (var optionArgument in arguments.Options)
            {
                var optionDescriptor = instance.findDescriptor(optionArgument.Key);
                if (optionDescriptor == null)
                {
                    instance.Errors.AddError(optionArgument.Key, $"Unknown command-line option {optionArgument.Key}");
                    continue;
                }

                instance.upsertOption(optionDescriptor.Name, optionArgument.Value.Value);
            }

            // 
            // check mandatory has to happen
            // before default values are assigned
            //
            instance.checkMandatory();


            // All required options must be there already (provided in the command-line)
            //  otherwise there is already an error item.
            // Now we iterate through all not-required option descriptors:
            // If there is no option yet, we populate it with the default value,
            // if available or we collect it in the unresolved Dictionary.     
            //
            var optionalArguments
                = commandLineOptionDescriptors.Where(od => !od.Required);
            foreach (var option in optionalArguments)
            {
                if (instance.GetProvidedValue(option.Name) == null && option.Default != null)
                {
                    instance.upsertOption(option.Name, option.Default.ToString());
                }
                else
                {
                    instance.addUnresolvedOption(option.Name);
                }
            }

            return instance;
        }



        public string GetProvidedValue(string optionName) =>
            _optionsByName.ContainsKey(optionName) ? _optionsByName[optionName] : null;



        /// <summary>
        ///     Check if all mandatory parameters are there.
        /// </summary>
        private void checkMandatory()
        {
            var mandatoryOptions = _optionDescriptors
               .Where(od => od.Required);

            // each argument can have any number of tags
            // n Arguments with m tags
            foreach (var optionDescriptor in mandatoryOptions)
            {
                if (null == GetProvidedValue(optionDescriptor.Name))
                    this.Errors.AddError(optionDescriptor.Name,
                                         $"The mandatory command-line argument '{optionDescriptor.Name}' was not provided.");
            }
        }
    }
}