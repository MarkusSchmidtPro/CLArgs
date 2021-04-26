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
        private const string COMMAND_LINE = "verb1 verb2 VERB2 --FILENAME=\"def\" --fileName=\"c:\\myfile.csv\" --target=\"XML 1\" --lines=7 targetfile.xml";


        /// <summary>
        /// Parse the command-line args
        /// and print Verbs and Options.
        /// </summary>
        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            // --- Use Demo command-line ---
            args = Helper.SplitCommandLine(COMMAND_LINE);
            // ------------------------------------------------
            
            CommandLineArguments arguments = CommandLineParser.Parse(args);
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
            
            foreach (string verb in arguments.Verbs)
            {
                Console.WriteLine($"Verb='{verb}'");
            }

            foreach (Option option in arguments.Options)
            {
                Console.WriteLine($"Options[{option.Key}] = '{option.Value}'");
            }
            
            foreach (string target in arguments.Targets)
            {
                Console.WriteLine($"Target: {target}");
            }
            
            // ------------------------------------------------
            Console.WriteLine("<<< End Main");
        }
    }
}