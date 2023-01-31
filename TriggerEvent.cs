using System;
using UnityEngine;

namespace SubLib
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEvent : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEnterEvent;
        public event Action<Collider> OnTriggerStayEvent;
        public event Action<Collider> OnTriggerExitEvent;


        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvent?.Invoke(other);
        }
    }
}