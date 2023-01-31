using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SubLib.Async
{
    public class Timer
    {
        private static HashSet<Timer> _activeTimers;
        private static bool _isStarted;

        private float _currentTime;
        private readonly System.Action _action;
        private readonly bool _loop;
        private float _frequency;

        public float Frequency
        {
            get => _frequency;
            set => _frequency = Mathf.Clamp(value, float.Epsilon, float.MaxValue);
        }

        public Timer(float frequency, System.Action action, bool loop = true)
        {
            _action = action;
            Frequency = frequency;
            _loop = loop;
            _currentTime = 0;
            Init();
        }

        public void Destroy()
        {
            _activeTimers.Remove(this);
        }

        private async void Init()
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            _activeTimers ??= new();
            _activeTimers.Add(this);

            if (_isStarted) return;
            StartTimers();
        }

        private static async void StartTimers()
        {
            _isStarted = true;
            var token = AsyncCancellation.Token;

            while (true)
            {
                await UniTask.Yield();
                if (token.IsCancellationRequested)
                {
                    _activeTimers.Clear();
                    _isStarted = false;
                    return;
                }

                foreach (var timer in _activeTimers)
                {
                    if (!timer.ReadyCheck()) continue;
                    if (!timer._loop) timer.Destroy();
                }
            }
        }

        private bool ReadyCheck()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime < Frequency) return false;

            var iterations = _currentTime / Frequency;
            while (iterations > 1)
            {
                iterations--;
                _currentTime -= Frequency;
                _action.Invoke();
            }
            return true;
        }
    }
}