using System.Collections.Generic;
using UnityEngine;

namespace UtilsSubmodule.ObjectPool
{
    [System.Serializable]
    public struct ObjectPool<T> where T : MonoBehaviour, IPoolObject
    {
        [field: SerializeField, ReadOnly] public List<T> Pool { get; private set; }
        [SerializeField] private Transform _parent;
        [SerializeField] private T _prefab;
        private int _currentIndex;

        public void Init()
        {
            Pool.Clear();
            Pool.AddRange(_parent.GetComponentsInChildren<T>());
            foreach (var poolObject in Pool)
            {
                poolObject.SwitchActive(false);
            }
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            var counter = Pool.Count;
            while (counter > 0)
            {
                _currentIndex++;
                if (_currentIndex >= Pool.Count) _currentIndex = 0;
                // if (Pool[_currentIndex] == null)
                if (!Pool[_currentIndex])
                {
                    Pool.RemoveAt(_currentIndex);
                    _currentIndex--;
#if UNITY_EDITOR
                    Debug.Log("Destroyed object in pool");
#endif
                    continue;
                }

                if (!Pool[_currentIndex].IsActive())
                {
                    var poolObject = Pool[_currentIndex];

                    var transform = poolObject.transform;
                    transform.position = position;
                    transform.rotation = rotation;
                    poolObject.SwitchActive(true);
                    return poolObject;
                }

                counter--;
            }

            var newObject = Object.Instantiate(_prefab, position, rotation, _parent);
            Pool.Add(newObject);

#if UNITY_EDITOR
            Debug.Log($"New pool object was created. {Pool.Count} objects in Pool");
#endif
            return newObject;
        }

        public T Get() => Get(Vector3.zero, Quaternion.identity);
        public T Get(Vector3 position) => Get(position, Quaternion.identity);
        public T Get(Quaternion rotation) => Get(Vector3.zero, rotation);
    }
}