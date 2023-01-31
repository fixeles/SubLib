using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace SubLib
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] private List<TKey> _serializedKeys = new();
        [SerializeField, HideInInspector] private List<TValue> _serializedValues = new ();
        [SerializeField, HideInInspector] private int _serializedCount;

        public void OnBeforeSerialize()
        {
            _serializedKeys.Clear();
            _serializedValues.Clear();

            _serializedValues.Capacity = Count;
            _serializedKeys.Capacity = Count;

            foreach (var item in this)
            {
                _serializedKeys.Add(item.Key);
                _serializedValues.Add(item.Value);
            }

            _serializedCount = Count;
        }

        public void OnAfterDeserialize()
        {
            Clear();

            if (_serializedCount != _serializedKeys.Count)
            {
                throw new SerializationException(string.Format("{0} failed to serialize.", typeof(TKey).Name));
            }

            if (_serializedCount != _serializedValues.Count)
            {
                throw new SerializationException(string.Format("{0} failed to serialize.", typeof(TValue).Name));
            }

            for (var i = 0; i < _serializedCount; ++i)
            {
                Add(_serializedKeys[i], _serializedValues[i]);
            }

            Debug.Log(_serializedCount);
            _serializedKeys.Clear();
            _serializedValues.Clear();
        }
    }
}