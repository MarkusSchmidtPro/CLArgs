using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     All arguments as they were provided in the command-line.
    /// </summary>
    /// <remarks>
    ///     An <c>Argument</c> is either a <see cref="Verbs" />
    ///     or an <see cref="Option" />.
    /// </remarks>
    [PublicAPI]
    public class CommandLineArguments
    {
        public string[] Args { get; }
        private readonly StringComparison _comparer;



        internal CommandLineArguments(string[] args, bool ignoreCase = false)
        {
            this.Args = args;
            this.CommandLine = string.Join(" ", args);

            _comparer = ignoreCase
                ? StringComparison.InvariantCultureIgnoreCase
                : StringComparison.InvariantCulture;

            this.Verbs = new List<string>();
            this.Targets = new List<string>();
            this.Options = new List<Option>();
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
        public string VerbPath => this.Verbs.Count == 0 ? null : string.Join(".", this.Verbs);

        /// <summary>
        ///     A key-value list of all options provided in the command-line.
        /// </summary>
        /// <remarks>
        ///     All option values are <c>strings</c> in the first instance.
        ///     Conversion may happen later.
        /// </remarks>
        public List<Option> Options { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionTag"></param>
        /// <returns></returns>
        public bool OptionTagProvided(string optionTag) => this.Options.Any(o => o.Key.Equals(optionTag, _comparer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verb"></param>
        public void AddVerb(string verb) => this.Verbs.Add(verb);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public void AddTarget(string target) => this.Targets.Add(target);

        /// <inheritdoc cref="AddOption(MSPro.CLArgs.Option)" />
        public void AddOption(Option option) => this.Options.Add(option);
    }
}