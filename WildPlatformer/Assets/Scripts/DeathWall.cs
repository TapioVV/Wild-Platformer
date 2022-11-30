using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{   

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = transform.position + new Vector3(35 * Time.deltaTime, 0, 0);
    }
}
