using UnityEngine;
using UnityEngine.InputSystem;
public class Gun : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;

    AudioSource audioSource;
    [SerializeField] AudioClip shootSound;
    float shootingTimer;
    [SerializeField] float shootingCooldown;
    bool shootingActivated = false;
    [SerializeField] Transform gunPivotTransform;

    [Header("Inputs")]
    [SerializeField] InputActionReference shoot;
    [SerializeField] InputActionReference controllerAim;
    [SerializeField] InputActionReference mouseAim;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        shootingTimer = Mathf.MoveTowards(shootingTimer, -1, Time.deltaTime);
        if (shootingActivated == true)
        {
            if (shootingTimer <= 0)
            {

                Instantiate(bulletPrefab, transform.position, gunPivotTransform.rotation);
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
    void StartShoot(InputAction.CallbackContext context)
    {
        shootingActivated = true;
    }
    void StopShoot(InputAction.CallbackContext context)
    {
        shootingActivated = true;
    }
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
        shoot.action.canceled += StartShoot;
        shoot.action.started += StopShoot;
        controllerAim.action.performed += ControllerAim;
        mouseAim.action.performed += MouseAim;
    }
    private void OnDisable()
    {
        shoot.action.canceled -= StartShoot;
        shoot.action.started -= StopShoot;
        controllerAim.action.performed -= ControllerAim;
        mouseAim.action.performed -= MouseAim;
    }
}

