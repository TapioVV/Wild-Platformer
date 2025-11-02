using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    public override void StateUpdate()
    {
        player.rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
        player.rb2D.velocity = Vector2.zero;
    }

    public  override void JumpInput()
    {

    }
}
