using System;
using MSPro.CLArgs;



namespace Level1.Options
{
    /// <summary>
    ///     CLArgs example to demonstrate Options basic. 
    /// </summary>
    /// <remarks>
    ///     See Properties\launchSettings.json for the used command-line.<br/>
    /// </remarks>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Arguments arguments = Commander.ParseCommandLine(args);
            Console.WriteLine(">>> Start Functionality");
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
    
            // Check if the 'fileName' option exist - was provided in the command-line
            // E.g. --fileName=.. /fileName:...
            const string FILENAME_TAG = "fileName";
            bool fileNameProvided = arguments.OptionTagProvided(FILENAME_TAG);
            Console.WriteLine($"Option '{FILENAME_TAG}' was provided in the command-line: {fileNameProvided}");
            
            // Set default value if not provided
            // Upsert = Update or Insert = Update or Add 
            if (!fileNameProvided)
                arguments.AddOption(FILENAME_TAG, "default.txt");
            
            foreach ( Option option in arguments.Options)
            {
                Console.WriteLine($"Options[{option.Key}] = '{option.Value}'");
            }
            Console.WriteLine("<<< End Functionality");
        }
    }
}