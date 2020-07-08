using System;
using System.Collections.Generic;
using System.Linq;



namespace MSPro.CLArgs.Sample2.Command2
{
    internal class Command : CommandBase
    {
        public Command(Arguments arguments) : base(arguments)
        {
        }


        protected override void OnValidate()
        {
            //ValidateArguments( new CommandArgsDescriptors());  
            ValidateArguments<CommandArgs>( );
            // In case parsing of any argument failed, there is
            // an entry in the ValidationErrors list.
            // The CommandBase implementation won't run 'OnExecute()' in such case.
        }



        IEnumerable<CommandLineOptionAttribute> GetOptionDescriptors<TOption>()
            => CustomAttributes.getSingle<CommandLineOptionAttribute>(typeof(TOption)).Values;



        private void ValidateArguments<TArgs>()
        {
            // Get Option descriptors from class (attribute) definition.
            IEnumerable<CommandLineOptionAttribute> optionDescriptors = GetOptionDescriptors<TArgs>();  
            ValidateArguments( new ArgsDescriptors(optionDescriptors));
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
            var x= descriptors.OptionDescriptors
                .Where( od=> od.Mandatory)
                .Select( od => od.Tags).ToArray();
            CheckMandatory(x);
        }



        protected override void OnExecute()
        {
            var args = new CommandArgs
            {
                Option1 = ToBool("--opt1"),
                Option2 = ToInt("--opt2")
            };
            Console.WriteLine(">>> Start Functionality");
            Console.WriteLine($"\t{nameof(CommandArgs.Option1)}={args.Option1}");
            Console.WriteLine($"\t{nameof(CommandArgs.Option2)}={args.Option2}");
            Console.WriteLine($"\t{nameof(CommandArgs.Option3)}={args.Option3}");
            Console.WriteLine("<<< End Functionality");
        }
    }
}