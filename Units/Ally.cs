using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : Unit
{
    public static int ID { get; private set; }

    [SerializeField] private NavMeshAgent _agent;
    private Transform _followPoint;

    private void Start()
    {
        _followPoint = Player.Instance.AllyPositions[ID];
        ID++;
    }

    protected override void Update()
    {
        _agent.SetDestination(_followPoint.position);
        base.Update();
    }

    private void OnDestroy()
    {
        ID = 0;
    }

    protected override void InitStates()
    {
        Dictionary<UnitState, IState> states = new();
        states[UnitState.Idle] = new AgentIdleState(_agent, this);
        states[UnitState.Run] = new AgentRunState(_agent, this);

        Statable = new(states, UnitState.Idle);
    }
}
