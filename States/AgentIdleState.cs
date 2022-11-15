using ExtensionsMbehs;
using UnityEngine;
using UnityEngine.AI;

namespace UtilsSubmodule.States
{
    public struct AgentIdleState : IState
    {
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;
        private readonly Statable _statable;
        private static readonly int RunHash = Animator.StringToHash("Run");

        public AgentIdleState(NavMeshAgent agent, Animator animator, Statable statable)
        {
            _agent = agent;
            _animator = animator;
            _statable = statable;
        }

        public void Enter()
        {
            _animator.SetBool(RunHash, false);
            _agent.isStopped = true;
        }

        public void Update()
        {
            if (!_agent.IsReached()) _statable.SetState(UnitState.Run);
        }

        public void Exit()
        {
        }
    }
}