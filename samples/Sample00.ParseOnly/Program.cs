using System;
using MSPro.CLArgs;



namespace CLArgs.Sample.ParseOnly
{
    /// <summary>
    ///     Simply parse your command-line arguments.
    /// </summary>
    internal static class Program
    {
        
        /// <summary>
        /// The test command-line for this example.
        /// </summary>
        private const string COMMAND_LINE = "verb1 verb2 --fileName='c:\\myfile.csv' --target='XML 1' --lines=7";


        /// <summary>
        /// Parse the command-line args
        /// and print Verbs and Options.
        /// </summary>
        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            // --- Use Demo command-line ---
            args = COMMAND_LINE.Split(' ');
            // ------------------------------------------------
            
            CommandLineArguments commandLineArguments = CommandLineParser.Parse(args);
            Console.WriteLine($"Command-Line: {commandLineArguments.CommandLine}");
            
            foreach (string verb in commandLineArguments.Verbs)
            {
                Console.WriteLine($"Verb='{verb}'");
            }

            foreach (Option option in commandLineArguments.Options)
            {
                Console.WriteLine($"Options[{option.Key}] = '{option.Value}'");
            }
            
            // ------------------------------------------------
            Console.WriteLine("<<< End Main");
        }
    }
}