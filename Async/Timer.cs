using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UtilsSubmodule.Async
{
    public class Timer
    {
        private const int StepMS = 50;
        private static List<Timer> ActiveTimers { get; set; }
        private static bool _isStarted;

        private int _currentTime;
        private readonly System.Action _action;
        private readonly bool _loop;
        private int _frequencyMS;
        private bool _destroyRequest;

        public int FrequencyMS
        {
            get => _frequencyMS;
            set => _frequencyMS = Mathf.Clamp(value, 1, int.MaxValue);
        }

        public Timer(int frequencyMS, System.Action action, bool loop = true)
        {
            _action = action;
            FrequencyMS = frequencyMS;
            _loop = loop;
            _currentTime = 0;
            Init();
        }

        public void Destroy()
        {
            _destroyRequest = true;
            ActiveTimers.Remove(this);
        }

        private async void Init()
        {
            await Task.Delay(StepMS * 2);
            ActiveTimers ??= new List<Timer>();
            ActiveTimers.Add(this);

            if (_isStarted) return;

            StartTimers();
        }

        private static async void StartTimers()
        {
            _isStarted = true;
            var token = AsyncCancellation.Token;

            while (true)
            {
                await Task.Delay(StepMS);
                if (token.IsCancellationRequested)
                {
                    ActiveTimers.Clear();
                    _isStarted = false;
                    return;
                }

                for (int i = ActiveTimers.Count - 1; i >= 0; i--)
                {
                    Timer timer = ActiveTimers[i];
                    if (timer._destroyRequest) continue;
                    if (!timer.IsReady()) continue;
                    if (!timer._loop) timer.Destroy();
                }
            }
        }

        private bool IsReady()
        {
            _currentTime += StepMS;
            if (_currentTime < FrequencyMS) return false;


            int iterations = _currentTime / FrequencyMS;
            while (iterations > 0)
            {
                iterations--;
                _currentTime -= FrequencyMS;
                _action.Invoke();
            }

            return true;
        }
    }
}