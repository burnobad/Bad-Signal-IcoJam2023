using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [SerializeField]
    private LineRenderer circleRenderer;

    public void DrawCircle(int _steps, float _radius)
    {
        circleRenderer.positionCount = _steps;

        for (int s = 0; s < _steps; s++)
        {
            float circumProg = (float)s / _steps;

            float curRad = circumProg * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(curRad);
            float yScaled = Mathf.Sin(curRad);

            float x = xScaled * _radius;
            float y = yScaled * _radius;    

            Vector3 curPos = new Vector3 (x, y, 0);

            circleRenderer.SetPosition(s, curPos);
        }

    }
}
