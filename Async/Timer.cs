using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Timer
{
    private const int StepMS = 50;
    public static List<Timer> ActiveTimers { get; private set; }

    private int _currentTime;
    private bool _loop;
    private bool _destroyRequest;
    private System.Action _action;
    private static bool _isStarted;
    private int frequencyMS;
    public int FrequencyMS
    {
        get => frequencyMS;
        set
        {
            frequencyMS = value;
            frequencyMS = Mathf.Clamp(frequencyMS, 1, int.MaxValue);
        }
    }

    public Timer(int _frequencyMS, System.Action action, bool loop = true)
    {
        _action = action;
        FrequencyMS = _frequencyMS;
        _loop = loop;
        _currentTime = 0;
        Init();
    }

    public void Destroy()
    {
        _destroyRequest = true;
    }

    private async void Init()
    {
        await Task.Delay(StepMS * 2);
        if (ActiveTimers == null) ActiveTimers = new List<Timer>();
        ActiveTimers.Add(this);

        if (!_isStarted) StartTimers();
    }

    private static async void StartTimers()
    {
        _isStarted = true;
        int sessisonID = AsyncCancellation.SessionID;

        while (true)
        {
            await Task.Delay(StepMS);
            if (AsyncCancellation.IsCancelled(sessisonID))
            {
                ActiveTimers = new List<Timer>();
                _isStarted = false;
                return;
            }


            for (int i = ActiveTimers.Count - 1; i >= 0; i--)
            {
                Timer timer = ActiveTimers[i];
                if (timer._destroyRequest) timer.Destroy(i);
                if (!timer.IsReady()) continue;
                if (!timer._loop) timer.Destroy(i);
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

    private void Destroy(int index)
    {
        ActiveTimers.RemoveAt(index);
    }
}
