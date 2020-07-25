using System;
using MSPro.CLArgs;



namespace Level2.ExtendedParameters
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main()");
            Commander.ExecuteCommand(args, new Settings
            {
                // Under Commander.Settings.CommandResolver you can provide your 
                // own CommandResolver or use the default: 
                //     new AssemblyCommandResolver(Assembly.GetEntryAssembly());
                // to find all classes with [Command] annotation in the EntryAssembly!
                AutoResolveCommands = true,
                // Ignore case for command-line options and verbs
                IgnoreCase = true
            });
            Console.WriteLine("<<< End Main()");
        }



        [Command("Default")]
        private class CommandBase : CommandBase<CommandParameters>
        {
            protected override void OnExecute(CommandParameters p)
            {
                Console.WriteLine($"UserName: {p.DbConnection.UserName}");
                Console.WriteLine($"DatabaseTableName: {p.DatabaseTableName}");
            }
        }
    }
}