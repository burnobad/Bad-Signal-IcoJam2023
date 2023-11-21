using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleScritpt : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private LineRenderer trajectoryLine;

    private bool canMove;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip startMoveClip;

    private void OnEnable()
    {
        canMove = false;
        trajectoryLine.SetPosition(0, Vector2.down * 10);
        trajectoryLine.SetPosition(1, Vector2.up * 100);
        StartCoroutine(BlinkTrajectory());
    }
    void Update()
    {
        if(canMove) 
        {
            rb.velocity = transform.up * 85;
        }

        if(Mathf.Abs(transform.position.x) > 50 || Mathf.Abs(transform.position.y) > 40)
        {
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator BlinkTrajectory()
    {
        for (int i = 0; i < 2; i++)
        {
            trajectoryLine.enabled = true;
            yield return new WaitForSecondsRealtime(.2f);
            trajectoryLine.enabled = false;
            yield return new WaitForSecondsRealtime(.2f);
        }
        canMove = true;
        audioSource.PlayOneShot(startMoveClip);
    }
}
