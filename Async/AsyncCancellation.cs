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
        var cts = _cts;
        _cts.Cancel();
        await Task.Delay(30000);
        _cts?.Dispose();
    }
}