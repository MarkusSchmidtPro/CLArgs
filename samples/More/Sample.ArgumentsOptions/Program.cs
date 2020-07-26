using System;
using MSPro.CLArgs;



namespace Sample.ArgumentsOptions
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

            Arguments arguments = CommandLineParser.Parse(args);
    
            // Check if the 'fileName' option exist - was provided in the command-line
            // E.g. --fileName=.. /fileName:...
            const string FILENAME_TAG = "fileName";
            bool fileNameProvided = arguments.OptionTagProvided(FILENAME_TAG);
            Console.WriteLine($"*** Option '{FILENAME_TAG}' was provided in the command-line: {fileNameProvided}");
            
            // Set default value if not provided
            // Upsert = Update or Insert = Update or Add 
            if (!fileNameProvided)
                arguments.SetOption(FILENAME_TAG, "default.txt");
            
            foreach ( Option option in arguments.Options)
            {
                Console.WriteLine($"Options[{option.Key}] = '{option.Value}'");
            }
            
            // ------------------------------------------------
            Console.WriteLine("<<< End Main");
        }
    }
}