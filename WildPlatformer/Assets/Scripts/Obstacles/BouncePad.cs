using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    
    [SerializeField] float jumpMultiplierBigJump;
    [SerializeField] float jumpMultiplierSmallJump;
    [SerializeField] float activationDelay;
    float activationTimer;
    Player player;
    private void Update()
    {
        activationTimer -= Time.deltaTime;
        activationTimer = Mathf.MoveTowards(activationTimer, -1, Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            if(activationTimer < 0)
            {
                activationTimer = activationDelay;
                player = collision.GetComponent<Player>();
                player.JumpOnBouncePad(jumpMultiplierBigJump, jumpMultiplierSmallJump);
            }
        }
    }
}

