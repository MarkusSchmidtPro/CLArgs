using System;
using System.Diagnostics;
using MSPro.CLArgs;



namespace CLArgs.Sample.Verbs
{
    /// <summary>
    ///     Demonstrates how to manually register verb-functionality.
    /// </summary>
    /// <remarks>
    ///     A <c>Verb</c> normally tells your application to <c>do</c> something.
    ///     A <c>Verb</c> specifies which functionality should be executed.
    /// </remarks>
    internal static class Program
    {
        /// <summary>
        ///     The test command-line for this example.
        /// </summary>
        private const string COMMAND_LINE = "word1 text2 verb3";



        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            Console.WriteLine($"Command-Line: {COMMAND_LINE}");
            args = COMMAND_LINE.Split(' ');
            // ------------------------------------------------

             
            Console.WriteLine("--- Commander resolution");
            Commander commander = new Commander(new Settings
            {
                AutoResolveCommands = false
            });
            commander.RegisterFunction("word1", word);
            commander.RegisterFunction("word1.text2", text);
            commander.RegisterFunction("word1.text2.verb3", verb);
            
            CommandLineArguments commandLineArguments = CommandLineParser.Parse(args);
            commander.ExecuteCommand(commandLineArguments);
            
            Console.WriteLine("--- Manual resolution");
            completelyManual(args);

            // ------------------------------------------------
            Console.WriteLine("<<< End Main");
        }



        #region Completely manual

        /// <summary>
        ///     Manual resolution of functionality by Verb.
        /// </summary>
        private static void completelyManual(string[] args)
        {
            CommandLineArguments commandLineArguments = CommandLineParser.Parse(args);
            if (commandLineArguments.VerbPath == "word1")
                word(commandLineArguments);
            else if (commandLineArguments.VerbPath == "word1.text2")
                text(commandLineArguments);
            else if (commandLineArguments.VerbPath == "word1.text2.verb3")
                verb(commandLineArguments);
            else
                Debug.Fail("Don't know what to do!");
        }



        private static void word(CommandLineArguments commandLineArguments) => Console.WriteLine("Function Word");
        private static void text(CommandLineArguments commandLineArguments) => Console.WriteLine("Function Text");
        private static void verb(CommandLineArguments commandLineArguments) => Console.WriteLine("Function Verb");

        #endregion
    }
}