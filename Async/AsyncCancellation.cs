using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncCancellation : MonoBehaviour
{
    private static CancellationTokenSource _cts;
    public static CancellationToken Token => _cts.Token;

    private void Awake()
    {
        _cts = new();
    }

    public static bool IsCancelled(params CancellationToken[] tokens)
    {
        for (int i = 0; i < tokens.Length; i++)
        {
            if (tokens[i].IsCancellationRequested) return true;
        }

        return false;
    }

    private async void OnDisable()
    {
        _cts?.Cancel();
        await Task.Yield();
        _cts?.Dispose();
    }

    private async void OnDestroy()
    {
        await Task.Yield();
        if (!Application.isPlaying) _cts.Cancel();
    }
}