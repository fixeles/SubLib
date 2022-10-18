using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ParticlesPool
{
    [SerializeField] private List<ParticleSystem> _pool;
    private int _currentIndex;

    public void PlayAt(Vector3 position)
    {
        _pool[_currentIndex].transform.position = position;
        Play();
    }

    public void Play()
    {
        _pool[_currentIndex].Play();
        _currentIndex++;
        if (_currentIndex >= _pool.Count) _currentIndex = 0;
    }
}
