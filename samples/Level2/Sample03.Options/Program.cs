using System;
using CLArgs.Sample.Options.DefaultCommand;
using MSPro.CLArgs;



namespace CLArgs.Sample.Options
{
    /// <summary>
    ///     Options are provided in the command-line.
    ///     See here how Options are converted to Parameters.
    ///     Parameters is the strongly typed object that is passed to a Command.
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     The test command-line for this example.
        /// </summary>
        /// <remarks>
        ///     Verb:    None -> use Default Command - there is only one in this example!
        ///     Options: <see cref="DefaultCommand.Parameters" /> class how these options are turned into Parameters.
        /// </remarks>
        private const string COMMAND_LINE = "--T=[dbo].IndexTable --u=msc --p=_ab123_ --BaseSetting";



        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            Console.WriteLine($"Command-Line: {COMMAND_LINE}");
            args = COMMAND_LINE.Split(' ');
            // ------------------------------------------------

            //
            // The easiest way: use the static Commander.ExecuteCommand()
            //
            // Commander auto resolution will resolve the one and only 
            // class with [Command("DemoCommand")] annotation.
            // This will become the ' DefaultCommand'.
            // Then Commander will convert the command-line args into an
            // DefaultCommand.Parameters object and execute the Command.
            //
            Console.WriteLine("--- Minimum Code");
            Commander.ExecuteCommand(args, new Settings {IgnoreCase = true});

            //
            // Alternative way of doing it with manual command registration
            // and the chance to 'tweak' the provided Options
            // Option conversion remains the same: nothing to do for it.
            //
            Console.WriteLine("--- The alternative way - using an explicit Commander instance ");
            Commander commander = new Commander(new Settings {IgnoreCase = true, AutoResolveCommands = false});
            commander.RegisterCommandFactory("TheOneAndOnlyCommand", () => new Command());

            // Let's change the argument from commands-line 
            Arguments arguments = CommandLineParser.Parse(args);
            arguments.SetOption("DatabaseTableName", "AnotherTable");
            
            // Execute the Command with slightly different Options
            commander.ExecuteCommand(arguments);

            Console.WriteLine("<<< End Main()");
        }
    }
}