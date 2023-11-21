using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameTextManager : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private Vector2 upPos;
    private Vector2 downPos;

    private string firstText;
    private string secondtText;

    private bool goingUp;
    void Awake()
    {
        goingUp = true;
        firstText = "Start";
        secondtText = "Game";
        upPos = new Vector2 (0, -1);
        downPos = new Vector2(0, -6);

        text.text = secondtText;
        transform.localPosition = upPos;
    }

    void FixedUpdate()
    {

        if (transform.localPosition.y +.01f >= upPos.y)
        {
            goingUp = false;

            if(text.text == firstText) 
            {
                text.text = secondtText;
            }
            else
            {
                text.text = firstText;
            }
        }
        else if (transform.localPosition.y - .01f <= downPos.y)
        {
            goingUp = true;
        }

        if (!goingUp)
        {
            MoveTo(downPos);
        }
        else if(goingUp)
        {
            MoveTo(upPos);  
        }

    }

    void MoveTo(Vector2 _pos)
    {
        Vector3 dir = _pos - (Vector2)transform.localPosition;
        dir.Normalize();

        transform.localPosition += dir / 4;
    }
}
