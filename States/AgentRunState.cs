using ExtensionsMbehs;
using Game.Scripts.MonoBehaviours;
using UnityEngine;
using UnityEngine.AI;

namespace UtilsSubmodule.States
{
    public class AgentRunState : IState
    {
        private readonly Unit _unit;
        private static readonly int RunHash = Animator.StringToHash("Run");

        public AgentRunState(Unit unit)
        {
            _unit = unit;
        }

        public void Enter()
        {
            _unit.Animator.SetBool(RunHash, true);
            _unit.Agent.isStopped = false;
        }

        public void Update()
        {
            if (_unit.Agent.IsReached()) _unit.Statable.SetState(UnitState.Idle);
        }

        public void Exit()
        {
        }
    }
}