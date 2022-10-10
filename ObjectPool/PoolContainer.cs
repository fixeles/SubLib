using UnityEngine;

public class PoolContainer : MonoBehaviour
{

    [Header("Money")]
    [SerializeField] private Transform _moneyPoolParent;
    [SerializeField] private Money _moneyPrefab;
    public static ObjectPool<Money> MoneyPool { get; private set; }

    private void Awake()
    {
        MoneyPool = new(_moneyPoolParent, _moneyPrefab);
    }


}
