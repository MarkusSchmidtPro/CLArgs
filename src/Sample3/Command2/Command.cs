using System;
using System.Collections.Generic;
using System.Linq;



namespace MSPro.CLArgs.Sample3.Command2
{
    internal class Command : CommandBase2
    {
        private CommandArgs _args;

        protected override void OnValidate()
        {
            ValidateArguments<CommandArgs>();
        }

        protected override void OnMap()
        {
            _args = new CommandArgs
            {
                Option1 = ToBool("--opt1"),
                Option2 = ToInt("--opt2")
            };
        }



        IEnumerable<CommandLineOption> GetOptionDescriptors<TOption>()
            => CustomAttributes.getSingle<CommandLineOption>(typeof(TOption)).Values;



        private void ValidateArguments<TArgs>()
        {
            // Get Option descriptors from class (attribute) definition.
            IEnumerable<CommandLineOption> optionDescriptors = GetOptionDescriptors<TArgs>();
            ValidateArguments(new ArgsDescriptors(optionDescriptors));
        }


        /// <summary>
        ///     Run a standard argument validation.
        /// </summary>
        /// <remarks>
        ///     Check if provided arguments meet the requirements of
        ///     an <see cref="ArgsDescriptors">argument descriptor</see>.<br />
        /// </remarks>
        private void ValidateArguments(ArgsDescriptors descriptors)
        {
            // Check if all mandatory parameters are there
            var x = descriptors.OptionDescriptors
                .Where(od => od.Mandatory)
                .Select(od => od.Tags).ToArray();
            CheckMandatory(x);
        }



        protected override void OnExecute()
        {

            Console.WriteLine(">>> Start Functionality");
            Console.WriteLine($"\t{nameof(CommandArgs.Option1)}={_args.Option1}");
            Console.WriteLine($"\t{nameof(CommandArgs.Option2)}={_args.Option2}");
            Console.WriteLine($"\t{nameof(CommandArgs.Option3)}={_args.Option3}");
            Console.WriteLine("<<< End Functionality");
        }
    }
}