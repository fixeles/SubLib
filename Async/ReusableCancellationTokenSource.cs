using System.Threading;
using System.Threading.Tasks;

namespace UtilsSubmodule.Async
{
    public class ReusableCancellationTokenSource
    {
        private CancellationTokenSource _cts;
        public CancellationToken Token => _cts.Token;

        public async Task<CancellationToken> Create()
        {
            Cancel();
            await Dispose();

            _cts = CancellationTokenSource.CreateLinkedTokenSource(AsyncCancellation.Token);
            return _cts.Token;
        }

        public void DisposeAfterCancel()
        {
            Cancel();
            _ = Dispose();
        }

        public void Cancel()
        {
            _cts?.Cancel();
        }

        private async Task Dispose()
        {
            await Task.Yield();
            _cts?.Dispose();
            _cts = null;
        }
    }
}