using System;
using System.Collections.Generic;
using System.IO;
using MSPro.CLArgs;



namespace CLArgs.Sample.ValueConversion
{
    /// <summary>
    ///     Demo how to use your custom Option to Parameter converter.
    /// </summary>
    /// <remarks>
    ///     In rare cases, you may have custom types in your Parameters class
    ///     and you want to assign (convert) command-line option values to it.
    /// </remarks>
    internal class Program
    {
        // File NewFile1.Txt must exists (part of this project), otherwise: Exception
        private const string COMMAND_LINE = "--DestType=XML --SourceFile=NewFile1.Txt";



        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            Console.WriteLine($"Command-Line: {COMMAND_LINE}");
            args = COMMAND_LINE.Split(' ');
            // ------------------------------------------------

            new Command().Execute(CommandLineParser.Parse(args));

            // ------------------------------------------------
            Console.WriteLine("<<< End Main()");
        }



        private class Command : CommandBase<CommandParameters>
        {
            protected override void BeforeArgumentConversion(CommandLineArguments commandLineArguments)
            {
                // Register a custom converter from string to FileInfo
                // See TypeConverters() constructor for out-of-the-boy supported types.
                this.Settings.ValueConverters.Register(
                    typeof(FileInfo), fileInfoConverter);
            }



            /// <summary>
            ///     A custom converter to convert a command-line option value - which is a string - into your application's type.
            /// </summary>
            /// <param name="optionValue">The option's value.</param>
            /// <param name="optionName">The option's name.</param>
            /// <param name="targetType">The target type into which the option.value should be converted.</param>
            /// <param name="errors">Add conversion errors to this list (don't throw exceptions).</param>
            /// <returns></returns>
            private object fileInfoConverter(string optionValue, string optionName, Type targetType,
                                             ErrorDetailList errors) =>
                new FileInfo(optionValue);



            protected override void BeforeExecute(
                CommandParameters ps, HashSet<string> unresolvedPropertyNames, ErrorDetailList errors)
            {
                //
                // Ensure promises to Execute: File must exist
                //
                if (!ps.SourceFile.Exists)
                {
                    errors.AddError(nameof(ps.SourceFile),
                                    $"Provided file '{ps.SourceFile.FullName}' does not exists!");
                    // Command.Execute() will not be called
                    // And the default OnError() implementation will throw an Aggregate exception,
                    // See other examples for more information how to handle errors.
                }
            }



            protected override void Execute(CommandParameters ps)
            {
                Console.WriteLine($"FileType: {ps.FileTypes}");
                Console.WriteLine($"FilePath: {ps.SourceFile.FullName}");
            }
        }



        /// <summary>
        ///     Enum properties are supported by default.
        /// </summary>
        private enum FileTypes
        {
            XML,
            JSON
        }



        private class CommandParameters
        {
            // The property's type is an existing enum
            // and conversion int to enum takes place automatically (by CLArgs)

            [OptionDescriptor("DestType", Required = true)]
            public FileTypes FileTypes { get; set; } // requires custom converter


            // CLArgs does not know (out-of-the-box) how to convert an command-line Option
            // into a property type of FileInfo. CLArgs needs a custom converter for this.
            // This converter is registered in the BeforeArgumentConversion() method - see above.

            [OptionDescriptor("SourceFile", Required = false)]
            public FileInfo SourceFile { get; set; }
        }
    }
}