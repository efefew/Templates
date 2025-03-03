using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class KeyValuePair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField] private List<KeyValuePair<TKey, TValue>> entries = new List<KeyValuePair<TKey, TValue>>();
    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public void Add(TKey key, TValue value) => dictionary.Add(key, value);
    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
    public TValue this[TKey key] => dictionary[key];

    public void OnBeforeSerialize()
    {
        entries.Clear();
        foreach (System.Collections.Generic.KeyValuePair<TKey, TValue> pair in dictionary)
        {
            entries.Add(new KeyValuePair<TKey, TValue> { Key = pair.Key, Value = pair.Value });
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary.Clear();
        foreach (KeyValuePair<TKey, TValue> entry in entries)
        {
            if (dictionary.ContainsKey(entry.Key))
            {
                entry.Key = default;
                if (dictionary.ContainsKey(default))
                {
                    continue;
                }
            }

            dictionary.Add(entry.Key, entry.Value);
        }
    }
}