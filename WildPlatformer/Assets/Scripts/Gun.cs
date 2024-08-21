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
                audioSource.PlayOneShot(shootSound);
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
}
