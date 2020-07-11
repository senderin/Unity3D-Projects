using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float speed;
	public Joystick joystick;
	public float rotationSpeed;

    [HideInInspector]
    public Vector3 initialPosition;

    private Rigidbody rb;
    private Quaternion initialRotation;

    private float previousHorizontalPoint = 0;
    private float previousVerticalPoint = 0;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
        initialPosition = new Vector3(0, 0, -11f);
        initialRotation = Quaternion.Euler(0, 180f, 0);
	}

	void Update()
	{

		if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetKey("escape")) 
		{
			Application.Quit();
		}
		else if(SystemInfo.deviceType==DeviceType.Handheld && Input.GetKeyDown (KeyCode.Escape))
		{
			Application.Quit ();
		}

        float currentHorizontalPoint = joystick.Horizontal;
        float currentVerticalPoint = joystick.Vertical;

        if (previousVerticalPoint == currentVerticalPoint && previousHorizontalPoint == currentHorizontalPoint)
            return;

        Vector3 moveVector = (-transform.right * joystick.Horizontal + transform.forward * joystick.Vertical).normalized;
		transform.Translate (moveVector * speed * Time.deltaTime);

        Vector3 touchMagnitude = new Vector3(joystick.Horizontal, joystick.Vertical, 0);
		Vector3 touchPosition = transform.position + touchMagnitude.normalized;
		Vector3 touchDirection = touchPosition - transform.position;
		float angle = Mathf.Atan2(-touchDirection.y, touchDirection.x) * Mathf.Rad2Deg;
		angle -= 90;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Mathf.Min(Time.fixedDeltaTime, .04f));
    }

    public void ResetPlayerTransformation()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

}