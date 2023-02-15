using Cysharp.Threading.Tasks;
using ExtensionsMain;
using Game.Scripts.Attributes;
using UnityEngine;
using SubLib.Async;
using SubLib.Extensions;

namespace SubLib
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadingUI : MonoBehaviour
    {
        [SerializeField, Min(float.Epsilon)] private float _fadeDuration;
        [SerializeField, @ReadOnly] private CanvasGroup _group;

        public CanvasGroup Group => _group;

        private ReusableCancellationTokenSource _cts;

        private void Awake()
        {
            _cts = new(UniTaskCancellationExtensions.GetCancellationTokenOnDestroy(this));
        }

        public async UniTaskVoid SwitchActive(bool value, float customDuration)
        {
            var token = _cts.Create();

            if (value) gameObject.SetActive(true);
            _group.interactable = value;
            _group.blocksRaycasts = value;
            await _group.FadeAsync(token, value ? 1 : 0, customDuration);
            if (!value) gameObject.SetActive(false);
        }

        public void SwitchActive(bool value)
        {
            SwitchActive(value, _fadeDuration).Forget();
        }

        private void OnValidate()
        {
            gameObject.TrySetComponent(ref _group);
        }
    }
}