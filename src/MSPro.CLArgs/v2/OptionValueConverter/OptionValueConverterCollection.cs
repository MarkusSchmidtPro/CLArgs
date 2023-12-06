using System;
using System.Collections;
using System.Collections.Generic;

namespace MSPro.CLArgs
{
    public class OptionValueConverterCollection : IOptionValueConverterCollection
    {
        private readonly Dictionary<Type, IArgumentConverter> _list = new();
        public IEnumerator<KeyValuePair<Type, IArgumentConverter>> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();

        public void Add(KeyValuePair<Type, IArgumentConverter> item)
        {
            _list.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(KeyValuePair<Type, IArgumentConverter> item) => _list.ContainsKey(item.Key);

        public void CopyTo(KeyValuePair<Type, IArgumentConverter>[] array, int arrayIndex)
            =>throw new NotSupportedException();


        public bool Remove(KeyValuePair<Type, IArgumentConverter> item) => _list.Remove(item.Key);

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public bool ContainsKey(Type key) => _list.ContainsKey(key);

        public void Add(Type key, IArgumentConverter value)
        {
            _list.Add(key, value);
        }

        public bool Remove(Type key) => _list.Remove(key);

        public bool TryGetValue(Type key, out IArgumentConverter value) => _list.TryGetValue(key, out value);

        public IArgumentConverter this[Type key]
        {
            get => _list[key];
            set => _list[key] = value;
        }

        public ICollection<Type> Keys => _list.Keys;

        public ICollection<IArgumentConverter> Values => _list.Values;
    }
}