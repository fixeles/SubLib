using Cysharp.Threading.Tasks;
using ExtensionsMain;
using UnityEngine;
using UtilsSubmodule.Async;
using UtilsSubmodule.Extensions;

namespace UtilsSubmodule
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

        public async void SwitchActive(bool value)
        {
            var token = _cts.Create();

            if (value) gameObject.SetActive(true);
            _group.interactable = value;
            await _group.FadeAsync(token, value ? 1 : 0, _fadeDuration);
            if (!value) gameObject.SetActive(false);
        }

        private void OnValidate()
        {
            gameObject.TrySetComponent(ref _group);
        }
    }
}