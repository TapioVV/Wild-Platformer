using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    AudioSource _as;
    [SerializeField] AudioClip SHOOTSOUND;
    float _shootingTimer;
    [SerializeField] float _shootingCooldown;
    bool _shootingActivated = false;

    private void Start()
    {
        _as = GetComponent<AudioSource>();
    }
    void Update()
    {
        _shootingTimer = Mathf.MoveTowards(_shootingTimer, -1, Time.deltaTime);

        if(_shootingActivated == true)
        {
            if (_shootingTimer <= 0)
            {
                _as.PlayOneShot(SHOOTSOUND);
                Instantiate(_bulletPrefab, transform.position, transform.rotation);
                _shootingTimer = _shootingCooldown;
            }
        }
    }

    public void ShootInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _shootingActivated = true;
        }
        if (context.canceled)
        {
            _shootingActivated = false;
        }
    }
}
