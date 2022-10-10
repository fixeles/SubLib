using UnityEngine;

[CreateAssetMenu]
public class TransitionCurves : ScriptableObject
{
    [field: SerializeField] public AnimationCurve MoveCurve { get; private set; }
    [field: SerializeField] public AnimationCurve ScaleCurve { get; private set; }
    [field: SerializeField] public AnimationCurve HeightCurve { get; private set; }
}
