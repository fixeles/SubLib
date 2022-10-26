using UnityEngine;

namespace Game.Scripts.UtilsSubmodule.Inventory
{
    [RequireComponent(typeof(Collider))]
    public class InventoryReceiver : Inventory
    {
        public event System.Action onFull;
        [SerializeField] private int _defaultItemsFrequency = 800;
        private Inventory _currentProvider;
        private Timer _timer;


        public Inventory CurrentProvider => _currentProvider;
        public void DestroyTimer()
        {
            if (_timer != null) _timer.Destroy();
        }


        private void OnTriggerStay(Collider other)
        {
            if (_currentProvider != null) return;

            Player unit;
            if (!other.TryGetComponent<Player>(out unit)) return;

            _currentProvider = unit.Inventory;

            if (_timer != null) _timer.Destroy();
            _timer = new Timer(_defaultItemsFrequency, ReceiveItem);
        }

        private void OnTriggerExit(Collider other)
        {
            Player unit;
            if (!other.TryGetComponent<Player>(out unit)) return;
            if (_currentProvider != unit.Inventory) return;

            DestroyTimer();
            _currentProvider = null;
        }

        private void OnDestroy()
        {
            DestroyTimer();
        }

        private async void ReceiveItem()
        {
            if (_currentProvider == null || this == null) return;

            foreach (var type in AwailableTypes)
            {
                if (!await _currentProvider.TransferItem(type, this)) continue;
                if (!HasEmptySlot(out _))
                {
                    onFull?.Invoke();
                    DestroyTimer();
                    return;
                }
                _timer.FrequencyMS /= 2;
                return;
            }
        }
    }
}