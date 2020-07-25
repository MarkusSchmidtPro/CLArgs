using System;
using MSPro.CLArgs;



namespace Sample1.SimpleAsThat
{
    internal static class Program
    {
        /// <summary>
        ///     The test command-line for this example.
        ///     > HelloWorld : is a Verb - that determines which Command is executed.
        ///     > --country  : is an Option that is passed to the Command.
        /// </summary>
        private const string COMMAND_LINE = "HelloWorld --country=Germany";



        /// <summary>
        ///     The easiest way to use CLArgs.
        /// </summary>
        /// <remarks>
        ///     The <see cref="Commander" /> implementation will, by default,
        ///     automatically resolve all classes in the Entry Assembly
        ///     which implement <see cref="ICommand" /> and which are
        ///     annotated with a <see cref="CommandAttribute">[Command]</see>-Attribute.
        /// </remarks>
        /// <seealso cref="Settings.AutoResolveCommands" />
        private static void MainDemo(string[] args)
        {
            Commander.ExecuteCommand(args);
        }



        #region Application Startup

        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Functionality");
            // Use Demo command-line
            args = COMMAND_LINE.Split(' ');
            MainDemo(args);
            Console.WriteLine("<<< End Functionality");
        }

        #endregion
    }



    internal class HelloWorldParameters
    {
        [OptionDescriptor("country", "c", Required = true, Default = "No country for old men")]
        public string Country { get; set; }
    }



    [Command("HelloWorld")]
    internal class HelloWorldCommandBase : CommandBase<HelloWorldParameters>
    {
        protected override void OnExecute(HelloWorldParameters p)
        {
            Console.WriteLine($"Hello {p.Country}!");
        }
    }
}