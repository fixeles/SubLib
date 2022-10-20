using UnityEngine;

public struct DeathState : IState
{
    public void Enter()
    {
        Player.Instance.Animator.SetTrigger(AnimationType.Death.ToString());
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}