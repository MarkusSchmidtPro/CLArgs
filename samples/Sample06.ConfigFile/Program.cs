using System;
using MSPro.CLArgs;



namespace CLArgs.Sample.ValueConversion
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
            args = COMMAND_LINE.Split(' ');
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
            [OptionDescriptor("Option0")]
            public string Option0 { get; set; }    

            [OptionDescriptor("Option1")]
            public string Option1 { get; set; } 
            
            [OptionDescriptor("Option2")]
            public int Option2 { get; set; }
        }
    }
}