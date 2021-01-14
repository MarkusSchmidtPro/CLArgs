using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    public delegate void DisplayAllCommandsDescription(List<CommandDescriptor> commandDescriptors);
    public delegate void DisplayCommandHelp(CommandDescriptor commandDescriptor);
    
    

    [PublicAPI]
    public class Settings
    {
        /// <summary>
        ///     Get or set a list of characters that mark the end of an option's name.
        /// </summary>
        public char[] OptionValueTags = {' ', ':', '='};

        public TraceLevel TraceLevel { get; set; } = TraceLevel.Off;
        public Action<string> Trace { get; set; } = Console.WriteLine;


        public ValueConverters ValueConverters { get; } = new ValueConverters(); 

        public bool IgnoreCase { get; set; }

        /// <summary>
        ///     Get or set if unknown option tags provided in the command-line should be ignored.
        /// </summary>
        /// <remarks>
        ///     If set to <c>true</c> unknown options are ignored.<br />
        ///     Otherwise an error is added to the <see cref="ErrorDetailList">error collection</see>.
        /// </remarks>
        public bool IgnoreUnknownOptions { get; set; }

        /// <summary>
        ///      Automatically resolve commands using <see cref="CommandResolver" />
        /// </summary>
        public bool AutoResolveCommands { get; set; } = true;

        /// <summary>
        ///     Get or set an object to resolve all known commands (and verbs).
        /// </summary>
        /// <remarks>
        /// The default resolver is
        ///    <see cref="AssemblyCommandResolver"/>(<b>Assembly.GetEntryAssembly()</b>),
        /// to find all classes with [Command] annotation in the <c>EntryAssembly</c>.
        /// </remarks>
        public ICommandResolver CommandResolver { get; set; } =
            new AssemblyCommandResolver(Assembly.GetEntryAssembly());


        /// <summary>
        /// Resolves to location of a provided configuration file with relative path (@fileName).
        /// </summary>
        /// <remarks>
        ///     The default
        /// </remarks>
        public IConfigFileResolver ConfigFileResolver { get; set; } = new ConfigFileResolver();

        /// <summary>
        ///     Get or set tags which identify an option.
        /// </summary>
        /// <remarks>
        ///     A command-line argument that starts with any of these character
        ///     is considered to be an <c>Option</c>.
        /// </remarks>
        public char[] OptionsTags { get; set; } = {'-', '/'};


        public DisplayAllCommandsDescription DisplayAllCommandsDescription { get; set; } = commandDescriptors =>
        // Default Implementation
        {
            Console.WriteLine( $"{commandDescriptors.Count} Commands available.");
            foreach (CommandDescriptor commandDescriptor in commandDescriptors)
            {
                Console.WriteLine( $"{commandDescriptor.Verb}\t\t{commandDescriptor?.Description}");
            }
        };

        public DisplayCommandHelp DisplayCommandHelp { get; set; } = (commandDescriptor) =>
        {
            const int ALIGN_COLUMN = 16;

            string alignSpaces = new string(' ', ALIGN_COLUMN);

            Console.WriteLine();
            Console.WriteLine(commandDescriptor.Verb);
            if (!string.IsNullOrEmpty(commandDescriptor.Description))
            {
                string formattedDescription = commandDescriptor.Description.Replace("\n", $"\n  ");
                Console.WriteLine($"  {formattedDescription}");
            }
            Console.WriteLine("----------------------------------------------");
            var command = commandDescriptor.CreateCommandInstance();
            foreach (OptionDescriptorAttribute oda in command.OptionDescriptors)
            {
                string tags = string.Join(",", oda.Tags);
                Console.WriteLine($"\t{oda.OptionName,-ALIGN_COLUMN}Tags={tags}, Required={oda.Required}, Default={oda.Default??"null"}");
                if (!string.IsNullOrEmpty(oda.HelpText))
                {
                    string formattedDescription1 = oda.HelpText.Replace("\n", $"\n\t{alignSpaces}");
                    Console.WriteLine($"\t{alignSpaces}{formattedDescription1}");
                }
                Console.WriteLine();
            }
        };
    }
}