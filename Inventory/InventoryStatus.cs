using UnityEngine;
using TMPro;

public class InventoryStatus : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private TextMeshProUGUI _storageText;

    private void Start()
    {
        _inventory.OnAddItem += UpdateText;
        _inventory.OnRemoveItem += UpdateText;
    }
    private void OnEnable()
    {
        UpdateText();
    }

    private void OnDestroy()
    {
        _inventory.OnAddItem -= UpdateText;
        _inventory.OnRemoveItem -= UpdateText;
    }

    private void UpdateText()
    {
        _storageText.text = $"{_inventory.GetItemsCount()}/{_inventory.Items.Count}";
    }
}
