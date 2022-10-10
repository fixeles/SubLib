using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AsyncCancellation : MonoBehaviour
{
    private static List<CancellationTokenSource> _ctsPool = new List<CancellationTokenSource>();

    public static CancellationToken Token => _ctsPool[_ctsPool.Count - 1].Token;
    public static int SessionID { get; private set; }

    private void Awake()
    {
        if (_ctsPool.Count != 0)
        {
            _ctsPool[_ctsPool.Count - 1].Cancel();
            SessionID++;
        }
        _ctsPool.Add(new CancellationTokenSource());
    }

    public static bool IsCancelled(int sessionID)
    {
        return _ctsPool[sessionID].Token.IsCancellationRequested;
    }

    private void OnDestroy()
    {
        if (!Application.isPlaying) _ctsPool[_ctsPool.Count - 1].Cancel();
    }

}
