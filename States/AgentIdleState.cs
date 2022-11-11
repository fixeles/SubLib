using ExtensionsMbehs;
using UnityEngine;
using UnityEngine.AI;

namespace UtilsSubmodule.States
{
    public struct AgentIdleState : IState
    {
        private NavMeshAgent _agent;
        private Animator _animator;
        private Statable _statable;

        public AgentIdleState(NavMeshAgent agent, Animator animator, Statable statable)
        {
            _agent = agent;
            _animator = animator;
            _statable = statable;
        }

        public void Enter()
        {
            _animator.SetBool(AnimationType.Run.ToString(), false);
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