using System;
using System.Collections.Generic;
using Es.InkPainter;
using UnityEngine;

public class CustomPainter : MonoBehaviour
{
	[SerializeField]
	private Brush brush;

    private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
	{
		if(Input.GetMouseButton(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			bool success = true;
			RaycastHit hitInfo;
			if(Physics.Raycast(ray, out hitInfo))
			{
				InkCanvas paintObject = hitInfo.transform.GetComponent<InkCanvas>();

                if (paintObject != null)
                    success = paintObject.Paint(brush, hitInfo);
                else
                    Debug.LogError("Failed to paint.");
			}
		}
	}
}
