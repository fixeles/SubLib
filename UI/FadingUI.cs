using Cysharp.Threading.Tasks;
using ExtensionsMain;
using SubLib.Async;
using SubLib.Extensions;
using UnityEngine;

namespace SubLib.UI
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

        public async UniTask SwitchActive(bool value, float customDuration)
        {
            var token = _cts.Create();

            if (value) gameObject.SetActive(true);
            _group.interactable = value;
            _group.blocksRaycasts = value;
            await _group.FadeAsync(token, value ? 1 : 0, customDuration);
            if (!value) gameObject.SetActive(false);
        }

        public async UniTask SwitchActive(bool value)
        {
            await SwitchActive(value, _fadeDuration);
        }

        private void OnValidate()
        {
            gameObject.TrySetComponent(ref _group);
        }
    }
}