using System;
using System.IO;
using MSPro.CLArgs;



namespace Level2.ValueConversion
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Arguments arguments = CommandLine.Parse(args);
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
            Console.WriteLine(">>> Start Main()");
            ICommand cmd = new Command();
            cmd.Execute(arguments);
            Console.WriteLine("<<< End Main()");
        }



        private class Command : CommandBase<CommandParameters>
        {
            public Command()
            {
                this.TypeConverters.Register(
                    typeof(FileInfo), (propertyName, optionValue) => new FileInfo(optionValue));
            }



            protected override void OnExecute(CommandParameters p)
            {
                Console.WriteLine($"FileType: {p.FileType}");
                Console.WriteLine($"FilePath: {p.SourceFile.FullName}");
            }
        }



        private enum FileType
        {
            XML,
            JSON
        }



        private class CommandParameters
        {
            [OptionDescriptor("DestType", Required = true)]
            public FileType FileType { get; set; }

            [OptionDescriptor("SourceFile", Required = false)]
            public FileInfo SourceFile { get; set; }
        }
    }
}