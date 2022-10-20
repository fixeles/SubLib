using UnityEngine;


public class MovableUIObject : MonoBehaviour
{
    public Transform Target;

    private void Start()
    {
        transform.SetParent(LevelData.Instance.MainCanvas.transform);
    }

    private void OnEnable()
    {
        transform.localPosition = new Vector3(-2000, -2000, 0f);
    }

    private void LateUpdate()
    {
        transform.position = StaticData.Instance.Camera.WorldToScreenPoint(Target.position);
    }
}