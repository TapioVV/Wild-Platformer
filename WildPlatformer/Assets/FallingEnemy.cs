using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingEnemy : MonoBehaviour
{
    Rigidbody2D _rb2D;
    [SerializeField] float fallSpeed;
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _rb2D.gravityScale = fallSpeed;
        }
    }

    void Update()
    {
        
    }
}
