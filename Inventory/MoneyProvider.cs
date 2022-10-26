using UnityEngine;

namespace Game.Scripts.UtilsSubmodule.Inventory
{
    [RequireComponent(typeof(Collider))]
    public class MoneyProvider : MonoBehaviour
    {
        [SerializeField] private Inventory _moneyStorage;
        private Inventory _target;
        private void OnTriggerStay(Collider other)
        {
            if (_target != null) return;

            Player player;
            if (!other.TryGetComponent<Player>(out player)) return;

            _target = player.Inventory;
            TransferItems();
            _moneyStorage.OnAddItem += TransferItems;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Player>(out _)) return;

            _target = null;
            TransferItems();
            _moneyStorage.OnAddItem -= TransferItems;
        }

        private void TransferItems()
        {
            if (_target == null) return;
            if (_moneyStorage.IsEmpty()) return;

            _ = Player.Instance.TakeMoney(_moneyStorage);
        }
    }
}
