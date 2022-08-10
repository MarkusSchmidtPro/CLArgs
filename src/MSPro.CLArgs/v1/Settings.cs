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
        public Settings()
        {
            this.HelpAlignColumn = 20;
            this.HelpFullWidth = 80;
            this.DisplayAllCommandsDescription = displayAllCommandsDescription;
            this.DisplayCommandHelp = displayCommandHelp;
        }


        /// <summary>
        ///     Get or set a list of characters that mark the end of an option's name.
        /// </summary>
        public string[] OptionValueTags { get; set; } = {  ":", "=" };

        public ValueConverters ValueConverters { get; } = new();

        public bool IgnoreCase { get; set; }

        public StringComparison StringComparison => IgnoreCase
            ?  StringComparison.InvariantCultureIgnoreCase
            : StringComparison.InvariantCulture;   
        
        public IEqualityComparer<string> GetStringComparer() => IgnoreCase
            ? StringComparer.InvariantCultureIgnoreCase
            : StringComparer.InvariantCulture;
        
        
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
            new AssemblyCommandResolver( Assembly.GetEntryAssembly());

        /// <summary>
        ///     Get or set tags which identify an option.
        /// </summary>
        /// <remarks>
        ///     A command-line argument that starts with any of these character
        ///     is considered to be an <c>Option</c>.
        /// </remarks>
        public string[] OptionsTags { get; set; } = { "--", "-", "/" };


        /// <summary>
        /// The width that is used to print a help text before a line break is inserted.
        /// </summary>
        public int HelpFullWidth { get; set; }

        /// <summary>
        /// First column where help text starts.
        /// </summary>
        public int HelpAlignColumn { get; set; }

        /// <summary>
        ///     Display a help text for all commands.
        /// </summary>
        /// <remarks>
        ///     This method is called when you application is called without any parameter
        ///     to display the help text for all commands. You may provide your own method
        ///     to override the default implementation;
        /// </remarks>
        public DisplayAllCommandsDescription DisplayAllCommandsDescription { get; set; }



        private void displayAllCommandsDescription(List<CommandDescriptor> commandDescriptors)
        {
            string insert = new(' ', HelpAlignColumn);

            Console.WriteLine($"{commandDescriptors.Count} Commands available:");
            Console.WriteLine($"=======================================");
            string v = "Verb".PadRight(HelpAlignColumn);
            Console.WriteLine($"{v}Description");
            Console.WriteLine($"---------------------------------------");
            foreach (CommandDescriptor commandDescriptor in commandDescriptors)
            {
                var wrapped = Helper.Wrap(commandDescriptor.Description, HelpFullWidth);
                string verbs = commandDescriptor.Verb.Replace('.', ' ');
                Console.WriteLine($"{verbs.PadRight(HelpAlignColumn)}{wrapped.AllLines[0]}");
                for (int lineNo = 1; lineNo < wrapped.AllLines.Length; lineNo++)
                {
                    Console.WriteLine(insert + wrapped.AllLines[lineNo]);
                }
                Console.WriteLine("");
            }
        }



        /// <summary>
        ///     Display a formatted help text for a single Command.
        /// </summary>
        /// <remarks>
        ///     This method is called when a VERB was specified and help for this particular verb should be printed.<br />
        ///     The <see cref="CommandDescriptor">Command's descriptor</see> is passed to the method (the Command is specified by
        ///     the Verb).
        /// </remarks>
        public DisplayCommandHelp DisplayCommandHelp { get; set; }



        private void displayCommandHelp(CommandDescriptor commandDescriptor)
        {
            string insert = new(' ', HelpAlignColumn);

            Console.WriteLine();

            var wrappedDesc = Helper.Wrap(commandDescriptor.Description, HelpFullWidth - HelpAlignColumn);
            string verbs = commandDescriptor.Verb.Replace('.', ' ');
            Console.WriteLine($"{verbs.PadRight(HelpAlignColumn)}{wrappedDesc.AllLines[0]}");
            for (int lineNo = 1; lineNo < wrappedDesc.AllLines.Length; lineNo++) Console.WriteLine(insert + wrappedDesc.AllLines[lineNo]);

            Console.WriteLine($"{new string('-', HelpFullWidth)}");
            var command = commandDescriptor.CreateCommandInstance();
            foreach (OptionDescriptorAttribute oda in command.OptionDescriptors)
            {
                string tags = oda.Tags != null ? $"Tags={string.Join(",", oda.Tags)} " : string.Empty;
                string required = oda.Required ? "required" : "optional" + " ";
                string split = !string.IsNullOrWhiteSpace(oda.AllowMultipleSplit) ? $" Split='{oda.AllowMultipleSplit}'" : string.Empty;
                string allowMultiple = !string.IsNullOrWhiteSpace(oda.AllowMultiple) ? $"AllowMultiple={oda.AllowMultiple != null}{split}" : string.Empty;
                Console.WriteLine($"/{oda.OptionName.PadRight(HelpAlignColumn - 1)}{tags}{required}{allowMultiple}");
                var wrapped = Helper.Wrap(oda.HelpText, HelpFullWidth - HelpAlignColumn);
                foreach (string line in wrapped.AllLines) Console.WriteLine(insert + line);
                if (oda.Default != null) Console.WriteLine($"{insert}DEFAULT: '{oda.Default}'");
                Console.WriteLine();
            }
        }
    }
}