using System;
using System.Collections.Generic;
using MSPro.CLArgs;



namespace CLArgs.Sample.ConfigFile
{
    /// <summary>
    /// Demo how to use your custom Option to Parameter converter.
    /// </summary>
    /// <remarks>
    ///    In rare cases, you may have custom types in your Parameters class
    ///     and you want to assign (convert) command-line option values to it.
    /// </remarks>
    internal class Program
    {
        // File NewFile1.Txt must exists (part of this project), otherwise: Exception
        private const string COMMAND_LINE = "--Option0=DirectArgs @addArgs.txt";

        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            Console.WriteLine($"Command-Line: {COMMAND_LINE}");
            args =   Helper.SplitCommandLine( COMMAND_LINE);
            // ------------------------------------------------

            // Execute the Command direct,
            // without Command or registration,
            // by using the Command Execute() method.
            new Command().Execute(CommandLineParser.Parse(args));
            
            // ------------------------------------------------
            Console.WriteLine("<<< End Main()");
        }



        private class Command : CommandBase<CommandParameters>
        {
            protected override void Execute(CommandParameters ps)
            {
                Console.WriteLine($"Option0: {ps.Option0}");
                Console.WriteLine($"Option1: {ps.Option1}");
                Console.WriteLine($"Option2: {ps.Option2}");
            }
        }

        private class CommandParameters
        {
            /// <summary>
            /// This property takes the first occurrence of Option0 in the command-line.
            /// </summary>
            [OptionDescriptor("Option0", AllowMultiple = nameof(AllOptions0))]
            public string Option0 { get; set; }

            /// <summary>
            /// This property takes all 'Option0' tag fro command-line
            /// as it is allowed to specify this option in the option file and in the command-line.
            /// </summary>
            public List<string> AllOptions0 { get; set; } = new List<string>();

            [OptionDescriptor("Option1")]
            public string Option1 { get; set; } 
            
            [OptionDescriptor("Option2")]
            public int Option2 { get; set; }
        }
    }
}