using System;
using System.Collections.Generic;
using System.Linq;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     All arguments as they were provided in the command-line.
    /// </summary>
    /// <remarks>
    ///     An <c>Argument</c> is either a <see cref="Verbs" />
    ///     or an <see cref="Option" />.
    /// </remarks>
    public class CommandLineArguments
    {
        public string[] Args { get; }
        private readonly StringComparison _stringComparison;


        internal CommandLineArguments(string[] args, StringComparison stringComparison)
        {
            Args         = args;
            CommandLine  = string.Join(" ", args);
            _stringComparison = stringComparison;
            Verbs        = [];
            Targets      = [];
            Options      = [];
        }



        /// <summary>
        ///     The full command-line as it was parsed.
        /// </summary>
        public string CommandLine { get; }


        /// <summary>
        ///     The list of verbs in the sequence order
        ///     as they were provided in the command-line.
        /// </summary>
        public List<string> Verbs { get; }

        /// <summary>
        ///     The list of Targets in the sequence order
        ///     as they were provided in the command-line.
        /// </summary>
        public List<string> Targets { get; }


        /// <summary>
        ///     All Verbs as a '.' concatenated list - namespaced verbs.
        /// </summary>
        /// <returns>
        ///     All <see cref="Verbs" /> concatenated by '.', for example, 'HelloWorld.Germany'.<br />
        ///     <c>null</c> in case, no verb was provided in the command-line.
        /// </returns>
        public string VerbPath => Verbs.Count == 0 ? null : string.Join(".", Verbs);

        /// <summary>
        ///     A key-value list of all options provided in the command-line.
        /// </summary>
        /// <remarks>
        ///     All option values are <c>strings</c> in the first instance.
        ///     Conversion may happen later.
        /// </remarks>
        public List<Option> Options { get; }

        /// <summary>
        /// Check if an option tag was provided in the command-line.
        /// </summary>
        /// <remarks>The function respects the <seealso cref="Settings.IgnoreCase"/>.</remarks>
        public bool OptionTagProvided(string optionTag) => Options.Any(o => o.Key.Equals(optionTag, _stringComparison));


        /// <summary>
        /// The user requested help in the command-line.
        /// </summary>
        public bool HelpRequested => OptionTagProvided("help") || OptionTagProvided("?");
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="verb"></param>
        public void AddVerb(string verb) => Verbs.Add(verb);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public void AddTarget(string target) => Targets.Add(target);

        /// <inheritdoc cref="AddOption(MSPro.CLArgs.Option)" />
        public void AddOption(Option option) => Options.Add(option);
    }
}