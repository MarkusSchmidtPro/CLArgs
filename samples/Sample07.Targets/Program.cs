using System;
using System.Collections.Generic;
using System.IO;
using MSPro.CLArgs;



namespace CLArgs.Sample.ValueConversion
{
    /// <summary>
    /// Demo how to use Targets in your command-line
    /// </summary>
    /// <remarks>
    /// Besides <c>Verbs</c> and <c>Options</c>, <c>Targets</c> are simply
    /// a list of tokens that will be recognized. <c>Targets</c> are used,
    /// for example, to provide a list of files.
    /// </remarks>
    internal class Program
    {
        // Verb, Options and three Arguments
        private const string COMMAND_LINE = "MyVerb --Option0=DirectArgs --Option1=1 File1.txt File2.txt 'data\\File2.txt'";

        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            Console.WriteLine($"Command-Line: {COMMAND_LINE}");
            args = COMMAND_LINE.Split(' ');
            // ------------------------------------------------

            CommandLineArguments arguments = CommandLineParser.Parse( args);
            foreach (string target in arguments.Targets)
            {
                Console.WriteLine($"Target: {target}");
            }
            
            // ------------------------------------------------
            Console.WriteLine("<<< End Main()");
        }

    }
}