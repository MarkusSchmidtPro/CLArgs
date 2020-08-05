using System;
using MSPro.CLArgs;



namespace CLArgs.Sample.SimpleAsThat
{
    /// <summary>
    ///     The easiest way to use CLArgs.
    /// </summary>
    /// <remarks>
    ///     Let the <see cref="Commander" /> automatically
    ///     resolve all classes in the Entry Assembly
    ///     which inherit from <see cref="CommandBase{TParam}" /> and which are
    ///     annotated with a <see cref="CommandAttribute">[Command]</see>-Attribute.<br />
    ///     <br />
    ///     You can configure <see cref="Settings.AutoResolveCommands" >command resolution</see>
    ///     and many other thins by using <see cref="Settings" />. 
    /// </remarks>
    internal static class Program
    {
        /// <summary>
        ///     The test command-line for this example.
        ///     > --country and --count  : are Options passed to the Command.
        /// </summary>
        /// <see cref="HelloWorldParameters" />
        /// <see cref="HelloWorldCommand" />
        private const string COMMAND_LINE = "--country=Germany --count=3";
        //private const string COMMAND_LINE = "HelloWorld --help" ; 

 
        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            // --- Use Demo command-line ---
            Console.WriteLine($"Command-Line: {COMMAND_LINE}");
            args = COMMAND_LINE.Split(' ');
            // ------------------------------------------------

            // ONE SINGLE LINE does it all!
            
            // a) Resolve default [Command] implementation,
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
        [OptionDescriptor("country", "c", Required = true, Description = "The country you're sending greetings to.")]
        public string Country { get; set; }

        [OptionDescriptor("count", Required = false, Default = 1,
                          Description = "This is a long story short\n"+
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