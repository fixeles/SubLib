using UI.ResourcesView;
using UnityEngine;
using static Enums;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public System.Action OnUpgrade;
    [SerializeField] private AnimationCurve _upgradeValues;
    [SerializeField] private AnimationCurve _prices;

    private IntDataValueSavable _upgradeData;
    private float _maxLvl;

    public bool HasNextLvl => _maxLvl >= CurrentLvl;
    public int CurrentLvl => _upgradeData.Value;
    public int NextLvlPrice => (int)_prices.Evaluate(CurrentLvl);
    public float UpgradedValue => _upgradeValues.Evaluate(CurrentLvl);


    public void Init(string key)
    {
        _maxLvl = _prices.keys[_prices.length - 1].time;
        _upgradeData = new IntDataValueSavable(key);
        OnUpgrade?.Invoke();
    }

    public bool LvlUp()
    {
        if (!HasNextLvl) return false;

        _upgradeData.Value++;
        OnUpgrade?.Invoke();
        return true;
    }

    public bool AbleToUp => HasNextLvl && ResourceHandler.GetResourceCount(ResourceType.Money) >= NextLvlPrice;
}
