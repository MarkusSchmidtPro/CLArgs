using System;



namespace MSPro.CLArgs.Sample2
{
    internal class ThisAppFunctionality : CommandBase
    {
        public ThisAppFunctionality(Arguments arguments) : base(arguments)
        {
        }



        class ThisAppArgs
        {
            public bool Option1 { get; set; }
            public int Option2 { get; set; }
            public string Option3 { get; set; }
        }



        private ThisAppArgs _args;





        protected override void OnValidate()
        {
            // Simple check for mandatory arguments.
            if (!CheckMandatory("--opt1", "--opt2")) return;

            /*  Alternatively use the CommandBase implementation or use the 'full-version'
                if( !Arguments.Options.ContainsKey("opt1"))
                    ValidationErrors.AddError("opt1", "The mandatory command-line argument 'opt1' was not provided.");
                if( !Arguments.Options.ContainsKey("opt1"))
                    ValidationErrors.AddError("opt1", "The mandatory command-line argument 'opt2' was not provided.");
                // we can stop parsing, because of incomplete argument-list
                if (ValidationErrors.HasErrors()) return;
             -------------------------------------------------------
             * You may implement arguments dependency check here as well.
             * For example, if 'opt1' is provided, 'opt4' is also required
             *  if( Arguments.Options.ContainsKey("opt1") && !Arguments.Options.ContainsKey("opt4"))
             *      ValidationErrors.AddError("opt4", "Command-line argument 'opt4' is mandatory if 'opt1' is set.");
            */

            // Console.WriteLine("All mandatory arguments present.");

            //
            // Parse all arguments and collect errors ValidationErrors
            //
            _args = new ThisAppArgs
            {
                Option1 = ToBool("--opt1"),
                Option2 = ToInt("--opt2")
            };
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