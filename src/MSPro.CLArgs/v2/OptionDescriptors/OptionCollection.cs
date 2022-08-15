using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;



namespace MSPro.CLArgs;

/// <summary>
///     Represents all known arguments of type Verb, Option or target.
/// </summary>
public class OptionCollection : IOptionCollection
{
    private readonly List<Option2> _list = new();

    public IEnumerator<Option2> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();

    public void Add([NotNull] Option2 item) => _list.Add(item);

    public void Clear() => _list.Clear();

    public bool Contains([NotNull] Option2 item) => _list.Contains(item);

    public bool Remove([NotNull] Option2 item) => _list.Remove(item);

    public int Count => _list.Count;

    public bool IsReadOnly => false;


    public void CopyTo(Option2[] array, int arrayIndex) => throw new NotImplementedException();
    public int IndexOf(Option2 item) => _list.IndexOf(item);
    public void Insert(int index, Option2 item) => _list.Insert(index, item);
    public void RemoveAt(int index) => _list.RemoveAt(index);



    public Option2 this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }
}