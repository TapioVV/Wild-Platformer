using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public float horizontalAcceleration;
    public float horizontalDeacceleration;
    public float jumpControlAcceleration;
    public float jumpControlDeacceleration;
    public float gravity;
    public float maxHorizontalSpeed;

    public BoxCollider2D boxCollider;
   
    public Player player;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public abstract void StateUpdate();
    public abstract void JumpInput();

}
