using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D coll)
    { 
        if(coll.CompareTag("Player"))
        {
            GameManager.Instance.inZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    { 
        if(coll.CompareTag("Player"))
        {
            GameManager.Instance.inZone = false;
        }
    }
}
