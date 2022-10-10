using System;
using UnityEngine;

[Serializable]
public class PlayerInput
{
    [SerializeField] private Joystick _joystick;
    private float _cos = 1;
    private float _sin;

    public bool IsActive()
    {
        const float joystickTrashold = 0.3f;
        return _joystick.Direction.magnitude > joystickTrashold;
    }

    public Vector3 Direction => GetNormalizedWorldDirection();
    public Joystick Joystick => _joystick;

    public void Init()
    {
        float angle = -Camera.main.transform.rotation.eulerAngles.y;
        _cos = Mathf.Cos(angle * Mathf.Deg2Rad);
        _sin = Mathf.Sin(angle * Mathf.Deg2Rad);
    }

    private Vector3 GetNormalizedWorldDirection()
    {
        Vector2 direction = _joystick.Direction;
        float x = direction.x * _cos - direction.y * _sin;
        float z = direction.x * _sin + direction.y * _cos;

        return new Vector3(x, 0, z).normalized;
    }
}