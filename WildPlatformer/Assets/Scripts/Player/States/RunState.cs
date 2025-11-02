using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State
{
    public override void StateUpdate()
    {
        animator.CrossFade("character_run_animation", 0, 0);
        player.velocity.y = -2;

        if (player.inputAxis == 0)
        {
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, 0, horizontalDeacceleration * Time.deltaTime);
        }
        else
        {
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, player.inputAxis * maxHorizontalSpeed, horizontalAcceleration * Time.deltaTime);
        }

        if (player.inputAxis == 1)
        {
            spriteRenderer.flipX = false;
        }
        else if (player.inputAxis == -1)
        {
            spriteRenderer.flipX = true;
        }

        if (player.velocity.x >= -0.1f && player.velocity.x <= 0.1f)
        {
            player.velocity.x = 0;
            player.ChangeState(player.idleState);
        }
        if (!player.IsGrounded())
        {
            player.ChangeState(player.fallState);
        }
    }
    public override void JumpInput()
    {
        player.OnPlayerNormalJump.Invoke();

        player.Jump(1f);
        player.ChangeState(player.jumpState);

    }
}   