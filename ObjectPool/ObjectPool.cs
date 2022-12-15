using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UtilsSubmodule.ObjectPool
{
    [System.Serializable]
    public class ObjectPool<T> where T : MonoBehaviour, IPooledObject
    {
        [field: SerializeField, Sirenix.OdinInspector.ReadOnly]
        public List<T> Pool { get; private set; }

        [SerializeField] private Transform _parent;
        [SerializeField] private T _prefab;

        public T Prefab => _prefab;

        private int _currentIndex;

#if UNITY_EDITOR
        [Button]
        public void Fill(int size = 1)
        {
            size = Mathf.Clamp(size, 0, size);
            for (int i = 0; i < size; i++)
            {
                PrefabUtility.InstantiatePrefab(_prefab, _parent);
            }

            Init();
        }
#endif


        private void Init()
        {
            Pool.Clear();
            Pool.AddRange(_parent.GetComponentsInChildren<T>(true));
            foreach (var pooledObject in Pool)
            {
                pooledObject.Prepare();
            }
        }

        public T Get(in Vector3 position, in Quaternion rotation)
        {
            if (Pool.Count == 0) return CreateNew(position, rotation);
            
            var counter = Pool.Count;
            while (counter > 0)
            {
                _currentIndex++;
                if (_currentIndex >= Pool.Count) _currentIndex = 0;
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
                    poolObject.GetPooled();
                    return poolObject;
                }

                counter--;
            }

            return CreateNew(position, rotation);
        }

        private T CreateNew(in Vector3 position, in Quaternion rotation)
        {
#if UNITY_EDITOR
            Debug.Log($"New pool object was created. {Pool.Count} objects in Pool");
#endif
            var newObject = Object.Instantiate(_prefab, position, rotation, _parent);
            Pool.Add(newObject);
            return newObject;
        }

        public void ReleaseAll()
        {
            for (int i = 0; i < Pool.Count; i++)
            {
                Pool[i].ReleasePooled();
            }
        }

        public T Get() => Get(Vector3.zero, Quaternion.identity);
        public T Get(in Vector3 position) => Get(position, Quaternion.identity);
        public T Get(in Quaternion rotation) => Get(Vector3.zero, rotation);
    }
}