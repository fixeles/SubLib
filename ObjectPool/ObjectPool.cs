using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UtilsSubmodule.ObjectPool
{
    [System.Serializable]
    public class ObjectPool<T> where T : Component
    {
        [SerializeField, ReadOnly] private SerializableHashSet<T> _pool;

        [SerializeField] private Transform _parent;
        [SerializeField] private T _prefab;

        public T Prefab => _prefab;

#if UNITY_EDITOR
        [Button]
        public void Fill(int size = 1)
        {
            size = Mathf.Clamp(size, 0, size);
            for (int i = 0; i < size; i++)
            {
                PrefabUtility.InstantiatePrefab(_prefab, _parent);
            }

            _pool.Clear();
            _pool.UnionWith(_parent.GetComponentsInChildren<T>(true));
            foreach (var pooledObject in _pool)
            {
                pooledObject.gameObject.SetActive(false);
            }
        }
#endif

        public T Get(in Vector3 position, in Quaternion rotation)
        {
            var pooledObject = _pool.FirstOrDefault(item => !item.gameObject.activeSelf);
            if (!pooledObject)
            {
                pooledObject = CreateNew(position, rotation);
                _pool.Add(pooledObject);
                return pooledObject;
            }

            var transform = pooledObject.transform;
            transform.position = position;
            transform.rotation = rotation;
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        private T CreateNew(in Vector3 position, in Quaternion rotation)
        {
            var newObject = Object.Instantiate(_prefab, position, rotation, _parent);
            _pool.Add(newObject);

#if UNITY_EDITOR
            Debug.Log($"New pool object was created. {_pool.Count} objects in Pool");
#endif
            return newObject;
        }

        public T Get() => Get(Vector3.zero, Quaternion.identity);
        public T Get(in Vector3 position) => Get(position, Quaternion.identity);
        public T Get(in Quaternion rotation) => Get(Vector3.zero, rotation);
    }
}