using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UI.ResourcesView;
using static Enums;

public class Player : Unit
{
    public static Player Instance { get; private set; }
    [field: SerializeField] public PlayerInput Input { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Inventory Inventory { get; private set; }
    [field: SerializeField] public List<Transform> AllyPositions { get; private set; }
    [field: SerializeField] public SphereCollider MoneyCollector { get; private set; }

    [SerializeField] private Upgrade _inventoryUpgrade;
    [SerializeField] private Upgrade _speedUpgrade;
    [SerializeField] private GameObject _inventoryFullIndicator;



    public float MoveSpeedMultiplier { get; private set; } = 1;

    public async Task TakeMoney(Inventory from)
    {
        var items = new List<InventoryItem>();
        for (int i = 0; i < from.Items.Count; i++)
        {
            var item = from.GetLastItem(out _);
            if (item == null) continue;
            items.Add(item);

            _ = from.TransferItem(item.Type, Inventory);
            ResourceHandler.AddResource(ResourceType.Money, 10);
            await Task.Delay(50);
        }

        await Task.Delay((int)(Inventory.TransitionDuration * 1000));
        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
    }

    protected override void Awake()
    {
        Instance = this;
        base.Awake();
    }

    private void Start()
    {
        Input.Init();
        // SpeedUpdate();
        // InventoryUpdate();
        // SwitchInventoryIndicator();

        Inventory.OnAddItem += VibrationCall;
        Inventory.OnRemoveItem += VibrationCall;
        // _inventoryUpgrade.OnUpgrade += InventoryUpdate;
        // _speedUpgrade.OnUpgrade += SpeedUpdate;

        // Inventory.OnAddItem += SwitchInventoryIndicator;
        // Inventory.OnRemoveItem += SwitchInventoryIndicator;
    }

    private void OnDestroy()
    {
        Inventory.OnAddItem -= VibrationCall;
        Inventory.OnRemoveItem -= VibrationCall;
        // _inventoryUpgrade.OnUpgrade -= InventoryUpdate;
        // _speedUpgrade.OnUpgrade -= SpeedUpdate;


        // Inventory.OnAddItem -= SwitchInventoryIndicator;
        // Inventory.OnRemoveItem -= SwitchInventoryIndicator;
    }

    protected override void InitStates()
    {
        Dictionary<UnitState, IState> states = new();
        states[UnitState.Idle] = new PlayerIdleState();
        states[UnitState.Run] = new PlayerRunState();

        Statable = new(states, UnitState.Idle);
    }



    private void VibrationCall()
    {
        MyVibration.Haptic(MyHapticTypes.LightImpact);
    }

    private void SpeedUpdate()
    {
        MoveSpeedMultiplier = _speedUpgrade.UpgradedValue;
        Animator.SetFloat("speed", MoveSpeedMultiplier);
    }

    private void InventoryUpdate()
    {
        Inventory.SizeUpdate((int)_inventoryUpgrade.UpgradedValue);
    }

    private void SwitchInventoryIndicator()
    {
        if (!_inventoryFullIndicator) return;
        _inventoryFullIndicator.SetActive(!Inventory.HasEmptySlot(out _));
    }
}
