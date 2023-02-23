using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SubLib.UI
{
    public class FadingText : FadingUI
    {
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
        [SerializeField, Min(0)] private float _lifetime = .5f;


        public async UniTaskVoid Show(string message)
        {
            Text.text = message;
            await SwitchActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(_lifetime));
            SwitchActive(false).Forget();
        }
    }
}