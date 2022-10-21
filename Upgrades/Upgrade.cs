using System;
using TMPro;
using UnityEngine.UI;
using UI.ResourcesView;
using UnityEngine;

namespace Game.Scripts.UtilsSubmodule.Upgrades
{
    public class Upgrade : MonoBehaviour

    {
        [SerializeField] private TextMeshProUGUI _lvlText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _button;
        [SerializeField] private UpgradeData _data;

        private Image _buttonImage;

        private void UpdateText()
        {
            _priceText.text = !_data.HasNextLvl ? "MAX" : _data.NextLvlPrice.ToString();
            _lvlText.text = $"LVL {_data.CurrentLvl + 1}";
        }

        private void SwitchBtn()
        {
            _button.interactable = _data.AbleToUp;
            _buttonImage.sprite = Sprite;
        }

        private Sprite Sprite =>
            _button.interactable ? StaticData.Instance.GreenBtnSprite : StaticData.Instance.GrayBtnSprite;

        private void LvlUp()
        {
            if (ResourceHandler.TrySubtractResource(Enums.ResourceType.Money, _data.NextLvlPrice)) _data.LvlUp();
        }

        private void OnEnable()
        {
            ResourceHandler.OnChanged += SwitchBtn;
            _data.OnUpgrade += UpdateText;
            _data.OnUpgrade += SwitchBtn;

            SwitchBtn();
            UpdateText();
        }

        private void OnDisable()
        {
            ResourceHandler.OnChanged -= SwitchBtn;
            _data.OnUpgrade -= UpdateText;
            _data.OnUpgrade -= SwitchBtn;
        }

        private void Awake()
        {
            _buttonImage = _button.GetComponent<Image>();
            _button.onClick.AddListener(LvlUp);
        }
    }
}