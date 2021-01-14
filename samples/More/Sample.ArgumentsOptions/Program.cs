using System;
using MSPro.CLArgs;



namespace CLArgs.Sample.ArgumentsOptions
{
    /// <summary>
    ///     CLArgs example to demonstrate Options basics. 
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     The test command-line for this example.
        /// </summary>
        private const string COMMAND_LINE = "--target='XML 1' --lines=7";

        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            Console.WriteLine($"Command-Line: {COMMAND_LINE}");
            args = COMMAND_LINE.Split(' ');
            // ------------------------------------------------

            CommandLineArguments commandLineArguments = CommandLineParser.Parse(args);
    
            // Check if the 'fileName' option exist - was provided in the command-line
            // E.g. --fileName=.. /fileName:...
            const string FILENAME_TAG = "fileName";
            bool fileNameProvided = commandLineArguments.OptionTagProvided(FILENAME_TAG);
            Console.WriteLine($"*** Option '{FILENAME_TAG}' was provided in the command-line: {fileNameProvided}");
            
            // Set default value if not provided
            // Upsert = Update or Insert = Update or Add 
            if (!fileNameProvided)
                commandLineArguments.AddOption(FILENAME_TAG, "default.txt");
            
            foreach ( Option option in commandLineArguments.Options)
            {
                Console.WriteLine($"Options[{option.Key}] = '{option.Value}'");
            }
            
            // ------------------------------------------------
            Console.WriteLine("<<< End Main");
        }
    }
}