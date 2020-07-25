using System;
using MSPro.CLArgs;



namespace Sample1
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
        /// <param name="args"><see cref="COMMAND_LINE"/> split into <paramref name="args"/>.</param>
        private static void MainDemo(string[] args)
        {
            Arguments arguments = CommandLineParser.Parse(args);
            foreach (string verb in arguments.Verbs)
            {
                Console.WriteLine($"Verb='{verb}'");
            }

            foreach (Option option in arguments.Options)
            {
                Console.WriteLine($"Options[{option.Key}] = '{option.Value}'");
            }
        }
        
        

        #region Application Startup

        private static void Main(string[] args)
        {
            // Use Demo command-line
            args = COMMAND_LINE.Split(' ');

            Console.WriteLine(">>> Start Functionality");
            MainDemo(args);
            Console.WriteLine("<<< End Functionality");
        }

        #endregion
    }
}