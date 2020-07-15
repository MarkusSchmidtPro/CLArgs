using System;
using System.Collections.Generic;
using MSPro.CLArgs;



namespace Sample1
{
    /// <summary>
    ///     Represent your application and your functionality.
    /// </summary>
    /// <remarks>
    ///    Use CLArgs to parse your command-line.
    /// </remarks>
    class Program
    {
        static void Main(string[] args)
        {
            Arguments arguments = CommandLine.Parse(args);

            //
            // Functionality: Display arguments
            // 
            Console.WriteLine(">>> Start Functionality");
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");

            for (int i = 0; i < arguments.Verbs.Count; i++)
            {
                string verb = arguments.Verbs[i];
                Console.WriteLine($"Verb[{i}] = '{verb}'");
            }

            foreach (KeyValuePair<string,OptionTag> option in arguments.Options)
            {
                Console.WriteLine($"Options[{option.Key}] = '{option.Value}'");
            }
            
            Console.WriteLine("<<< End Functionality");
        }
    }
}
