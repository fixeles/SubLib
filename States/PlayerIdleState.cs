public struct PlayerIdleState : IState
{
    public void Enter()
    {
        Player.Instance.Animator.SetBool(AnimationType.Run.ToString(), false);
    }

    public void Update()
    {
        if (Player.Instance.Input.IsActive()) Player.Instance.Statable.SetState(UnitState.Run);
    }

    public void Exit()
    {
    }
}
