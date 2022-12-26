using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableHashSet<T> : ISerializationCallbackReceiver, ISet<T>, IReadOnlyCollection<T>
{
    [SerializeField] private List<T> values = new();
    protected HashSet<T> HashSet = new();

    #region Constructors

    // empty constructor required for Unity serialization
    public SerializableHashSet()
    {
    }

    public SerializableHashSet(IEnumerable<T> collection)
    {
        HashSet = new HashSet<T>(collection);
    }

    #endregion Constructors


    #region Interface forwarding to the _hashset

    public bool Add(T item) => HashSet.Add(item);
    public T FirstOrDefault(Func<T, bool> prediction) => HashSet.FirstOrDefault(prediction);
    public bool Remove(T item) => HashSet.Remove(item);
    public int Count => HashSet.Count;
    public bool IsReadOnly => false;
    bool ISet<T>.Add(T item) => HashSet.Add(item);
    bool ICollection<T>.Remove(T item) => HashSet.Remove(item);
    public void ExceptWith(IEnumerable<T> other) => HashSet.ExceptWith(other);
    public void IntersectWith(IEnumerable<T> other) => HashSet.IntersectWith(other);
    public bool IsProperSubsetOf(IEnumerable<T> other) => HashSet.IsProperSubsetOf(other);
    public bool IsProperSupersetOf(IEnumerable<T> other) => HashSet.IsProperSupersetOf(other);
    public bool IsSubsetOf(IEnumerable<T> other) => HashSet.IsSubsetOf(other);
    public bool IsSupersetOf(IEnumerable<T> other) => HashSet.IsSupersetOf(other);
    public bool Overlaps(IEnumerable<T> other) => HashSet.Overlaps(other);
    public bool SetEquals(IEnumerable<T> other) => HashSet.SetEquals(other);
    public void SymmetricExceptWith(IEnumerable<T> other) => HashSet.SymmetricExceptWith(other);
    public void UnionWith(IEnumerable<T> other) => HashSet.UnionWith(other);
    public void Clear() => HashSet.Clear();
    public bool Contains(T item) => HashSet.Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => HashSet.CopyTo(array, arrayIndex);
    void ICollection<T>.Add(T item) => HashSet.Add(item);
    public IEnumerator<T> GetEnumerator() => HashSet.GetEnumerator();
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
            HashSet.Add(val);
        }
    }

    #endregion ISerializationCallbackReceiver implemenation
}