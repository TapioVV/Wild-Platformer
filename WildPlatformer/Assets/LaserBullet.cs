using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    
    
    void Start()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, transform.up);
      
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
