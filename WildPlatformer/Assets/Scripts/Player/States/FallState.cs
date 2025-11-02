using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : State
{
    public override void StateUpdate()
    {
        animator.CrossFade("character_jump_animation", 0, 0);
        if (player.inputAxis == 0)
        {
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, 0, horizontalDeacceleration / jumpControlDeacceleration * Time.deltaTime);
        }
        else
        {
            player.velocity.x = Mathf.MoveTowards(player.velocity.x, player.inputAxis * maxHorizontalSpeed, horizontalAcceleration / jumpControlAcceleration * Time.deltaTime);
        }

        if (player.IsGrounded())
        {
            if (player.velocity.x >= -0.1f && player.velocity.x <= 0.1f)
            {
                player.velocity.x = 0;
                player.ChangeState(player.idleState);
            }
            else
            {
                player.ChangeState(player.runState);
            }
        }
        player.velocity.y -= gravity * Time.deltaTime;
    }

    public override void JumpInput()
    {

    }
}
