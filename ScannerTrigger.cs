using System;
using UnityEngine;

namespace UtilsSubmodule
{
    [RequireComponent(typeof(SphereCollider))]
    public class ScannerTrigger : MonoBehaviour, IAutoInit
    {
        public event Action<Collider> OnTriggerEnterEvent;
        [SerializeField, ReadOnly] private SphereCollider _selfCollider;

        [SerializeField, Min(1)] private float _scanSpeed;
        public float MaxRadius { get; private set; }

        public void Reset()
        {
            _selfCollider.radius = 0;
        }

        public void AutoInit()
        {
            _selfCollider = GetComponent<SphereCollider>();
            _selfCollider.isTrigger = true;
        }

        private void Awake()
        {
            MaxRadius = _selfCollider.radius;
            _selfCollider.radius = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }

        private void Update()
        {
            _selfCollider.radius += Time.deltaTime * _scanSpeed;
            if (_selfCollider.radius > MaxRadius) Reset();
        }
    }
}