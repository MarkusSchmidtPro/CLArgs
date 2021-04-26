using System;
using MSPro.CLArgs;



namespace CLArgs.Sample.SimpleAsThat
{
    /// <summary>
    ///     See three was how to use CLArgs.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     The test command-line for this example.
        ///     > --country and --count  : are Options passed to the Command.
        /// </summary>arguments.Verbs.Any
        /// <see cref="HelloWorldParameters" />
        /// <see cref="HelloWorldCommand" />
        private const string COMMAND_LINE = "--country=Germany --count=3";
        //private const string COMMAND_LINE = "HelloWorld --help" ; 

 
        private static void Main(string[] args)
        {
            // --- Use Demo command-line ---
            Console.WriteLine($"Command-Line: {COMMAND_LINE}");
            args =   Helper.SplitCommandLine( COMMAND_LINE);
            // ------------------------------------------------

            Console.WriteLine(">>> Option 1");
            // ONE SINGLE LINE does it all!
            // a) Resolve default [Command] implementation: 
            //    uses: Settings.AutoResolveCommands = true
            //    there is only one in this example : HelloWorldCommand
            // b) Convert command-line arguments
            //    into an object of type            : HelloWorldParameters
            // c) Execute the Command:
            //    HelloWorldCommand( HelloWorldCommand p)
            
            Commander.ExecuteCommand( args);
            
            
            // Control Commander by providing a Settings object
            // Commander.ExecuteCommand( args, new Settings{ AutoResolveCommands = true } );
            
            
            /*
            Console.WriteLine(">>> Option 2");
            // a) Parse CommandLine
            // b) Create instance of a Command: CommandBase<HelloWorldParameters>
            //    b1) Let CLArgs convert the args into HelloWorldParameters
            Arguments arguments = CommandLineParser.Parse(args);
            var cmd = new HelloWorldCommand();
            cmd.Execute(arguments);
            
            Console.WriteLine(">>> Option 3");
            Commander commander = new Commander(new Settings { AutoResolveCommands = false});
            commander.RegisterCommandFactory("TheOneAndOnlyCommand", () => new HelloWorldCommand());
            commander.ExecuteCommand( arguments);
            */
        }
    }



    internal class HelloWorldParameters
    {
        [OptionDescriptor("country", "c", Required = true, HelpText = "The country you're sending greetings to.")]
        public string Country { get; set; }

        [OptionDescriptor("count", Required = false, Default = 1,
                          HelpText = "This is a long story short\n"+
                                     "Provide a number how often the country\n" +
                                     "should receive a 'HelloWorld'.")]
        public int Count { get; set; }
    }



    /// <summary>
    ///     Implement the HelloWorld functionality
    /// </summary>
    [Command("HelloWorld", "This is the help text of my HelloWorld command.")]
    internal class HelloWorldCommand : CommandBase<HelloWorldParameters>
    {
        protected override void Execute(HelloWorldParameters ps)
        {
            for (int i = 0; i < ps.Count; i++)
                Console.WriteLine($"Hello {ps.Country}!");
        }
    }
}