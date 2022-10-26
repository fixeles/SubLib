using UnityEngine;
using static Utils.EditorUtils;

namespace Game.Scripts.UtilsSubmodule.Inventory
{
    public class Magnet : MonoBehaviour, IAutoInit
    {
        [SerializeField] private MagnetSettings Settings;
        [SerializeField, ReadOnly] private Transform _target;
        private float _force;
        private Vector3 _lastPosition;
        private Quaternion _lastRotation;


        public Transform Target
        {
            get => _target;
            set
            {
                _target = value;
                this.enabled = value != null;
                if (_target == null) return;

                var lerpValue = _target.transform.localPosition.y / Settings.Height;
                _force = Settings.ForceCurve.Evaluate(lerpValue) * Settings.ForceMultiplier;
            }
        }

        public void Reset()
        {
            _target = null;
            _force = 0;
        }

        private void Update()
        {
            if (Target == null || _force == 0)
            {
                this.enabled = false;
                return;
            }

            transform.rotation = _lastRotation;
            transform.position = _lastPosition;
            transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _force);
            transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, Time.deltaTime * _force * 2);
        }

        private void LateUpdate()
        {
            _lastRotation = transform.rotation;
            _lastPosition = transform.position;
        }

        private void OnDisable()
        {
            Reset();
        }

        public void AutoInit()
        {
            Settings = GetAllInstances<MagnetSettings>()[0];
        }
    }
}