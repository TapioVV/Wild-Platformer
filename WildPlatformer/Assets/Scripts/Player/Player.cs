using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] public static event Action OnPlayerDeath;
    [SerializeField] UnityEvent OnPlayerDeathUnityEvent;

    [SerializeField] LayerMask groundLayerMask;
    enum STATES { IDLE, RUN, JUMP, FALL, DEAD }
    STATES currentState;

    [SerializeField] float horizontalAcceleration;
    [SerializeField] float horizontalDeacceleration;
    [SerializeField] float jumpControlAcceleration;
    [SerializeField] float jumpControlDeacceleration;

    [SerializeField] float timeToJumpPeak;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpDistance;

    [SerializeField] float smallJump;
    [SerializeField] float maxVerticalSpeed;
    bool jumpPressed = false;

    float maxHorizontalSpeed;
    float gravity;
    float jumpSpeed;

    public Vector2 velocity;

    int inputAxis;

    float jumpBufferTimer = 0;
    [SerializeField] float jumpInputBuffer;
    public bool grounded;

    Rigidbody2D rb2D;
    BoxCollider2D boxCollider;
    Animator animator;
    SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem particleSystem;

    [Header("Inputs")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;

    #region Inputs
    public void MoveInput(InputAction.CallbackContext context)
    {
        inputAxis = (int)context.ReadValue<float>();
    }
    public void ResetAfterDeathInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }
    public void ResetInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }
    public void LeaveToMenuInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
        }
    }

    private void OnEnable()
    {
        move.action.performed += MoveInput;
        jump.action.started += PressJump;
        LaserBullet.OnLaserJump += Jump;

        jump.action.canceled += ReleaseJump;
    }
    private void OnDisable()
    {
        move.action.performed -= MoveInput;
        jump.action.started -= PressJump;

        jump.action.canceled -= ReleaseJump;
    }

    #endregion
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleSystem = GetComponent<ParticleSystem>();

        currentState = STATES.IDLE;

        gravity = (2 * jumpHeight) / Mathf.Pow(timeToJumpPeak, 2);
        jumpSpeed = gravity * timeToJumpPeak;
        maxHorizontalSpeed = jumpDistance / (2 * timeToJumpPeak);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
            rb2D.velocity = Vector2.zero;
            OnPlayerDeath?.Invoke();
            OnPlayerDeathUnityEvent.Invoke();
            currentState = STATES.DEAD;
        }
    }
    private void Update()
    {
        jumpBufferTimer = Mathf.MoveTowards(jumpBufferTimer, -1, Time.deltaTime);

        switch (currentState)
        {
            case STATES.IDLE:
                Idle();
                break;

            case STATES.RUN:
                Running();
                break;

            case STATES.JUMP:
                Jumping();
                break;

            case STATES.FALL:
                Falling();
                break;
            case STATES.DEAD:
                rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
                rb2D.velocity = Vector2.zero;

                break;
        }
        if (jumpBufferTimer > 0 && jumpPressed == true)
        {
            if (currentState == STATES.IDLE || currentState == STATES.RUN)
            {
                Jump(1f);
            }
        }

        velocity.y = Mathf.Clamp(velocity.y, -maxVerticalSpeed, 10000000);
    }

    void Idle()
    {
        animator.CrossFade("character_idle_animation", 0, 0);
        velocity.y = -2;
        if (inputAxis != 0)
        {
            currentState = STATES.RUN;
        }
        if (!IsGrounded())
        {
            currentState = STATES.FALL;
        }
    }
    void Running()
    {
        animator.CrossFade("character_run_animation", 0, 0);
        velocity.y = -2;

        if (inputAxis == 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, horizontalDeacceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * maxHorizontalSpeed, horizontalAcceleration * Time.deltaTime);
        }

        if (inputAxis == 1)
        {
            spriteRenderer.flipX = false;
        }
        else if (inputAxis == -1)
        {
            spriteRenderer.flipX = true;
        }

        if (velocity.x >= -0.1f && velocity.x <= 0.1f)
        {
            velocity.x = 0;
            currentState = STATES.IDLE;
        }
        if (!IsGrounded())
        {
            currentState = STATES.FALL;
        }
    }
    public void PressJump(InputAction.CallbackContext context)
    {
        jumpPressed = true;
        jumpBufferTimer = jumpInputBuffer;
    }
    public void ReleaseJump(InputAction.CallbackContext context)
    {
        jumpPressed = false;
        if (currentState == STATES.JUMP)
        {
            velocity.y = velocity.y * smallJump;
        }
    }
    public void JumpOnBouncePad(float bigJump, float smallJump)
    {
        if (jumpPressed)
        {
            Jump(bigJump);
        }
        if (!jumpPressed)
        {
            Jump(smallJump);
        }
    }
    public void Jump(float multiplier)
    {
        velocity.y = jumpSpeed * multiplier;
        currentState = STATES.JUMP;
    }

    void Jumping()
    {
        animator.CrossFade("character_jump_animation", 0, 0);
        if (inputAxis == 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, horizontalDeacceleration / jumpControlDeacceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * maxHorizontalSpeed, horizontalAcceleration / jumpControlAcceleration * Time.deltaTime);
        }

        if (IsTouchingCeiling())
        {
            velocity.y = 0;
        }

        if (velocity.y <= 0)
        {
            currentState = STATES.FALL;
        }

        velocity.y -= gravity * Time.deltaTime;
    }
    void Falling()
    {
        animator.CrossFade("character_jump_animation", 0, 0);
        if (inputAxis == 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, horizontalDeacceleration / jumpControlDeacceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * maxHorizontalSpeed, horizontalAcceleration / jumpControlAcceleration * Time.deltaTime);
        }

        if (IsGrounded())
        {
            if (velocity.x >= -0.1f && velocity.x <= 0.1f)
            {
                velocity.x = 0;
                currentState = STATES.IDLE;
            }
            else
            {
                currentState = STATES.RUN;
            }
        }
        velocity.y -= gravity * Time.deltaTime;
    }

    void FixedUpdate()
    {
        rb2D.velocity = velocity;
    }

    bool IsGrounded()
    {
        RaycastHit2D ray = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayerMask);
        return ray.collider != null;
    }
    bool IsTouchingCeiling()
    {
        RaycastHit2D ray = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, groundLayerMask);
        return ray.collider != null;
    }
}
