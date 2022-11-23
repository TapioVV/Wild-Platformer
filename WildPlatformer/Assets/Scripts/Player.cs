using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] TMP_Text _stateText;
    enum STATES { IDLE, RUN, JUMP, FALL}
    STATES _currentState;

    [SerializeField] float _horizontalAcceleration;
    [SerializeField] float _horizontalDeacceleration;
    [SerializeField] float _jumpControlAcceleration;
    [SerializeField] float _jumpControlDeacceleration;

    [SerializeField] float _timeToJumpPeak;
    [SerializeField] float _jumpHeight;
    [SerializeField] float _jumpDistance;

    [SerializeField] float _smallJump;
    [SerializeField] float _maxVerticalSpeed;
    bool _jumpPressed = false;

    float _maxHorizontalSpeed;
    float _gravity;
    float _jumpSpeed;

    public Vector2 _velocity;

    int _inputAxis;

    float _jumpTimer = 0;
    [SerializeField] float _jumpTimerAmount;
    public bool _grounded;

    Rigidbody2D _rb2D;
    BoxCollider2D _boxCollider2D;


    [SerializeField] Transform _gunPivotTransform;

    #region Inputs
    public void MoveInput(InputAction.CallbackContext context)
    {
        _inputAxis = (int)context.ReadValue<float>();
    }
    public void MouseAim(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());

        float angle = Mathf.Atan2(transform.position.y - mousePosition.y, transform.position.x - mousePosition.x) * Mathf.Rad2Deg + 90;
        _gunPivotTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }
    public void ControllerAim(InputAction.CallbackContext context)
    {
        if (!context.canceled && context.ReadValue<Vector2>() != Vector2.zero)
        {
            float angle = Mathf.Atan2(context.ReadValue<Vector2>().y, context.ReadValue<Vector2>().x) * Mathf.Rad2Deg - 90;
            _gunPivotTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
    #endregion
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _currentState = STATES.IDLE;

        _gravity = (2 * _jumpHeight) / Mathf.Pow(_timeToJumpPeak, 2);
        _jumpSpeed = _gravity * _timeToJumpPeak;
        _maxHorizontalSpeed = _jumpDistance / (2 * _timeToJumpPeak);
    }

    private void Update()
    {
        _jumpTimer = Mathf.MoveTowards(_jumpTimer, -1, Time.deltaTime);

        _stateText.text = _currentState.ToString();
        switch (_currentState)
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
        }

        if(_jumpTimer > 0 && _jumpPressed == true)
        {
            if (_currentState == STATES.IDLE || _currentState == STATES.RUN)
            {
                _jumpPressed = false;
                _velocity.y = _jumpSpeed;
                _currentState = STATES.JUMP;
            }
        }

        _velocity.y = Mathf.Clamp(_velocity.y, -_maxVerticalSpeed, 10000000);
    }

    void Idle()
    {
        _velocity.y = 0;
        if (_inputAxis != 0)
        {
            _currentState = STATES.RUN;
        }
        if (!IsGrounded())
        {
            _currentState = STATES.FALL;
        }
    }
    void Running()
    {
        _velocity.y = 0;
        if (_inputAxis == 0)
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, _horizontalDeacceleration * Time.deltaTime);
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, _inputAxis * _maxHorizontalSpeed, _horizontalAcceleration * Time.deltaTime);
        }


        if(_velocity.x >= -0.1f && _velocity.x <= 0.1f)
        {
            _velocity.x = 0;
            _currentState = STATES.IDLE;
        }
        if (!IsGrounded())
        {
            _currentState = STATES.FALL;
        }
    }
    public void JumpEnter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _jumpPressed = true;
            _jumpTimer = _jumpTimerAmount;
        }
        if (context.canceled)
        {
            if(_currentState == STATES.JUMP)
            {
                Debug.Log("SmallJump");
                _velocity.y = _velocity.y * _smallJump;
            }
        }
    }
    void Jumping()
    {
        if (_inputAxis == 0)
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, _horizontalDeacceleration / _jumpControlDeacceleration * Time.deltaTime);
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, _inputAxis * _maxHorizontalSpeed, _horizontalAcceleration / _jumpControlAcceleration * Time.deltaTime);
        }

        if (IsTouchingCeiling())
        {
            Debug.Log("Osui kattoon");
            _velocity.y = 0;
        }

        if (_velocity.y <= 0)
        {
            _currentState = STATES.FALL;
        }

        _velocity.y -= _gravity * Time.deltaTime;
    }
    void Falling()
    {
        if (_inputAxis == 0)
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, _horizontalDeacceleration / _jumpControlDeacceleration * Time.deltaTime);
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, _inputAxis * _maxHorizontalSpeed, _horizontalAcceleration / _jumpControlAcceleration * Time.deltaTime);
        }

        if (IsGrounded())
        {
            if (_velocity.x >= -0.1f && _velocity.x <= 0.1f)
            {
                _velocity.x = 0;
                _currentState = STATES.IDLE;
            }
            else
            {
                _currentState = STATES.RUN;
            }
        }
        _velocity.y -= _gravity * Time.deltaTime;
    }

    void FixedUpdate()
    {
        _rb2D.velocity = _velocity;
    }

    bool IsGrounded()
    {
        RaycastHit2D ray = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0, Vector2.down, 0.1f, _groundLayerMask);
        return ray.collider != null;
    }
    bool IsTouchingCeiling()
    {
        RaycastHit2D ray = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size, 0, Vector2.up, 0.1f, _groundLayerMask);
        return ray.collider != null;
    }
}
