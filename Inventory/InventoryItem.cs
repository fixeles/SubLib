using System.Threading;
using System.Threading.Tasks;
using ExtensionsAsync;
using UnityEngine;

namespace Game.Scripts.UtilsSubmodule.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        [field: SerializeField] public ItemType Type { get; private set; }

        private CancellationTokenSource _moveCTS;
        private Magnet _magnet;
        public Magnet Magnet => _magnet;

        private void Awake()
        {
            _magnet = GetComponent<Magnet>();
        }

        public async Task<bool> MoveTo(Inventory inventory)
        {
            if (_moveCTS != null)
            {
                _moveCTS.Cancel();
                await Task.Yield();
            }

            _moveCTS = new CancellationTokenSource();

            if (!inventory.Add(this)) return false;

            await transform.CurveMoveAsync(transform.parent, inventory.Curves, _moveCTS.Token);
            if (_moveCTS != null) _moveCTS.Dispose();
            _moveCTS = null;

            if (_magnet)
            {
                _magnet.Target = transform.parent;
                if (!inventory.Dynamic) _magnet.Reset();
            }

            return true;
        }

        public async Task Hide(bool destroy = false)
        {
            await transform.RescaleAsync(transform.localScale, Vector3.zero, default, 0.2f);
            if (_magnet != null) _magnet.Reset();
            transform.localPosition = Vector3.zero;
            if (destroy) Destroy(gameObject);
        }

        public async Task Show()
        {
            await transform.RescaleAsync(Vector3.zero, Vector3.one, default, 0.5f);
        }

        private void OnDestroy()
        {
            if (_moveCTS != null) _moveCTS.Cancel();
        }

        public void SwitchActive(bool value)
        {
            if (_moveCTS != null) _moveCTS.Cancel();

            if (value)
            {
                _ = Show();
            }
            else
            {
                _ = Hide(true);
            }
        }

        public bool IsActive() => gameObject.activeInHierarchy;
    }

    public enum ItemType
    {
        Item
    }
}