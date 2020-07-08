using System.Collections.Generic;
using System.Linq;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public class CommandLineOptions
    {
        private readonly List<CommandLineOptionAttribute> _optionDescriptors;

        // By name 
        private readonly Dictionary<string, string> _optionsByName = new Dictionary<string, string>();



        private CommandLineOptions(List<CommandLineOptionAttribute> optionDescriptors)
        {
            _optionDescriptors = optionDescriptors;
        }



        public ErrorDetailList Errors { get; } = new ErrorDetailList();



        private CommandLineOptionAttribute findDescriptor(string optionArgumentTag)
            => _optionDescriptors.FirstOrDefault(cld => cld.Tags.Any(t => t == optionArgumentTag));



        private void upsertOption(string optionName, string value) => _optionsByName[optionName] = value;



        public static CommandLineOptions FromArguments<TCommandOptions>(Arguments arguments) =>
            FromArguments(arguments, GetDescriptorsFromType<TCommandOptions>());



        public static List<CommandLineOptionAttribute> GetDescriptorsFromType<TCommandOptions>() =>
            CustomAttributes.getSingle<CommandLineOptionAttribute>(typeof(TCommandOptions)).Values.ToList();



        public static CommandLineOptions FromArguments(
            Arguments arguments,
            List<CommandLineOptionAttribute> commandLineOptionDescriptors)
        {
            CommandLineOptions instance = new CommandLineOptions(commandLineOptionDescriptors);

            //
            // Collect options by tag (as provided in the command-line)
            // and store them under option( name)
            // 
            foreach (var optionArgument in arguments.Options)
            {
                var optionDescriptor = instance.findDescriptor(optionArgument.Key);
                if (optionDescriptor == null)
                {
                    instance.Errors.AddError(optionArgument.Key, $"Unknown option {optionArgument.Key}");
                    continue;
                }

                instance.upsertOption(optionDescriptor.Name, optionArgument.Value.Value);
            }

            // 
            // check mandatory has to happen
            // before default values are assigned
            //
            instance.checkMandatory();


            //
            // populate non mandatory with default values
            //
            var optionalArguments = commandLineOptionDescriptors.Where(od => !od.Mandatory);
            foreach (var option in optionalArguments)
            {
                if (instance.GetProvidedValue(option.Name) == null)
                {
                    // not provided --> set default value (as string)
                    instance.upsertOption(option.Name, option.Default);
                }
            }


            return instance;
        }



        public string GetProvidedValue(string optionName) => _optionsByName.ContainsKey(optionName) ? _optionsByName[optionName] : null;



        /// <summary>
        ///     Check if all mandatory parameters are there.
        /// </summary>
        private void checkMandatory()
        {
            var mandatoryOptions = _optionDescriptors
                .Where(od => od.Mandatory);

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