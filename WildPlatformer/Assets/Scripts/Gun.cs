using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    
    float _shootingTimer;
    [SerializeField] float _shootingCooldown;
    bool _shootingActivated = false;

    void Update()
    {
        _shootingTimer = Mathf.MoveTowards(_shootingTimer, -1, Time.deltaTime);

        if(_shootingActivated == true)
        {
            if (_shootingTimer <= 0)
            {
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
