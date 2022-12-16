using System.Collections.Generic;
using UnityEngine;

namespace UtilsSubmodule
{
    [System.Serializable]
    public class SerializableHashSet<T> : ISerializationCallbackReceiver
    {
        private HashSet<T> _hashSet = new ();

        [SerializeField] private List<T> _serializableItems;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
           _serializableItems = new List<T>(_hashSet);
            _hashSet = null;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            foreach (T item in _serializableItems)
            {
                _ = _hashSet.Add(item);
            }

            _serializableItems = null;
        }

        public bool Add(T item) => _hashSet.Add(item);

        public bool Contains(T item) => _hashSet.Contains(item);
    }
}