using System;
using System.Collections.Generic;
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
            Arguments arguments = CommandLine.Parse(args);
            Console.WriteLine(">>> Start Functionality");
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
    
            // Check if the 'fileName' option exist - was provided in the command-line
            // E.g. --fileName=.. /fileName:...
            const string OPTION_NAME = "fileName";
            bool fileNameProvided = arguments.Options.ContainsKey(OPTION_NAME);
            Console.WriteLine($"Option '{OPTION_NAME}' was provided in the command-line: {fileNameProvided}");
            
            // Set default value if not provided
            // Upsert = Update or Insert = Update or Add 
            if (!fileNameProvided)
                arguments.UpsertOption(OPTION_NAME, "default.txt");
            
            foreach (KeyValuePair<string, OptionTag> option in arguments.Options)
            {
                Console.WriteLine($"Options[{option.Key}] = '{option.Value}'");
            }
            Console.WriteLine("<<< End Functionality");
        }
    }
}