using System;
using System.IO;
using MSPro.CLArgs;



namespace Level2.ExtendedParameters
{
    internal partial class Program
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
                Console.WriteLine($"UserName: {p.DBConnection.UserName}");
                Console.WriteLine($"DatabaseTableName: {p.DatabaseTableName}");
            }
        }
    }
}