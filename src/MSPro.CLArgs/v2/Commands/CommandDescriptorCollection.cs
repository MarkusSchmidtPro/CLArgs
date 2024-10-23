using System;
using System.Collections;
using System.Collections.Generic;

namespace MSPro.CLArgs
{
    /// <summary>
    /// Concrete implementation of a dictionary.
    /// </summary>
    public class CommandDescriptorCollection(Settings2 settings) : ICommandDescriptorCollection
    {
        private readonly Dictionary<string, CommandDescriptor2> _data = new(settings.GetStringComparer());


        public IEnumerator<KeyValuePair<string, CommandDescriptor2>> GetEnumerator() =>
            _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable)_data).GetEnumerator();

        public void Add(KeyValuePair<string, CommandDescriptor2> kv)
        {
            _data.Add(kv.Key, kv.Value);
        }

        public void Clear() => _data.Clear();

        public bool Contains(KeyValuePair<string, CommandDescriptor2> kv) =>
            _data.ContainsKey(kv.Key);

        public void CopyTo(KeyValuePair<string, CommandDescriptor2>[] array, int arrayIndex) =>
            throw new NotSupportedException();

        public bool Remove(KeyValuePair<string, CommandDescriptor2> kv) =>
            _data.Remove(kv.Key);

        public int Count => _data.Count;

        public bool IsReadOnly => throw new NotSupportedException();

        public void Add(string key, CommandDescriptor2 value) => _data.Add(key, value);

        public bool ContainsKey(string key) => _data.ContainsKey(key);

        public bool Remove(string key) => _data.Remove(key);

        public bool TryGetValue(string key, out CommandDescriptor2 value) =>
            _data.TryGetValue(key, out value);

        public CommandDescriptor2 this[string key]
        {
            get => _data[key];
            set => _data[key] = value;
        }

        public ICollection<string> Keys => _data.Keys;

        public ICollection<CommandDescriptor2> Values => _data.Values;
    }
}