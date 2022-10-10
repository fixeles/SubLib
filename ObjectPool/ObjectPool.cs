using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : MonoBehaviour, IPoolObject
{
    protected List<T> Pool;
    private Transform _parent;
    private int _currentIndex;
    private T _prefab;

    public ObjectPool(Transform parent, T prefab)
    {
        _parent = parent;
        _prefab = prefab;
        Pool = new List<T>();

        Pool.AddRange(parent.GetComponentsInChildren<T>());
        foreach (var poolObject in Pool)
        {
            poolObject.SwitchActive(false);
        }
    }

    public T Get(Vector3 position, Quaternion rotation)
    {
        int counter = Pool.Count;
        while (counter > 0)
        {
            _currentIndex++;
            if (_currentIndex >= Pool.Count) _currentIndex = 0;
            if (Pool[_currentIndex] == null)
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
                T poolObject = Pool[_currentIndex];

                poolObject.transform.position = position;
                poolObject.transform.rotation = rotation;
                poolObject.SwitchActive(true);
                return poolObject;
            }
            counter--;
        }

        var newObject = GameObject.Instantiate(_prefab, position, rotation, _parent);
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
