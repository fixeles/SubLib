using System;
using System.Collections.Generic;

namespace SubLib
{
    public class Queue<T>
    {
        public event Action OnQueueChangeEvent;
        public event Action OnDequeueEvent;
        private readonly int _maxCapacity;
        private readonly List<T> _items;

        public T[] Items => _items.ToArray();
        public bool HasSpace => _items.Count < _maxCapacity;

        public Queue(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
            _items = new List<T>(_maxCapacity);
        }


        public bool Enqueue(T item)
        {
            if (!HasSpace) return false;
            _items.Add(item);
            OnQueueChangeEvent?.Invoke();
            return true;
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
            OnDequeueEvent?.Invoke();
            return item;
        }

        public T Peek()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            return _items[0];
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}