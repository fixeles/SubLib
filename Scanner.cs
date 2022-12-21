using System;
using System.Collections.Generic;
using System.Linq;
using ExtensionsMbehs;
using Game.Scripts.MonoBehaviours;
using UnityEngine;
using UtilsSubmodule.Async;

namespace UtilsSubmodule
{
    public class Scanner : MonoBehaviour
    {
        public event System.Action OnTargetFound;
        [@ReadOnly] public Hitable CurrentTarget;
        [SerializeField, Min(0)] private float _radius;

        [SerializeField, TagSelector] private string[] _tags;
        [SerializeField] private LayerMask _scanLayer;

        private Timer _timer;


        public float Radius => _radius;
        public void Reset() => CurrentTarget = null;

        public void Run()
        {
            _timer = new Timer(500, Scan);
        }

        public void Stop()
        {
            _timer?.Destroy();
        }

        public void Scan()
        {
            var colliders = Physics.OverlapSphere(transform.position, _radius, _scanLayer).ToList();
            List<Hitable> scanList = new();
            foreach (var overlapped in colliders)
            {
                if (_tags.Length > 0 && !TagCheck(overlapped)) continue;
                var hitable = overlapped.GetComponent<Hitable>();
                if (hitable) scanList.Add(hitable);
            }

            if (scanList.Count == 0) return;

            CurrentTarget = scanList.GetNearestObject(transform.position);
            OnTargetFound?.Invoke();
        }
        
        private void OnDisable()
        {
            Stop();
        }

        private bool TagCheck(in Collider overlappedCollider)
        {
            for (int i = 0; i < _tags.Length; i++)
            {
                if (overlappedCollider.CompareTag(_tags[i])) return true;
            }

            return false;
        }

#if UNITY_EDITOR
        [SerializeField] private Color _gizmoColor = Color.black;
        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
#endif
    }
}