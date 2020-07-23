using System;
using System.IO;
using MSPro.CLArgs;



namespace Level2.ExtendedParameters
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Under Commander.Settings.CommandResolver you can provide your 
            // own CommandResolver or use the default: 
            //     new AssemblyCommandResolver(Assembly.GetEntryAssembly());
            // to find all classes with [Command] annotation in the EntryAssembly!
            Commander.Settings.AutoResolveCommands = true;
            
            // Ignore case for command-line options and verbs
            Commander.Settings.IgnoreCase = true;
            Commander.Settings.IgnoreUnknownOptions = true;
            Arguments arguments = Commander.ParseCommandLine(args);
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
            Console.WriteLine(">>> Start Main()");
            Commander.ExecuteCommand(arguments);
            Console.WriteLine("<<< End Main()");
        }



        [Command("Default")]
        private class Command : CommandBase<CommandParameters>
        {
            protected override void OnExecute(CommandParameters p)
            {
                Console.WriteLine($"UserName: {p.DbConnection.UserName}");
                Console.WriteLine($"DatabaseTableName: {p.DatabaseTableName}");
            }
        }
    }
}