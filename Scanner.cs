using UnityEngine;

namespace UtilsSubmodule
{
    public class Scanner<T> where T : MonoBehaviour
    {
        public event System.Action OnTargetFound;
        private readonly ScannerTrigger _trigger;
        private readonly string _tag;
        public T CurrentTarget { get; private set; }

        public Scanner(ScannerTrigger trigger, string tag = "")
        {
            _tag = tag;
            _trigger = trigger;
            CurrentTarget = null;
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
            if (_tag.Length > 0 && !collider.CompareTag(_tag)) return;

            var newTarget = collider.GetComponent<T>();
            if (newTarget == null) return;

            _trigger.Reset();
            OnTargetFound?.Invoke();
            if (newTarget == CurrentTarget) return;

            CurrentTarget = newTarget;
        }
    }
}