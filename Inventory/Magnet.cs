using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private AnimationCurve _forceCurve;
    [SerializeField, ReadOnly] private Transform _target;
    private const float ForceMultiplier = 15;
    private const float Height = 3.5f;
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

            var lerpValue = _target.transform.localPosition.y / Height;

            _force = _forceCurve.Evaluate(lerpValue) * ForceMultiplier;
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

}
