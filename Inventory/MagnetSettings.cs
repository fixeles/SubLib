using UnityEngine;

namespace Game.Scripts.UtilsSubmodule.Inventory
{
    [CreateAssetMenu]
    public class MagnetSettings : ScriptableObject
    {
        [field: SerializeField] public AnimationCurve ForceCurve;
        [field: SerializeField] public float ForceMultiplier = 15;
        [field: SerializeField] public float Height = 3.5f;
    }
}