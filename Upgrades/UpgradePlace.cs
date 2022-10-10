using UnityEngine;
using SquareDino.Utils;
using UI.ResourcesView;
using static Enums;

public class UpgradePlace : MonoBehaviour
{
    [field: SerializeField] public Upgrade InventoryUpgrade { get; private set; }
    [field: SerializeField] public Upgrade SpeedUpgrade { get; private set; }

    [Header("VFX")]
    [SerializeField] private ParticleSystem _upgradeParticles;

    public void Buy(int price)
    {
        if (!ResourceHandler.TrySubtractResource(ResourceType.Money, price)) return;

        _upgradeParticles.Play();
        TryDestroy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Tags.Player)) return;
        UpgradeTab.Instance.EnablePanel(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(Tags.Player)) return;
        UpgradeTab.Instance.DisablePanel();
    }

    private void Awake()
    {
        InventoryUpgrade.Init("inventory_upgrade");
        SpeedUpgrade.Init("speed_upgrade");
        TryDestroy();
    }

    private void TryDestroy()
    {
        if (!InventoryUpgrade.HasNextLvl && !SpeedUpgrade.HasNextLvl)
        {
            UpgradeTab.Instance.DisablePanel();
            gameObject.SetActive(false);
            Destroy(gameObject, 5);
        }
    }
}

