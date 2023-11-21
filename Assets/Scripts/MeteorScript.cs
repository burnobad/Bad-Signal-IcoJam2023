using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    private Vector3 landPos;
    private Vector3 landDir;

    private int steps = 6;
    private float radius = 3;

    private float curDist;
    private float dist;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Collider2D coll;

    [SerializeField]
    private GameObject landingCircle;

    [SerializeField]
    private Circle myCircle;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip landClip;
    void OnEnable()
    {
        landPos = new Vector2(Random.Range(-23, 23), Random.Range(-13, 13));
        landDir = landPos - transform.position;

        coll.enabled = false;

        dist = Vector2.Distance(transform.position, landPos);
        curDist = dist;

        landingCircle.transform.position = landPos;
        myCircle.DrawCircle(steps, radius);
    }

    private void Update()
    {
        curDist = Vector2.Distance(transform.position, landPos);
        if (curDist > 0.25f)
        {
            landDir = landPos - transform.position;
            rb.velocity = landDir.normalized * 25;
        }
        else if(rb.velocity != Vector2.zero)
        {
            StartCoroutine(SwitchCollider());
            rb.velocity = Vector2.zero;
            audioSource.PlayOneShot(landClip);
        }

        myCircle.DrawCircle(steps, radius * curDist/dist);
        landingCircle.transform.rotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y,
            270 * curDist / dist);
        landingCircle.transform.position = landPos;

    }

    IEnumerator SwitchCollider()
    {
        coll.enabled = true;
        coll.isTrigger = true;

        yield return new WaitForSeconds(0.2f);
        coll.isTrigger = false;
    }

}
