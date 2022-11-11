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

        public void Enter()
        {
            _animator.SetBool(AnimationType.Run.ToString(), true);
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