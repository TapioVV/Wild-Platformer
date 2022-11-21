using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int InputVector;

    Rigidbody2D _rb2D;
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        _rb2D.velocity = new Vector2(InputVector, 0);
    }

    public void Move(InputAction.CallbackContext context)
    {
        InputVector = (int)context.ReadValue<float>();
    }

    public void Aim(InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            float angle = Mathf.Atan2(context.ReadValue<Vector2>().y, context.ReadValue<Vector2>().x) * Mathf.Rad2Deg - 90;
            Debug.Log(angle);
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
