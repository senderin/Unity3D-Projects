using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Transform ball;
    public Transform hole;

    private bool drawLine;
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        drawLine = true;
    }

    void Update()
    {
        if (drawLine)
        {
            line.enabled = true;
            DrawCurvedLine(hole.position, new Vector3(hole.position.x, hole.position.y, ball.position.z),  ball.position);
        }
        else {
            line.enabled = false;
        }
    }

    void DrawCurvedLine(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        line.positionCount = 20;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < line.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            line.SetPosition(i, B);
            t += (1 / (float)line.positionCount);
        }
    }


    public void SetDrawLine(bool state) {
        drawLine = state;
    }
}
