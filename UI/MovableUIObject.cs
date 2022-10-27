using UnityEngine;

public class MovableUIObject : MonoBehaviour
{
    public Transform Target;
    private Transform _cachedTransform;

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
        const int dampingSpeed = 20;
        _cachedTransform.position =
            Vector3.Lerp(_cachedTransform.position,
                StaticData.Instance.Camera.WorldToScreenPoint(Target.position), Time.deltaTime * dampingSpeed);
    }
}