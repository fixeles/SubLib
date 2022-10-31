using UnityEngine;

public class MovableUIObject : MonoBehaviour
{
    public Transform Target;
    private Transform _cachedTransform;

    [SerializeField, Min(0)] private int dampingSpeed = 20;

    private void Awake()
    {
        _cachedTransform = transform;
        _cachedTransform.SetParent(LevelData.Instance.MainCanvas.transform);
        _cachedTransform.localPosition = new Vector3(-3000, -3000, 0f);
    }

    private void OnEnable()
    {
        _cachedTransform.position = StaticData.Instance.Camera.WorldToScreenPoint(Target.position);
    }

    private void LateUpdate()
    {
        _cachedTransform.position = dampingSpeed == 0
            ? Target.position
            : Vector3.Lerp(_cachedTransform.position,
                StaticData.Instance.Camera.WorldToScreenPoint(Target.position), Time.deltaTime * dampingSpeed);
    }
}