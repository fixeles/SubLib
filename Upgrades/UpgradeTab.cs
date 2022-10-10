using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UI.ResourcesView;

public class UpgradeTab : MonoBehaviour
{
    public static UpgradeTab Instance;
    private UpgradePlace _upgradePlace;
    [Header("Sprites")]
    [SerializeField] private Sprite _enabledSprite;
    [SerializeField] private Sprite _disabledSprite;


    [Header("Speed")]
    [SerializeField] private TextMeshProUGUI _speedLvlText;
    [SerializeField] private TextMeshProUGUI _speedPrice;
    [SerializeField] private Button _buySpeedBtn;
    [SerializeField] private Image _speedBtnImage;
    private Upgrade _currentSpeedUpgrade;

    [Header("Carry")]
    [SerializeField] private TextMeshProUGUI _carryLvlText;
    [SerializeField] private TextMeshProUGUI _carryPrice;
    [SerializeField] private Button _buyCarryBtn;
    [SerializeField] private Image _carryBtnImage;
    private Upgrade _currentInventoryUpgrade;



    public void EnablePanel(UpgradePlace place)
    {
        _upgradePlace = place;
        _currentInventoryUpgrade = place.InventoryUpgrade;
        _currentSpeedUpgrade = place.SpeedUpgrade;

        gameObject.SetActive(true);
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    private void UpdateSpeedText()
    {
        if (!_currentSpeedUpgrade.HasNextLvl) _speedPrice.text = "MAX";
        else _speedPrice.text = _currentSpeedUpgrade.NextLvlPrice.ToString();

        _speedLvlText.text = $"LVL {_currentSpeedUpgrade.CurrentLvl + 1}";
    }

    private void UpdateCarryText()
    {
        if (!_currentInventoryUpgrade.HasNextLvl) _carryPrice.text = "MAX";
        else _carryPrice.text = _currentInventoryUpgrade.NextLvlPrice.ToString();

        _carryLvlText.text = $"LVL {_currentInventoryUpgrade.CurrentLvl + 1}";
    }

    private void SwitchSpeedBtn()
    {
        bool ableToUp = _currentSpeedUpgrade.AbleToUp;

        _buySpeedBtn.interactable = ableToUp;
        _speedBtnImage.sprite = GetSprite(ableToUp);
    }

    private void SwitchCarryBtn()
    {
        bool ableToUp = _currentInventoryUpgrade.AbleToUp;

        _buyCarryBtn.interactable = ableToUp;
        _carryBtnImage.sprite = GetSprite(ableToUp);
    }



    private void OnEnable()
    {
        ResourceHandler.OnChanged += SwitchCarryBtn;
        ResourceHandler.OnChanged += SwitchSpeedBtn;

        _currentSpeedUpgrade.OnUpgrade += UpdateSpeedText;
        _currentSpeedUpgrade.OnUpgrade += SwitchSpeedBtn;

        _currentInventoryUpgrade.OnUpgrade += UpdateCarryText;
        _currentInventoryUpgrade.OnUpgrade += SwitchCarryBtn;

        _buyCarryBtn.onClick.AddListener(CarryUp);
        _buySpeedBtn.onClick.AddListener(SpeedUp);

        SwitchCarryBtn();
        SwitchSpeedBtn();
        UpdateCarryText();
        UpdateSpeedText();
    }

    private void OnDisable()
    {
        ResourceHandler.OnChanged -= SwitchCarryBtn;
        ResourceHandler.OnChanged -= SwitchSpeedBtn;

        _currentSpeedUpgrade.OnUpgrade -= UpdateSpeedText;
        _currentSpeedUpgrade.OnUpgrade -= SwitchSpeedBtn;

        _currentInventoryUpgrade.OnUpgrade -= UpdateCarryText;
        _currentInventoryUpgrade.OnUpgrade -= SwitchCarryBtn;

        _buyCarryBtn.onClick.RemoveListener(CarryUp);
        _buySpeedBtn.onClick.RemoveListener(SpeedUp);
    }

    private void SpeedUp()
    {
        _upgradePlace.Buy(_currentSpeedUpgrade.NextLvlPrice);
        _currentSpeedUpgrade.LvlUp();
    }
    private void CarryUp()
    {
        _upgradePlace.Buy(_currentInventoryUpgrade.NextLvlPrice);
        _currentInventoryUpgrade.LvlUp();
    }



    private Sprite GetSprite(bool enabled) => enabled ? _enabledSprite : _disabledSprite;
}
