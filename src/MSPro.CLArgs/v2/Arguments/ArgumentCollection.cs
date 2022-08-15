using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs;


/// <summary>
/// Represents all known arguments of type Verb, Option or target.
/// </summary>
public class ArgumentCollection : IArgumentCollection
{
    private readonly List<Argument> _list = new ();


    public IEnumerable<string> Verbs =>
        _list
           .Where(arg => arg.Type == ArgumentType.Verb)
           .Select(arg => arg.Key);

    public IEnumerable<string> Targets =>
        _list
           .Where(arg => arg.Type == ArgumentType.Target)
           .Select(arg => arg.Key);

    public IEnumerable<KeyValuePair<string, string>> Options =>
        _list
           .Where(arg => arg.Type == ArgumentType.Option)
           .Select(arg => new KeyValuePair<string, string>(arg.Key, arg.Value));

    public string VerbPath => this.Verbs.Any() ? string.Join(".", this.Verbs) : null;

    public IEnumerator<Argument> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();

    public void Add([NotNull] Argument item) => _list.Add(item);

    public void Clear() => _list.Clear();

    public bool Contains([NotNull]Argument item) => _list.Contains(item);

    public bool Remove([NotNull] Argument item) => _list.Remove(item);

    public int Count => _list.Count;

    public bool IsReadOnly => false;


    public void CopyTo(Argument[] array, int arrayIndex) => throw new NotImplementedException();
    public int IndexOf(Argument item) => _list.IndexOf(item);
    public void Insert(int index, Argument item) => _list.Insert(index, item);
    public void RemoveAt(int index) => _list.RemoveAt(index);

    public Argument this[int index]
    {
        get => _list[index];
        set => _list[index]=value;
    }
}