using System;
using UnityEngine;

namespace SubLib
{
    [RequireComponent(typeof(Collider))]
    public class CollisionEvent : MonoBehaviour
    {
        public event Action<Collision> OnCollisionEnterEvent;
        public event Action<Collision> OnCollisionStayEvent;
        public event Action<Collision> OnCollisionExitEvent;


        private void OnCollisionEnter(Collision other)
        {
            OnCollisionEnterEvent?.Invoke(other);
        }

        private void OnCollisionStay(Collision other)
        {
            OnCollisionStayEvent?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            OnCollisionExitEvent?.Invoke(other);
        }

    }
}
