using System.Collections.Generic;

namespace MSPro.CLArgs;

public interface ICommandlineArgumentCollection : IList<CommandlineArgument>
{
    public IEnumerable<string> Verbs { get; }
    public IEnumerable<string> Targets{ get; }
    public IEnumerable<Option> Options { get; }
}