using System;
using System.Collections.Generic;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     The arguments as they were provided as a command-line.
    /// </summary>
    /// <remarks>
    ///     An <c>Argument</c> can be either a <see cref="Verbs" />
    ///     or an <see cref="Option" />.
    /// </remarks>
    [PublicAPI]
    public class Arguments
    {
        private readonly Dictionary<string, Option> _options;



        internal Arguments(string commandLine, bool ignoreCase = false)
        {
            this.CommandLine = commandLine;

            IEqualityComparer<string> c = ignoreCase
                ? StringComparer.InvariantCultureIgnoreCase
                : StringComparer.InvariantCulture;
            this.Verbs = new HashSet<string>(c);
            _options   = new Dictionary<string, Option>(c);
        }



        /// <summary>
        ///     The full command-line as it was parsed.
        /// </summary>
        public string CommandLine { get; }


        /// <summary>
        ///     The list of verbs in the sequence order
        ///     as they were provided in the command-line.
        /// </summary>
        public HashSet<string> Verbs { get; }



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
        ///     All option values are <c>strings</c> int he first instance.
        ///     Conversion may happen later.
        /// </remarks>
        public IEnumerable<Option> Options => _options.Values;

        public bool OptionTagProvided(string optionTag) => _options.ContainsKey(optionTag);

        public void AddVerb(string verb) => this.Verbs.Add(verb);



        /// <inheritdoc cref="SetOption(MSPro.CLArgs.Option)" />
        public void SetOption(Option option) => _options[option.Key] = option;



        /// <summary>
        ///     Manually add or update an option.
        /// </summary>
        /// <remarks>Options are unique by their <see cref="Option.Key" /></remarks>
        public void SetOption(string tag, string value) => SetOption(new Option(tag, value));
    }
}