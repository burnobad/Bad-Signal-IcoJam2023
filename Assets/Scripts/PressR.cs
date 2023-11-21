using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressR : MonoBehaviour
{

    [SerializeField]
    private GameObject rText;
    [SerializeField]
    private float time;
    private void OnEnable()
    {
        rText.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        rText.active = !rText.active;
        yield return new WaitForSeconds(time);
        StartCoroutine(Blink());
    }
}
