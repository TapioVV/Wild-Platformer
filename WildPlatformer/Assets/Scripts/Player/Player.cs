using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] UnityEvent OnPlayerWin;
    [SerializeField] UnityEvent OnPlayerDeath;

    public LayerMask GroundLayerMask;
    
    public State CurrentState;

    [HideInInspector] public IdleState idleState;
    [HideInInspector] public RunState runState;
    [HideInInspector] public JumpState jumpState;
    [HideInInspector] public FallState fallState;
    [HideInInspector] public DeadState deadState;

    
    
    [SerializeField] float horizontalAcceleration;
    [SerializeField] float horizontalDeacceleration;
    [SerializeField] float jumpControlAcceleration;
    [SerializeField] float jumpControlDeacceleration;

    [SerializeField] float timeToJumpPeak;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpDistance;

    public float smallJump;
    [SerializeField] float maxVerticalSpeed;
    bool jumpPressed = false;

    [HideInInspector] public float maxHorizontalSpeed;
    float gravity;
    float jumpSpeed;

    public Vector2 velocity;

    public int inputAxis;

    float jumpBufferTimer = 0;
    [SerializeField] float jumpInputBuffer;

    [HideInInspector] public Rigidbody2D rb2D;
    BoxCollider2D boxCollider;
    [HideInInspector] public Animator animator;
    SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem particleSystem;

    [Header("Inputs")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;

    [Header("SoundEvents")]
    public UnityEvent OnPlayerNormalJump;
    [SerializeField] UnityEvent OnPlayerBouncePadJump;

    private void Awake()
    {
        idleState = gameObject.AddComponent<IdleState>();
        runState = gameObject.AddComponent<RunState>();
        fallState = gameObject.AddComponent<FallState>();
        deadState = gameObject.AddComponent<DeadState>();
        jumpState = gameObject.AddComponent<JumpState>();

        rb2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleSystem = GetComponent<ParticleSystem>();

        gravity = (2 * jumpHeight) / Mathf.Pow(timeToJumpPeak, 2);
        jumpSpeed = gravity * timeToJumpPeak;
        maxHorizontalSpeed = jumpDistance / (2 * timeToJumpPeak);

        State[] states = { idleState, runState, jumpState, fallState, deadState };
        for (int i = 0; i < states.Length; i++) 
        {
            states[i].horizontalAcceleration = horizontalAcceleration;
            states[i].horizontalDeacceleration = horizontalDeacceleration;
            states[i].jumpControlAcceleration = jumpControlAcceleration;
            states[i].jumpControlDeacceleration = jumpControlDeacceleration;
            states[i].maxHorizontalSpeed = maxHorizontalSpeed;
            states[i].boxCollider = boxCollider;
            states[i].gravity = gravity;

            states[i].player = this;
            states[i].spriteRenderer = spriteRenderer;
            states[i].animator = animator;
        }
    }
    public void MoveInput(InputAction.CallbackContext context)
    {
        inputAxis = (int)context.ReadValue<float>();
    }
    public void StoppedMoveInput(InputAction.CallbackContext context)
    {
        inputAxis = 0;
    }


    private void OnEnable()
    {
        move.action.canceled += StoppedMoveInput;
        move.action.performed += MoveInput;
        jump.action.started += PressJump;
        LaserBullet.OnLaserJump += Jump;

        jump.action.canceled += jumpState.ReleaseJump; 
    }
    private void OnDisable()
    {
        move.action.canceled -= StoppedMoveInput;
        move.action.performed -= MoveInput;
        jump.action.started -= PressJump;

        jump.action.canceled -= jumpState.ReleaseJump;

    }
    public void PressJump(InputAction.CallbackContext context)
    {
        jumpPressed = true;
        jumpBufferTimer = jumpInputBuffer;
    }
    public void Jump(float multiplier)
    {
        velocity.y = jumpSpeed * multiplier;
    }
    public void JumpBuffer()
    {
        jumpBufferTimer = Mathf.MoveTowards(jumpBufferTimer, -1, Time.deltaTime);
        if (jumpBufferTimer > 0 && jumpPressed == true)
        {
            CurrentState.JumpInput();
        }
    }
    void Start()
    {
        CurrentState = idleState;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            CurrentState = deadState;
            OnPlayerDeath.Invoke();
        }
        if (collision.gameObject.tag == "Win")
        {
            CurrentState = deadState;
            OnPlayerWin.Invoke();
        }
    }
    private void Update()
    {
        CurrentState.StateUpdate();

        JumpBuffer();
        velocity.y = Mathf.Clamp(velocity.y, -maxVerticalSpeed, 10000000);
    }

    public void ChangeState(State newState)
    {
        CurrentState = newState;
    }
    public void JumpOnBouncePad(float bigJump, float smallJump)
    {
        OnPlayerBouncePadJump?.Invoke();

        if (jumpPressed)
        {
            Jump(bigJump);
        }
        if (!jumpPressed)
        {
            Jump(smallJump);
        }
    }


    void FixedUpdate()
    {
        rb2D.velocity = velocity;
    }
    public bool IsGrounded()
    {
        RaycastHit2D ray = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, GroundLayerMask);
        return ray.collider != null;
    }
}
