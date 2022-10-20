using UnityEngine;

[CreateAssetMenu]
public class TransitionCurves : ScriptableObject
{
    [field: SerializeField, Min(0.1f)] public float Duration { get; private set; } = 0.5f;
    [field: SerializeField] public AnimationCurve MoveCurve { get; private set; }
    [field: SerializeField] public AnimationCurve ScaleCurve { get; private set; }
    [field: SerializeField] public AnimationCurve HeightCurve { get; private set; }
}
