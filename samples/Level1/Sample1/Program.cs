using System;
using MSPro.CLArgs;



namespace Sample1
{
    /// <summary>
    ///     Represent your application and your functionality.
    /// </summary>
    /// <remarks>
    ///     Use CLArgs to parse your command-line.
    /// </remarks>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Arguments arguments = Commander.ParseCommandLine(args);

            //
            // Functionality: Display arguments
            // [1] = {string} "verb2" View
            Console.WriteLine(">>> Start Functionality");
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");

            foreach (string verb in arguments.Verbs)
            {
                Console.WriteLine($"Verb='{verb}'");
            }

            foreach (Option option in arguments.Options)
            {
                Console.WriteLine($"Options[{option.Key}] = '{option.Value}'");
            }

            Console.WriteLine("<<< End Functionality");
        }
    }
}