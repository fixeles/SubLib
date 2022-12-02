using UnityEngine;

namespace UtilsSubmodule
{
    public class Scanner<T> where T : MonoBehaviour
    {
        public event System.Action OnTargetFound;
        public T CurrentTarget;
        private readonly ScannerTrigger _trigger;
        private readonly string[] _tags;

        public float Radius => _trigger.MaxRadius;

        public Scanner(ScannerTrigger trigger, params string[] tags)
        {
            _tags = tags;
            _trigger = trigger;
            Run();
        }

        public void Run()
        {
            _trigger.enabled = true;
            _trigger.OnTriggerEnterEvent += TriggerCheck;
        }

        public void Stop()
        {
            _trigger.Reset();
            _trigger.enabled = false;
            _trigger.OnTriggerEnterEvent -= TriggerCheck;
        }

        private void TriggerCheck(Collider collider)
        {
            if (_tags.Length > 0 && !TagCheck(collider)) return;

            var newTarget = collider.GetComponent<T>();
            if (newTarget == null || !newTarget.enabled) return;

            _trigger.Reset();
            OnTargetFound?.Invoke();
            if (newTarget == CurrentTarget) return;

            CurrentTarget = newTarget;
        }

        private bool TagCheck(in Collider collider)
        {
            for (int i = 0; i < _tags.Length; i++)
            {
                if (collider.CompareTag(_tags[i])) return true;
            }

            return false;
        }
    }
}