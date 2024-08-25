using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThisObject : MonoBehaviour
{
    public void DestroyMyself()
    {
        Destroy(gameObject);
    }
}
