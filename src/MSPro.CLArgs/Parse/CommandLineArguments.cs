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
        private readonly StringComparison _stringComparison;


        internal CommandLineArguments( StringComparison stringComparison)
        {
            _stringComparison = stringComparison;
            this.Verbs        = new List<string>();
            this.Targets      = new List<string>();
            this.Options      = new List<Option>();
        }

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
        public bool OptionTagProvided(string optionTag) => this.Options.Any(o => o.Key.Equals(optionTag, _stringComparison));


        /// <summary>
        /// The user requested help in the command-line.
        /// </summary>
        public bool HelpRequested => OptionTagProvided("help") || OptionTagProvided("?");
        
        
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