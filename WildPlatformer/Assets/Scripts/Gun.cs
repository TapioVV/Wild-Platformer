using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class Gun : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    AudioSource audioSource;
    [SerializeField] AudioClip ShootSound;
    float shootingTimer;
    [SerializeField] float shootingCooldown;
    bool shootingActivated = false;
    [SerializeField] Transform gunPivotTransform;
    [SerializeField] Transform playerTransform;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        shootingTimer = Mathf.MoveTowards(shootingTimer, -1, Time.deltaTime);

        if(shootingActivated == true)
        {
            if (shootingTimer <= 0)
            {
                audioSource.PlayOneShot(ShootSound);
                Instantiate(bulletPrefab, transform.position, transform.rotation);
                shootingTimer = shootingCooldown;
            }
        }
    }
    public void ShootInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            shootingActivated = true;
        }
        if (context.canceled)
        {
            shootingActivated = false;
        }
    }
    public void MouseAim(InputAction.CallbackContext context)
    {

        Debug.Log("Jee");
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());

        float angle = Mathf.Atan2(playerTransform.position.y - mousePosition.y, playerTransform.position.x - mousePosition.x) * Mathf.Rad2Deg + 90;
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
}
