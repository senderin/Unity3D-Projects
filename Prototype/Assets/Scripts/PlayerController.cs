using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : Competitor {
	
	public float speed;
	public Joystick joystick;
	public float rotationSpeed;
    public Vector2 clampValues;

    private Quaternion initialRotation;
    private float previousHorizontalPoint = 0f;
    private float previousVerticalPoint = 0f;

    public override void Start()
    {
        base.Start();

        isPlayer = true;
        initialRotation  = Quaternion.Euler(0, 0f, 0);
    }

	void Update()
	{
        if (!isRunning)
            return;

        float currentHorizontalPoint = joystick.Horizontal;
        float currentVerticalPoint = joystick.Vertical;

        Vector3 touchMagnitude = new Vector3(joystick.Horizontal, 0, 0);
        Vector3 touchPosition = transform.position + touchMagnitude.normalized;
        Vector3 touchDirection = touchPosition - transform.position;

        float angle = Mathf.Atan2(-touchDirection.y, touchDirection.x) * Mathf.Rad2Deg;
        if(touchDirection.x > 0)
            angle -= 90;
        if (touchDirection.x < 0)
        {
            angle -= 90;
            angle *= -1;

        }
        angle = ClampAngle(angle, -30, 30);

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.down);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Mathf.Min(Time.deltaTime, .04f)); 
    }

    public void FixedUpdate()
    {
        if (!isRunning)
            return;

        Vector3 movementVector = transform.position + transform.forward * speed * Time.fixedDeltaTime;
        movementVector.x = Mathf.Clamp(movementVector.x, clampValues.x, clampValues.y);
        rb.MovePosition(movementVector);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (gameManager == null || gameManager.gameState != GameState.RACE_STAGE)
            return;

        if (collision.gameObject.CompareTag("obstacle"))
        {
            base.CollideWithObstacle();
        }

        if (collision.gameObject.CompareTag("finishLine"))
        {
            base.ReachFinishLine();
        }

        if (collision.gameObject.CompareTag("rotator"))
        {
            Vector3 contactPoint = collision.contacts[0].point;
            // Throw(contactPoint);
            /* if (!isPlayer)
            {
                rb.isKinematic = false;
                GetComponent<NavMeshAgent>().enabled = false;
            }

            Vector3 dir = collision.contacts[0].point - transform.position;
            dir = dir.normalized;
            rb.AddForce(- dir * force, ForceMode.Impulse);

            if (!isPlayer && rb.velocity.magnitude <= 0.1f)
            {
                rb.isKinematic = true;
                GetComponent<NavMeshAgent>().enabled = true;
            } */
        }
    }

    public void ResetPlayerTransformation()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    public override void Throw(Vector3 contactPoint)
    {
        Vector3 dir = contactPoint - transform.position;
        dir = dir.normalized;
        rb.AddForce(-dir * 100, ForceMode.Impulse);
    }
}