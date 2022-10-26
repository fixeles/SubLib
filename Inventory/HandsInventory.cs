using UnityEngine;

namespace Game.Scripts.UtilsSubmodule.Inventory
{
    public class HandsInventory : global::Game.Scripts.UtilsSubmodule.Inventory.Inventory
    {
        [SerializeField] private Animator _animator;

        protected override void Start()
        {
            base.Start();
            OnAddItem += SwitchHands;
            OnRemoveItem += SwitchHands;
        }

        private void OnDestroy()
        {
            OnAddItem -= SwitchHands;
            OnRemoveItem -= SwitchHands;
            StopAllCoroutines();
        }

        private void SwitchHands()
        {
            _animator.SetBool("HandsUp", GetItemsCount() > 0);
        }

        /*
        private async void UpHands()
        {
            if (GetItemsCount() == 1) return;
            if (_cts != null) _cts.Cancel();
            _cts = new CancellationTokenSource();

            while (_rig.weight < 1)
            {
                _rig.weight += Time.deltaTime;
                await Task.Yield();
                if (_cts.Token.IsCancellationRequested) return;
            }

            if (_cts != null) _cts.Dispose();
        }

        private async void DownHands()
        {
            if (!IsEmpty()) return;
            if (_cts != null) _cts.Cancel();
            _cts = new CancellationTokenSource();

            while (_rig.weight > 0)
            {
                _rig.weight -= Time.deltaTime;
                await Task.Yield();
                if (_cts.Token.IsCancellationRequested) return;
            }

            if (_cts != null) _cts.Dispose();
        }*/
    }
}
