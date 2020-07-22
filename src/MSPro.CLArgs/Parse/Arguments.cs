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
        private readonly HashSet<string> _verbs;



        public Arguments(string commandLine, bool caseSensitive = true)
        {
            this.CommandLine = commandLine;

            IEqualityComparer<string> c = caseSensitive
                ? StringComparer.InvariantCulture
                : StringComparer.InvariantCultureIgnoreCase;
            _verbs   = new HashSet<string>(c);
            _options = new Dictionary<string, Option>(c);
        }



        /// <summary>
        ///     The full command-line as it was parsed.
        /// </summary>
        public string CommandLine { get; }


        /// <summary>
        ///     The list of verbs in the sequence order
        ///     as they were provided in the command-line.
        /// </summary>
        public HashSet<string> Verbs => _verbs;


        /// <summary>
        ///     All Verbs as a '.' concatenated list - namespaced verbs.
        /// </summary>
        public string VerbPath => string.Join(".", this.Verbs);

        /// <summary>
        ///     A key-value list of all options provided in the command-line.
        /// </summary>
        /// <remarks>
        ///     All option values are <c>strings</c> int he first instance.
        ///     Conversion may happen later.
        /// </remarks>
        public IEnumerable<Option> Options => _options.Values;

        public bool OptionTagProvided(string optionTag) => _options.ContainsKey(optionTag);

        public void AddVerb(string verb) => _verbs.Add(verb);

        public void AddOption(Option option) => _options[option.Key] = option;


        /// <summary>
        ///     Manually add or update an option.
        /// </summary>
        /// <remarks>Options are unique by their <see cref="Option.Key" /></remarks>
        public void AddOption(string tag, string value) => AddOption(new Option(tag, value));
    }
}