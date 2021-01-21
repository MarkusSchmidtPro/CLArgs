using System;
using System.Collections.Generic;
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
        public char[] OptionValueTags = { ' ', ':', '=' };

        public ValueConverters ValueConverters { get; } = new();

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
        ///     Automatically resolve commands using <see cref="CommandResolver" />
        /// </summary>
        public bool AutoResolveCommands { get; set; } = true;

        /// <summary>
        ///     Get or set an object to resolve all known commands (and verbs).
        /// </summary>
        /// <remarks>
        ///     The default resolver is
        ///     <see cref="AssemblyCommandResolver" />(<b>Assembly.GetEntryAssembly()</b>),
        ///     to find all classes with [Command] annotation in the <c>EntryAssembly</c>.
        /// </remarks>
        public ICommandResolver CommandResolver { get; set; } =
            new AssemblyCommandResolver(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Get or set tags which identify an option.
        /// </summary>
        /// <remarks>
        ///     A command-line argument that starts with any of these character
        ///     is considered to be an <c>Option</c>.
        /// </remarks>
        public char[] OptionsTags { get; set; } = { '-', '/' };


        const int HELP_ALIGN_COLUMN = 25;
        const int HELP_FULL_WIDTH = 100;

        /// <summary>
        ///     Display a help text for all commands.
        /// </summary>
        /// <remarks>
        ///     This method is called when you application is called without any parameter.
        /// </remarks>
        public DisplayAllCommandsDescription DisplayAllCommandsDescription { get; set; } = commandDescriptors =>
        // Default Implementation
        {
            Console.WriteLine($"{commandDescriptors.Count} Commands available.");
            foreach (CommandDescriptor commandDescriptor in commandDescriptors)
            {
                Console.WriteLine("");
                var wrapped = Helper.Wrap(commandDescriptor.Description, HELP_FULL_WIDTH);
                Console.WriteLine($"{commandDescriptor.Verb.Replace('.', ' '),-HELP_ALIGN_COLUMN}{wrapped.AllLines[0]}");
                for (int lineNo = 1; lineNo < wrapped.AllLines.Length; lineNo++)
                    Console.WriteLine($"{" ",HELP_ALIGN_COLUMN}{wrapped.AllLines[lineNo]}");
            }
        };


        /// <summary>
        ///     Display a formatted help text for a single Command.
        /// </summary>
        /// <remarks>
        ///     This method is called when a VERB was specified and help for this particular verb should be printed.<br />
        ///     The <see cref="CommandDescriptor">Command's descriptor</see> is passed to the method (the Command is specified by
        ///     the Verb).
        /// </remarks>
        public DisplayCommandHelp DisplayCommandHelp { get; set; } = commandDescriptor =>
        {
            Console.WriteLine();

            var wrappedDesc = Helper.Wrap(commandDescriptor.Description, HELP_FULL_WIDTH - HELP_ALIGN_COLUMN);
            Console.WriteLine($"{commandDescriptor.Verb.Replace('.', ' '),-HELP_ALIGN_COLUMN}{wrappedDesc.AllLines[0]}");
            for (int lineNo = 1; lineNo < wrappedDesc.AllLines.Length; lineNo++)
                Console.WriteLine($"{" ",HELP_ALIGN_COLUMN}{wrappedDesc.AllLines[lineNo]}");

            Console.WriteLine($"{new string('-', HELP_FULL_WIDTH)}");
            var command = commandDescriptor.CreateCommandInstance();
            foreach (OptionDescriptorAttribute oda in command.OptionDescriptors)
            {
                string tags = oda.Tags!= null?  $"Tags={string.Join(",", oda.Tags)} ": string.Empty;
                string required = oda.Required ? "required" : "optional" +" ";
                string split = !string.IsNullOrWhiteSpace(oda.AllowMultipleSplit) ? $" Split='{oda.AllowMultipleSplit}'" : string.Empty;
                string allowMultiple = !string.IsNullOrWhiteSpace(oda.AllowMultiple) ? $"AllowMultiple={oda.AllowMultiple != null}{split}" : string.Empty;
                Console.WriteLine($"/{oda.OptionName,-HELP_ALIGN_COLUMN + 1}{tags}{required}{allowMultiple}");
                var wrapped = Helper.Wrap(oda.HelpText, HELP_FULL_WIDTH - HELP_ALIGN_COLUMN);
                foreach (string line in wrapped.AllLines) Console.WriteLine($"{" ",HELP_ALIGN_COLUMN}{line}");
                if( oda.Default!= null) Console.WriteLine($"{" ",HELP_ALIGN_COLUMN}DEFAULT: '{oda.Default}'"); 
                Console.WriteLine();
            }
        };
    }
}