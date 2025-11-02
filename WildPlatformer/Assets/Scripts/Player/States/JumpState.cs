using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.VersionControl.Asset;

public class JumpState : State
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

        if (IsTouchingCeiling())
        {
            player.velocity.y = 0;
        }

        if (player.velocity.y <= 0)
        {
            player.ChangeState(player.fallState);
        }

        player.velocity.y -= gravity * Time.deltaTime;
        
    }
    bool IsTouchingCeiling()
    {
        RaycastHit2D ray = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, player.GroundLayerMask);
        return ray.collider != null;
    }

    public override void JumpInput()
    {

    }
    public void ReleaseJump(InputAction.CallbackContext context)
    {
        player.velocity.y = player.velocity.y * player.smallJump;
    }
}
