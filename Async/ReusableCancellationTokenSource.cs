using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ReusableCancellationTokenSource
{
    private CancellationTokenSource _cts;
    public CancellationToken Token => _cts.Token;

    public async Task<CancellationToken> Create()
    {
        await Dispose();

        _cts = CancellationTokenSource.CreateLinkedTokenSource(AsyncCancellation.Token);
        return _cts.Token;
    }

    public async Task Dispose()
    {
        _cts?.Cancel();
        await Task.Yield();
        _cts?.Dispose();
        _cts = null;
    }

    public void Cancel()
    {
        _cts?.Cancel();
    }
}