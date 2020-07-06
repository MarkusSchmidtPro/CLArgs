using System;
using System.Linq;



namespace MSPro.CLArgs.Sample2.Command2
{
    internal class Command : CommandBase
    {
        private ThisAppArgs _args;



        public Command(Arguments arguments) : base(arguments)
        {
        }



        protected override void OnValidate()
        {
            ValidateArguments(new CommandArgsDescriptors());

            // Parse all arguments and collect errors ValidationErrors
            _args = new ThisAppArgs
            {
                Option1 = ToBool("--opt1"),
                Option2 = ToInt("--opt2")
            };

            // In case parsing of any argument failed, there is
            // an entry in the ValidationErrors list.
            // The CommandBase implementation won't run 'OnExecute()' in such case.
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
            // Check if all mandatory parameters are they
            
            var x= descriptors.OptionDescriptors
                .Where( od=> od.Mandatory)
                .Select( od => od.Tags).ToArray();
            CheckMandatory(x);
        }



        protected override void OnExecute()
        {
            Console.WriteLine(">>> Start Functionality");
            Console.WriteLine($"\t{nameof(ThisAppArgs.Option1)}={_args.Option1}");
            Console.WriteLine($"\t{nameof(ThisAppArgs.Option2)}={_args.Option2}");
            Console.WriteLine($"\t{nameof(ThisAppArgs.Option3)}={_args.Option3}");
            Console.WriteLine("<<< End Functionality");
        }
    }
}