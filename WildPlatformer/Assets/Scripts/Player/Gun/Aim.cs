using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    [SerializeField] Transform gunPivotTransform;
    [Header("Inputs")]
    [SerializeField] InputActionReference controllerAim;
    [SerializeField] InputActionReference mouseAim;

    public void MouseAim(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        float angle = Mathf.Atan2(transform.position.y - mousePosition.y, transform.position.x - mousePosition.x) * Mathf.Rad2Deg + 90;
        gunPivotTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }
    public void ControllerAim(InputAction.CallbackContext context)
    {
        if (!context.canceled && context.ReadValue<Vector2>() != Vector2.zero)
        {
            float angle = Mathf.Atan2(context.ReadValue<Vector2>().y, context.ReadValue<Vector2>().x) * Mathf.Rad2Deg - 90;
            gunPivotTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
    private void OnEnable()
    {
        controllerAim.action.performed += ControllerAim;
        mouseAim.action.performed += MouseAim;
    }
    private void OnDisable()
    {
        controllerAim.action.performed -= ControllerAim;
        mouseAim.action.performed -= MouseAim;
    }

}
