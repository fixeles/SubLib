using Game.Scripts.Data;
using UnityEngine;

namespace SubLib.UI
{
    public class MovableUIObject : MonoBehaviour
    {
        [SerializeField] private Transform _customParentOnEnable;
        public Transform Target;
        private Transform _cachedTransform;


        [SerializeField, Min(0)] private int dampingSpeed = 0;

        private void Awake()
        {
            _cachedTransform = transform;
            _cachedTransform.SetParent(_customParentOnEnable
                ? _customParentOnEnable
                : LevelData.Instance.MainCanvas.transform);

            _cachedTransform.localPosition = new Vector3(-3000, -3000, 0f);
        }

        private void OnEnable()
        {
            _cachedTransform.position = StaticData.Instance.MainCamera.WorldToScreenPoint(Target.position);
        }

        private void LateUpdate()
        {
            _cachedTransform.position = dampingSpeed == 0
                ? Target.position
                : Vector3.Lerp(_cachedTransform.position,
                    StaticData.Instance.MainCamera.WorldToScreenPoint(Target.position), Time.deltaTime * dampingSpeed);
        }
    }
}