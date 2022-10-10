using ExtensionsMbehs;
using UnityEngine.AI;


public struct AgentRunState : IState
{
    private NavMeshAgent _agent;
    private Unit _unit;

    public AgentRunState(NavMeshAgent agent, Unit unit)
    {
        _agent = agent;
        _unit = unit;
    }
    
    public void Enter()
    {
        _unit.Animator.SetBool(AnimationType.Run.ToString(), true);
        _agent.isStopped = false;
    }

    public void Update()
    {
        if (_agent.IsReached()) _unit.Statable.SetState(UnitState.Idle);
    }

    public void Exit()
    {
    }
}
