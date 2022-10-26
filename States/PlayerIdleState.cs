using Game.Scripts.Units;

public struct PlayerIdleState : IState
{
    public void Enter()
    {
    }

    public void Update()
    {
        if (MainJoystick.Instance.IsActive()) Player.Instance.Statable.SetState(UnitState.Run);
    }

    public void Exit()
    {
    }
}
