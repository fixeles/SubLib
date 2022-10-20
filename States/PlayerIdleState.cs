public struct PlayerIdleState : IState
{
    public void Enter()
    {
    }

    public void Update()
    {
        if (Player.Instance.Input.IsActive()) Player.Instance.Statable.SetState(UnitState.Run);
    }

    public void Exit()
    {
    }
}
