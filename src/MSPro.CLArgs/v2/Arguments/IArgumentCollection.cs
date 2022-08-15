using System.Collections.Generic;
using JetBrains.Annotations;



namespace MSPro.CLArgs;

public interface IArgumentCollection : IList<Argument>
{
    /// <summary>
    /// All verbs path: all verbs joined by '.'.
    /// </summary>
    [CanBeNull] public string VerbPath { get; }
    public IEnumerable<string> Verbs { get; }
    public IEnumerable<string> Targets{ get; }
    public IEnumerable<KeyValuePair<string,string>> Options { get; }
}