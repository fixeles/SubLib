using System.Collections.Generic;
using System.Threading.Tasks;
using ExtensionsMain;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event System.Action OnAddItem;
    public event System.Action OnRemoveItem;
    [field: SerializeField, Min(0.5f)] public float TransitionDuration { get; private set; } = 0.5f;
    [field: SerializeField] public TransitionCurves Curves { get; set; }
    [SerializeField] private bool _perItemLimit;
    [SerializeField] private int _defaultSize;
    [SerializeField] bool _dynamic;


    public List<ItemType> AwailableTypes;

    [SerializeField] private List<InventoryItem> _items;
    private List<Transform> _positions;


    public List<InventoryItem> Items => _items;
    public int DefaultSize => _defaultSize;
    public bool PerItemLimit => _perItemLimit;
    public bool Dynamic => _dynamic;

    [ExecuteInEditMode]
    private void OnEnable()
    {
        if (Curves == null) Curves = FindObjectOfType<TransitionCurves>();
    }

    public void Clear()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            InventoryItem item = _items[i];
            if (item == null) continue;
            item.SwitchActive(false);
            if (item == null) continue;
            item.transform.parent = null;
            _items[i] = null;
        }
        OnRemoveItem?.Invoke();
    }

    public bool Add(InventoryItem item)
    {
        if (!AwailableTypes.Contains(item.Type)) return false;

        int slotIndex;
        if (_perItemLimit && !HasTypeSpace(item.Type)) return false;

        if (!HasEmptySlot(out slotIndex)) return false;

        _items[slotIndex] = item;
        item.transform.parent = _positions[slotIndex];
        OnAddItem?.Invoke();

        return true;
    }

    public async Task<bool> TransferItem(ItemType itemType, Inventory targetInventory)
    {
        if (!targetInventory.AwailableTypes.Contains(itemType)) return false;

        if (targetInventory.PerItemLimit && !targetInventory.HasTypeSpace(itemType)) return false;
        else if (!targetInventory.HasEmptySlot(out _)) return false;

        InventoryItem removedItem;
        if (!RemoveItem(itemType, out removedItem)) return false;

        var result = await removedItem.MoveTo(targetInventory);
        return result;
    }

    public bool IsEmpty()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] != null) return false;
        }
        Debug.Log(this + " is empty");
        return true;
    }

    public bool HasEmptySlot(out int index)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] != null) continue;

            index = i;
            return true;
        }

        index = -1;
        Debug.Log("Inventory is full");
        return false;
    }

    public bool HasItem(in ItemType item, out int index)
    {
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            if (_items[i] == null || _items[i].Type != item) continue;

            index = i;
            return true;
        }

        index = -1;
        Debug.Log("Item not found");
        return false;
    }

    public bool RemoveItem(in ItemType itemType, out InventoryItem removedItem)
    {
        int itemIndex;
        if (!HasItem(itemType, out itemIndex))
        {
            removedItem = null;
            return false;
        }

        removedItem = _items[itemIndex];
        _items[itemIndex] = null;
        OnRemoveItem?.Invoke();
        TrySort();
        return true;
    }

    public int GetItemsCount(ItemType type)
    {
        int count = 0;
        foreach (var item in _items)
        {
            if (item?.Type == type) count++;
        }
        return count;
    }

    public int GetItemsCount()
    {
        int count = 0;
        foreach (var item in _items)
        {
            if (item != null) count++;
        }
        return count;
    }

    public bool RemoveItems(in ItemType[] request, out List<InventoryItem> removedItems)
    {
        var indices = new List<int>();

        for (int i = 0; i < request.Length; i++)
        {
            for (int j = _items.Count - 1; j >= 0; j--)
            {
                if (indices.Contains(j)) continue;

                if (_items[j]?.Type == request[i])
                {
                    indices.Add(j);
                    break;
                }
            }
        }

        removedItems = new List<InventoryItem>();
        if (indices.Count == request.Length)
        {
            foreach (var index in indices)
            {
                removedItems.Add(_items[index]);
                _items[index] = null;
            }

            OnRemoveItem?.Invoke();
            TrySort();
            return true;
        }
        return false;
    }

    public InventoryItem GetLastItem()
    {
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            if (_items[i] != null)
            {
                return _items[i];
            }
        }
        return null;
    }

    public void SetAwailableTypes(ItemType[] types)
    {
        for (int i = 0; i < types.Length; i++)
        {
            if (AwailableTypes.Contains(types[i])) continue;
            AwailableTypes.Add(types[i]);
        }
    }

    public void SizeUpdate(int desiredSize)
    {
        int missingSlots = desiredSize - _items.Count;
        if (missingSlots == 0) return;

        for (int i = 0; i < missingSlots; i++)
        {
            _items.Add(null);
            NewSlot();
        }
        GetComponent<LayoutGroup3D>()?.RebuildLayout();
    }

    protected virtual void Start()
    {
        SizeUpdate(_defaultSize);
    }

    private void Awake()
    {
        _positions = gameObject.GetActiveChilds();
        if (_items == null) _items = new List<InventoryItem>();
        for (int i = 0; i < _positions.Count; i++)
        {
            _items.Add(null);
        }
    }

    private void NewSlot()
    {
        var newSlot = new GameObject("Slot").transform;
        newSlot.parent = transform;
        newSlot.localPosition = Vector3.zero;
        newSlot.localRotation = Quaternion.identity;
        newSlot.localScale = Vector3.one;
        _positions.Add(newSlot);
    }

    private bool HasTypeSpace(ItemType type)
    {
        return GetItemsCount(type) < _items.Count / AwailableTypes.Count;
    }

    private ItemType[] CreateTypeArray(ItemType type, int count)
    {
        ItemType[] array = new ItemType[count];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = type;
        }
        return array;
    }

    private void TrySort(int startIndex = 0)
    {
        int emptySlot = -1;
        for (int i = startIndex; i < Items.Count; i++)
        {
            if (Items[i] == null)
            {
                emptySlot = i;
                break;
            }
        }
        if (emptySlot == Items.Count - 1) return;

        bool hasTask = false;
        for (int i = emptySlot; i < Items.Count; i++)
        {
            if (Items[i] == null) continue;
            hasTask = true;
            MoveItem(i, emptySlot);
            break;
        }

        if (hasTask) TrySort(emptySlot);
    }

    private void MoveItem(int from, int to)
    {
        Items[to] = Items[from];
        Items[to].transform.parent = _positions[to];

        if (Items[to].Magnet != null) Items[to].Magnet.Target = Items[to].transform.parent;
        else Items[to].transform.localPosition = Vector3.zero;

        Items[from] = null;
    }

    /*
    #if UNITY_EDITOR
        [CustomEditor(typeof(Inventory))]
        public class CurveTransitionEditor : Editor
        {
            private void OnEnable()
            {
                var tgt = (Inventory)target;
                tgt._curveTransition = new AnimBool(false);
                tgt._curveTransition.valueChanged.AddListener(Repaint);
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                var tgt = (Inventory)target;
                tgt._curveTransition.target = EditorGUILayout.ToggleLeft("Curve Transition", tgt._curveTransition.target);
                if (tgt._curveTransition.value)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.ObjectField(tgt.Curves, typeof(TransitionCurves), true);

                    // EditorGUILayout.CurveField("Move curve", tgt._moveCurve);
                    // EditorGUILayout.CurveField("Scale curve", tgt._scaleCurve);
                    // EditorGUILayout.CurveField("Height curve", tgt._heightCurve);

                    EditorGUI.indentLevel--;
                }
            }
        }
    #endif*/
}



