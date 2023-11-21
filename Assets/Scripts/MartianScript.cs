using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MartianScript : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.Instance.AddScore(500);
        gameObject.SetActive(false);
    }
}
