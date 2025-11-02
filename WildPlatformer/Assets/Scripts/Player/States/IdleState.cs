using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class IdleState : State
{
    public override void StateUpdate()
    {
        animator.CrossFade("character_idle_animation", 0, 0);

        player.velocity.y = -2;
        if (player.inputAxis != 0)   
        {
            player.ChangeState(player.runState);
        }
        if (!player.IsGrounded())
        {
            player.ChangeState(player.fallState);
        }
    }

    public override void JumpInput()
    {
        player.Jump(1f);
        player.OnPlayerNormalJump.Invoke();

        player.CurrentState = player.jumpState;
    }
}
