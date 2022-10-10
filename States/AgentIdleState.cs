using ExtensionsMbehs;
using UnityEngine.AI;


public struct AgentIdleState : IState
{
    private NavMeshAgent _agent;
    private Unit _unit;

    public AgentIdleState(NavMeshAgent agent, Unit unit)
    {
        _agent = agent;
        _unit = unit;
    }

    public void Enter()
    {
        _unit.Animator.SetBool(AnimationType.Run.ToString(), false);
        _agent.isStopped = true;
    }

    public void Update()
    {
        if (!_agent.IsReached()) _unit.Statable.SetState(UnitState.Run);
    }

    public void Exit()
    {
    }
}
