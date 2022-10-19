using UnityEngine;

public class MovableUIObject : MonoBehaviour
{
    public Transform Target;

    private void OnEnable()
    {
        transform.localPosition = new Vector3(-2000, -2000, 0f);
    }

    private void Update()
    {
        transform.position = StaticData.Instance.Camera.WorldToScreenPoint(Target.position);
    }
}
