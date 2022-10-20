using UnityEngine;

public struct PlayerRunState : IState
{
    public void Enter()
    {
        Player.Instance.Animator.SetBool(AnimationType.Run.ToString(), true);
    }

    public void Update()
    {
        const float defaultMoveSpeed = 5;
        Vector3 moveDirrection =
            Player.Instance.Input.Direction * defaultMoveSpeed * Player.Instance.MoveSpeedMultiplier;
        Player.Instance.Controller.SimpleMove(moveDirrection);

        Vector3 velocity = new Vector3(Player.Instance.Controller.velocity.x, 0, Player.Instance.Controller.velocity.z);
        if (velocity.magnitude > 0)
        {
            Player.Instance.transform.rotation = Quaternion.LookRotation(velocity.normalized);
        }

        if (!Player.Instance.Input.IsActive()) Player.Instance.Statable.SetState(UnitState.Idle);
    }

    public void Exit()
    {
        Player.Instance.Animator.SetBool(AnimationType.Run.ToString(), false);
    }
}