using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ExtensionsAsync;
using System.Threading.Tasks;
using ExtensionsMain;

public class Enemy : Unit
{
    public static event System.Action OnKillEvent;
    private static int _lastUnitID;
    [SerializeField] private NavMeshAgent _agent;
    [field: SerializeField] public Transform AimPoint { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _lastUnitID++;
        _agent.avoidancePriority = _lastUnitID;
        Hitable.OnDeath += Death;
    }

    protected override void InitStates()
    {
        Dictionary<UnitState, IState> states = new();
        states[UnitState.Idle] = new AgentIdleState(_agent, this);
        states[UnitState.Run] = new AgentRunState(_agent, this);

        Statable = new(states, UnitState.Idle);
    }

    private void OnDestroy()
    {
        Hitable.OnDeath -= Death;
    }

    protected override void Update()
    {
        base.Update();
        _agent.SetDestination(Player.Instance.transform.position);
    }

    private async void Death()
    {
        OnKillEvent?.Invoke();
        SpawnMoney();
        Animator.SetTrigger(AnimationType.Death.ToString());
        _agent.isStopped = true;
        Vector3 downTo = transform.position;
        downTo.y -= 3;

        var selfTF = transform;
        var selfGO = gameObject;
        Destroy(this);
        await Task.Delay(3000);
        await selfTF.MoveAsync(downTo, default, 1);
        Destroy(selfGO);
    }

    private void SpawnMoney()
    {
        for (int i = 0; i < Random.Range(5, 11); i++)
        {
            PoolContainer.MoneyPool.Get(transform.position + Utils.Vector3.Displace(0.5f), transform.rotation.RandomRotation());
        }
    }
}
