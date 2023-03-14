using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SubLib
{
    public class Queue<T> where T : class
    {
        public event Action OnQueueChangeEvent;
        private List<T> _items = new();

        public T[] Items => _items.ToArray();

        public int Count => _items.Count;

        public void Sort(IComparer<T> comparer)
        {
            var sortedList = _items.OrderBy(x => x, comparer).ToList();
            if (sortedList == _items || sortedList.Count == 0) return;

            _items = sortedList;
            OnQueueChangeEvent?.Invoke();
        }

        public void Enqueue(T item)
        {
            _items.Add(item);
            OnQueueChangeEvent?.Invoke();
        }

        public T Dequeue()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            T item = _items[0];
            _items.RemoveAt(0);
            OnQueueChangeEvent?.Invoke();
            return item;
        }

        public T Peek()
        {
            if (_items.Count == 0)
            {
                Debug.Log("Queue is empty");
                return null;
            }

            return _items[0];
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}