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

    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] Transform playerTransform;

    [Header("Inputs")]
    [SerializeField] InputActionReference shoot;


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
                Instantiate(bulletPrefab, transform.position, bulletSpawnPoint.rotation);
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
    void StopShoot(InputAction.CallbackContext context)
    {
        shootingActivated = false;
    }
    void StartShoot(InputAction.CallbackContext context)
    {
        shootingActivated = true;
    }
    private void OnEnable()
    {
        shoot.action.started += StartShoot;
        shoot.action.canceled += StopShoot;
    }
    private void OnDisable()
    {
        shoot.action.started -= StartShoot;
        shoot.action.canceled -= StopShoot;
    }
}

