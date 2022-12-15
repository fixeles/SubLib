using System.Threading;
using System.Threading.Tasks;

namespace UtilsSubmodule.Async
{
    public class ReusableCancellationTokenSource
    {
        private CancellationTokenSource _cts;

        public ReusableCancellationTokenSource()
        {
            AsyncCancellation.OnDisposeEvent += Reset;
        }

        ~ReusableCancellationTokenSource()
        {
            AsyncCancellation.OnDisposeEvent -= Reset;
        }

        private void Reset()
        {
            _cts = null;
        }

        public CancellationToken Token => _cts.Token;

        public async Task<CancellationToken> Create()
        {
            Cancel();
            await Task.Yield();


            _cts = CancellationTokenSource.CreateLinkedTokenSource(AsyncCancellation.Token);
            AsyncCancellation.DisposePool.Add(_cts);
            return _cts.Token;
        }

        public void Cancel()
        {
            _cts?.Cancel();
        }
    }
}