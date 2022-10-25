using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    public event Action<Collider> OnTriggerEnterEvent;
    public event Action<Collider> OnTriggerStayEvent;
    public event Action<Collider> OnTriggerExitEvent;

    private float _radius;

    public float Radius
    {
        get
        {
            if (_radius == 0)
            {
                if (TryGetComponent(out SphereCollider sphereCollider))
                    _radius = sphereCollider.radius;
            }

            return _radius;
        }
    }


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