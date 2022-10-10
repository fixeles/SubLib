using UnityEngine;

[RequireComponent(typeof(Hitable))]
public abstract class Unit : MonoBehaviour
{

    public Statable Statable;
    [field: SerializeField] public Animator Animator { get; private set; }
    public Hitable Hitable { get; private set; }


    protected virtual void Awake()
    {
        InitStates();
        Hitable = GetComponent<Hitable>();
    }

    protected virtual void Update()
    {
        Statable.Update();
    }

    protected abstract void InitStates();
}
