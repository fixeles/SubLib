using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ReusableCancellationTokenSource
{
    private CancellationTokenSource _cts;
    public CancellationToken Token => _cts.Token;

    public async Task<CancellationToken> Create()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            await Task.Yield();
        }

        _cts?.Dispose();
        _cts = CancellationTokenSource.CreateLinkedTokenSource(AsyncCancellation.Token);
        return _cts.Token;
    }

    public async void Dispose()
    {
        if (_cts == null) return;
        _cts.Cancel();
        await Task.Yield();
        _cts.Dispose();
        _cts = null;
    }

    public void Cancel()
    {
        _cts?.Cancel();
    }
}