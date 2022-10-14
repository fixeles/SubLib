using System.Threading.Tasks;
using System.Threading;

public class ReusableCancellationTokenSource
{
    private CancellationTokenSource _cts;
    public CancellationToken Token => _cts.Token;

    public async Task Create()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            await Task.Yield();
        }
        _cts = new CancellationTokenSource();
    }

    public void Dispose()
    {
        if (_cts != null) _cts.Dispose();
        _cts = null;
    }
}
