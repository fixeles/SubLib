using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableHashSet<T> : ISerializationCallbackReceiver, ISet<T>, IReadOnlyCollection<T>
{
    [SerializeField] private List<T> values = new();
    private HashSet<T> _hashSet = new();

    #region Constructors

    // empty constructor required for Unity serialization
    public SerializableHashSet()
    {
    }

    public SerializableHashSet(IEnumerable<T> collection)
    {
        _hashSet = new HashSet<T>(collection);
    }

    #endregion Constructors


    #region Interface forwarding to the _hashset

    public bool Remove(T item) => _hashSet.Remove(item);
    public int Count => _hashSet.Count;
    public bool IsReadOnly => false;
    bool ISet<T>.Add(T item) => _hashSet.Add(item);
    bool ICollection<T>.Remove(T item) => _hashSet.Remove(item);
    public void ExceptWith(IEnumerable<T> other) => _hashSet.ExceptWith(other);
    public void IntersectWith(IEnumerable<T> other) => _hashSet.IntersectWith(other);
    public bool IsProperSubsetOf(IEnumerable<T> other) => _hashSet.IsProperSubsetOf(other);
    public bool IsProperSupersetOf(IEnumerable<T> other) => _hashSet.IsProperSupersetOf(other);
    public bool IsSubsetOf(IEnumerable<T> other) => _hashSet.IsSubsetOf(other);
    public bool IsSupersetOf(IEnumerable<T> other) => _hashSet.IsSupersetOf(other);
    public bool Overlaps(IEnumerable<T> other) => _hashSet.Overlaps(other);
    public bool SetEquals(IEnumerable<T> other) => _hashSet.SetEquals(other);
    public void SymmetricExceptWith(IEnumerable<T> other) => _hashSet.SymmetricExceptWith(other);
    public void UnionWith(IEnumerable<T> other) => _hashSet.UnionWith(other);
    public void Clear() => _hashSet.Clear();
    public bool Contains(T item) => _hashSet.Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => _hashSet.CopyTo(array, arrayIndex);
    void ICollection<T>.Add(T item) => _hashSet.Add(item);
    public IEnumerator<T> GetEnumerator() => _hashSet.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion Interface forwarding to the _hashset


    #region ISerializationCallbackReceiver implemenation

    public void OnBeforeSerialize()
    {
        var cur = new HashSet<T>(values);

        foreach (var val in this)
        {
            if (!cur.Contains(val))
            {
                values.Add(val);
            }
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();

        foreach (var val in values)
        {
            _hashSet.Add(val);
        }
    }

    #endregion ISerializationCallbackReceiver implemenation
}