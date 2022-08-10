using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MSPro.CLArgs;

public class ArgumentCollection : IArgumentCollection
{
    public IEnumerable<string> Verbs =>
        _list
            .Where(arg => arg.Type == CommandlineArgumentType.Verb)
            .Select(arg => arg.Key);

    public IEnumerable<string> Targets =>
        _list
            .Where(arg => arg.Type == CommandlineArgumentType.Target)
            .Select(arg => arg.Value);

    public IEnumerable<Option> Options =>
        _list
            .Where(arg => arg.Type == CommandlineArgumentType.Option)
            .Select(arg => new Option(arg.Key, arg.Value));

    public string VerbPath => Verbs.Any() ? string.Join(".", Verbs) : null;

    private readonly List<CommandlineArgument> _list = new();

    public IEnumerator<CommandlineArgument> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();

    public void Add(CommandlineArgument item)
    {
        _list.Add(item);
    }

    public void Clear()
    {
        _list.Clear();
    }

    public bool Contains(CommandlineArgument item) => _list.Contains(item);

    public void CopyTo(CommandlineArgument[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    public bool Remove(CommandlineArgument item) => _list.Remove(item);

    public int Count => _list.Count;

    public bool IsReadOnly => false;

    public int IndexOf(CommandlineArgument item) => _list.IndexOf(item);

    public void Insert(int index, CommandlineArgument item)
    {
        _list.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _list.RemoveAt(index);
    }

    public CommandlineArgument this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }
}