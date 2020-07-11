using Imphenzia;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Ball ball;
    public Transform hole;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - ball.transform.position;
    }

    void LateUpdate()
    {
        if(ball.isReleased && (ball.transform.position.y >= hole.position.y))
            transform.position = ball.transform.position + offset;
    }

    public void Reset() {
        transform.position = ball.transform.position;
    }
}
