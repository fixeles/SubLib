using System.Threading;

namespace SubLib.Async
{
    public class ReusableCancellationTokenSource
    {
        private CancellationTokenSource _cts;
        private readonly CancellationToken _onDestroyToken;

        public ReusableCancellationTokenSource(CancellationToken onDestroyToken)
        {
            _onDestroyToken = onDestroyToken;
            AsyncCancellation.OnDisposeEvent += Reset;
        }

        ~ReusableCancellationTokenSource() => AsyncCancellation.OnDisposeEvent -= Reset;

        public CancellationToken Token => _cts.Token;
        private void Reset() => _cts = null;

        public CancellationToken Create()
        {
            Cancel();
            //await UniTask.Yield();

            _cts = CancellationTokenSource.CreateLinkedTokenSource(AsyncCancellation.Token, _onDestroyToken);
            AsyncCancellation.DisposePool.Add(_cts);
            return _cts.Token;
        }

        public void Cancel()
        {
            _cts?.Cancel();
        }
    }
}