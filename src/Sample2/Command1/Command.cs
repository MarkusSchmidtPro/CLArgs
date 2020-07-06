using System;
using MSPro.CLArgs.Sample2.Functionality1;



namespace MSPro.CLArgs.Sample2.Command1
{
    /// <summary>
    /// A functionality example that only displays all expected arguments.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <remarks>
    /// <see cref="OnValidate"/> how to validate and parse command-line arguments.<br/>
    /// <see cref="OnExecute"/> for the functionality.
    /// </remarks>
    internal class Command : CommandBase
    {
        private CommandArgs _args;

        public Command(Arguments arguments) : base(arguments)
        {
        }



        /*  Instead of the simple check you may
            alternatively use the CommandBase implementation or use the 'full-version'
            
                if( !Arguments.Options.ContainsKey("opt1"))
                    ValidationErrors.AddError("opt1", "The mandatory command-line argument 'opt1' was not provided.");
                if( !Arguments.Options.ContainsKey("opt1"))
                    ValidationErrors.AddError("opt1", "The mandatory command-line argument 'opt2' was not provided.");

                // After 'full'-checking 
                // you can probably stop parsing, 
                // because of incomplete argument-list
                
                if (ValidationErrors.HasErrors()) return;

  

          * You may implement arguments dependency check here as well.
          * For example, if 'opt1' is provided, 'opt4' is also required
          *  if( Arguments.Options.ContainsKey("opt1") && !Arguments.Options.ContainsKey("opt4"))
          *      ValidationErrors.AddError("opt4", "Command-line argument 'opt4' is mandatory if 'opt1' is set.");
         */



        protected override void OnValidate()
        {
            // Simple check for mandatory arguments.
            if (!CheckMandatory( "--opt1", "--opt2")) return;

            // Parse all arguments and collect errors ValidationErrors
            _args = new CommandArgs
            {
                Option1 = ToBool("--opt1"),
                Option2 = ToInt("--opt2")
            };

            // In case parsing of any argument failed, there is
            // an entry in the ValidationErrors list.
            // The CommandBase implementation won't run 'OnExecute()' in such case.
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