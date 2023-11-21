using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorCollider : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            PlayerController.Instance.DealDamage();
        }
    }
}
