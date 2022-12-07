using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace UtilsSubmodule.Async
{
    public class AsyncCancellation : MonoBehaviour
    {
        public static readonly List<IDisposable> DisposePool = new();
        private static CancellationTokenSource _cts;
        public static CancellationToken Token => _cts.Token;

        private void Awake()
        {
            _cts = new();
        }

        private static async void Dispose()
        {
            await Task.Yield();
            for (int i = 0; i < DisposePool.Count; i++)
            {
                DisposePool[i].Dispose();
            }

            DisposePool.Clear();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private async void OnDisable()
        {
            Dispose();
            var cts = _cts;
            cts.Cancel();
            await Task.Delay(30000);
            cts.Dispose();
        }
    }
}