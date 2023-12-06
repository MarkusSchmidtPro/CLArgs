using System.Collections.Generic;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    /// <summary>
    /// Represent all arguments provided in the command-line divided into their types.
    /// </summary>
    public interface IArgumentCollection : IList<Argument>
    {
        /// <summary>
        /// All verbs path: all verbs joined by '.'.
        /// </summary>
        [CanBeNull] public string VerbPath { get; }
        public IEnumerable<string> Verbs { get; }
        public IEnumerable<string> Targets{ get; }
    
        /// <summary>
        /// Get all options provided in the command-line as a KeyValue pair. 
        /// </summary>
        /// <remarks>
        /// The option tag as it was specified in the command-line becomes the <c>Key</c>.
        /// If an option with the same tag was specified more than once there is an entry for each option.
        /// </remarks>
        public IEnumerable<KeyValuePair<string,string>> Options { get; }
    }
}