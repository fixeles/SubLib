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
                
                _lastRotation = transform.rotation;
                _lastPosition = transform.position;
            }
        }

        public void Reset()
        {
            _target = null;
            _force = 0;
        }

        private void FixedUpdate()
        {
            if (Target == null || _force == 0)
            {
                this.enabled = false;
                return;
            }

            transform.position = Vector3.Lerp(_lastPosition, _target.position, Time.fixedDeltaTime * _force);
            transform.rotation = Quaternion.Lerp(_lastRotation, _target.rotation, Time.fixedDeltaTime * _force * 2);
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