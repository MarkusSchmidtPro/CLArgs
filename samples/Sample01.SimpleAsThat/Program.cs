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
        /// </summary>
        /// <see cref="HelloWorldParameters" />
        /// <see cref="HelloWorldCommand" />
        private const string COMMAND_LINE = "--country=Germany --count=3";

 
        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            // --- Use Demo command-line ---
            Console.WriteLine($"Command-Line: {COMMAND_LINE}");
            args = COMMAND_LINE.Split(' ');
            // ------------------------------------------------

            Console.WriteLine(">>> Option 1");
            // a) Parse CommandLine
            // b) Create instance of a Command: CommandBase<HelloWorldParameters>
            //    b1) Let CLArgs convert the args into HelloWorldParameters
            Arguments arguments = CommandLineParser.Parse(args);
            var cmd = new HelloWorldCommand();
            cmd.Execute(arguments);
            
            Console.WriteLine(">>> Option 2");
            Commander commander = new Commander(new Settings { AutoResolveCommands = false});
            commander.RegisterCommandFactory("TheOneAndOnlyCommand", () => new HelloWorldCommand());
            commander.ExecuteCommand( arguments);
            
            Console.WriteLine(">>> Option 3");
            // ONE SINGLE LINE does it all!
            // a) Resolve default [Command] implementation: 
            //    uses: Settings.AutoResolveCommands = true
            //    there is only one in this example : HelloWorldCommand
            // b) Convert command-line arguments
            //    into an object of type            : HelloWorldParameters
            // c) Execute the Command:
            //    HelloWorldCommand( HelloWorldCommand p)
            
            Commander.ExecuteCommand(args);

            // ------------------------------------------------
            Console.WriteLine("<<< End Main");
        }
    }



    internal class HelloWorldParameters
    {
        [OptionDescriptor("country", "c", Required = true)]
        public string Country { get; set; }

        [OptionDescriptor("count", Required = false, Default = 1)]
        public int Count { get; set; }
    }



    /// <summary>
    ///     Implement the HelloWorld functionality
    /// </summary>
    [Command("HelloWorld")]
    internal class HelloWorldCommand : CommandBase<HelloWorldParameters>
    {
        protected override void Execute(HelloWorldParameters ps)
        {
            for (int i = 0; i < ps.Count; i++)
                Console.WriteLine($"Hello {ps.Country}!");
        }
    }
}