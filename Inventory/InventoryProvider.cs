using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InventoryProvider : Inventory
{
    private Inventory _target;
    private void OnTriggerStay(Collider other)
    {
        if (_target != null) return;

        Player unit;
        if (!other.TryGetComponent<Player>(out unit)) return;

        _target = unit.Inventory;
        TransferItems();
        OnAddItem += TransferItems;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Unit>(out _)) return;

        _target = null;
        TransferItems();
        OnAddItem -= TransferItems;
    }

    private async void TransferItems()
    {
        if (_target == null) return;
        if (IsEmpty()) return;

        var cachedInventoryForAsync = _target;
        for (int i = Items.Count - 1; i >= 0; i--)
        {
            if (!cachedInventoryForAsync.HasEmptySlot(out _)) return;
            InventoryItem item = Items[i];
            if (item == null) continue;

            _ = TransferItem(item.Type, cachedInventoryForAsync);
            await Task.Delay(15);
        }
    }
}
