using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CustomRange
{
    public float min;
    public float max;
}

public class MoveHorizantal : MonoBehaviour
{
    public CustomRange speedRange;
    public Transform movingObject;
    public Vector2 clampValues;
    public bool isMovementLimit;

    private float speed;
    private Vector3 startPos;
    private Vector3 newPos;
    private Vector3 direction;
    private RangeAttribute movableRange;

    public void Start()
    {
        startPos = movingObject.position;

        if (transform.eulerAngles.y > 0)
            direction = Vector3.right;
        else
            direction = Vector3.left;

        speed = Random.Range(speedRange.min, speedRange.max);
        if(isMovementLimit)
            movableRange = new RangeAttribute(clampValues.x + startPos.x, clampValues.y + startPos.x);
        else
            movableRange = new RangeAttribute(clampValues.x, clampValues.y);
    }

    public void Update()
    {
        newPos = movingObject.position + direction * speed * Time.deltaTime;
        newPos.x = Mathf.Clamp(newPos.x, movableRange.min, movableRange.max);
        movingObject.position = newPos;

        if (newPos.x >= movableRange.max || newPos.x <= movableRange.min)
            direction *= -1;
    }

}
