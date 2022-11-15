using ExtensionsMbehs;
using UnityEngine;
using UnityEngine.AI;

namespace UtilsSubmodule.States
{
    public struct AgentRunState : IState
    {
        private NavMeshAgent _agent;
        private Animator _animator;
        private Statable _statable;
        private static readonly int RunHash = Animator.StringToHash("Run");

        public AgentRunState(NavMeshAgent agent, Animator animator, Statable statable)
        {
            _agent = agent;
            _animator = animator;
            _statable = statable;
        }

        public void Enter()
        {
            _animator.SetBool(RunHash, true);
            _agent.isStopped = false;
        }

        public void Update()
        {
            if (_agent.IsReached()) _statable.SetState(UnitState.Idle);
        }

        public void Exit()
        {
        }
    }
}