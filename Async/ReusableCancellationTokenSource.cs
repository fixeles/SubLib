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
        _cts = new CancellationTokenSource();
        return _cts.Token;
    }

    public void Dispose()
    {
        _cts?.Dispose();
        _cts = null;
    }

    public void Cancell()
    {
        _cts?.Cancel();
    }
}