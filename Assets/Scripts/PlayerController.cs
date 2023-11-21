using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController Instance
    { get { return instance; } }

    [SerializeField]
    private int maxHp;
    private int currentHp;
    [SerializeField]
    private GameObject[] hearts;

    private bool canMove;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject head;

    [SerializeField]
    private GameObject body;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip deathClip;

    private Vector2 RawInputs;
    void Awake()
    {
        instance = this;
    }


    void Update()
    {
        if(canMove)
        {
            ManageRawInputs();
        }
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < currentHp)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
       rb.velocity = RawInputs * moveSpeed;

        head.transform.up = MouseDir().normalized;
        if(RawInputs != Vector2.zero)
        {
            body.transform.up = RawInputs;
        }
    }

    public void NewGame()
    {
        canMove = true;
        currentHp = maxHp;
        NewCycle();
    }

    public void DealDamage()
    {
        currentHp--;
        Debug.Log("Current Hp = " + currentHp);

        if(currentHp <= 0) 
        {
            canMove = false;
            audioSource.PlayOneShot(deathClip);
             GameManager.Instance.EndGame();
        }
    }

    public void NewCycle()
    {
        transform.position = Vector2.zero;
    }

    private void ManageRawInputs()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        RawInputs = new Vector3 (x, y, 0); 
        RawInputs.Normalize(); 
    }

    private Vector2 MousePos()
    {
        Vector2 toReturn = cam.ScreenToWorldPoint(Input.mousePosition);

        return toReturn;
    }

    private Vector2 MouseDir()
    {
        Vector2 toReturn = MousePos() - (Vector2)transform.position;

        return toReturn;
    }
}
