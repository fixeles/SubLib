using Game.Scripts.MonoBehaviours;
using UnityEngine;

namespace UtilsSubmodule.States
{
    public class AgentDeathState: IState
    {
        private readonly Unit _unit;
        private static readonly int DeathHash = Animator.StringToHash("Death");

        public AgentDeathState(Unit unit)
        {
            _unit = unit;
        }

        public void Enter()
        {
            _unit.Agent.destination = _unit.transform.position;
            _unit.Agent.isStopped = true;
            _unit.Animator.SetTrigger(DeathHash);
        }

        public void Update()
        { 
        }

        public void Exit()
        { 
        }
    }
}