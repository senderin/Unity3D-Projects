using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Operation { 
    DRAG, ROTATE, NONE
}

public class InputController : MonoBehaviour
{
    [Range(1f, 30f)]
    public float speed = 10f;
    public float rotationSpeed = 50f;
    [Range(1f, 20f)]
    public float rotationSensitivity = 10f;
    public float minFOV = 1f;
    public float maxFOV = 179f;
    [Range(1f, 20f)]
    public float sensitivity = 10f;

    public GameObject nodeObject;

    private bool isDrag;
    private bool isRotate;
    private Vector3 screenSpace;
    private Vector3 offset;

    void Update()
    {
        MoveXY();
        ZoomInOut();

        // left mouse click --> rotate around clicked node
        if (Input.GetMouseButtonDown(0))
        {
            nodeObject = FindAt(Input.mousePosition);
            if (nodeObject != null)
                isRotate = true;
            else
                isRotate = false;
        }

        if (isRotate)
            Rotate();

        // right mouse click --> drag a node
        if (Input.GetMouseButtonDown(1))
        {
            isRotate = false;
            nodeObject = FindAt(Input.mousePosition);
            if (nodeObject != null)
            {
                isDrag = true;
                screenSpace = Camera.main.WorldToScreenPoint(nodeObject.transform.position);
                offset = nodeObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
            }
            else
                isDrag = false;
        }

        if (Input.GetMouseButtonUp(1) && nodeObject != null)
        {
            isDrag = false;
            // ????
            // FindObjectOfType<GraphController>().UpdateGraph();
        }

        if (isDrag)
        {
            //keep track of the mouse position
            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

            //convert the screen mouse position to world point and adjust with offset
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            //update the position of the object in the world
            FindObjectOfType<GraphController>().UpdateVertexPosition(nodeObject, curPosition);
        }
    }

    private void Rotate()
    {
        transform.RotateAround(nodeObject.transform.position,
                                        transform.up,
                                        -Input.GetAxis("Mouse X") * rotationSpeed * rotationSensitivity * Time.deltaTime);

        transform.RotateAround(nodeObject.transform.position,
                                        transform.right,
                                        -Input.GetAxis("Mouse Y") * rotationSpeed * rotationSensitivity * Time.deltaTime);
    }

    private void ZoomInOut()
    {
        /* float fov = Camera.main.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * rotationSensitivity;
        fov = Mathf.Clamp(fov, minFOV, maxFOV);
        Camera.main.fieldOfView = fov;  */
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Input.GetAxis("Mouse ScrollWheel") * rotationSensitivity);
    }

    private void MoveXY()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, vertical, 0);
        transform.Translate(direction.normalized * Time.deltaTime * speed);
    }

    public GameObject FindAt(Vector3 pos)
    {
        // returns the game object at the certain position if there is
        var ray = Camera.main.ScreenPointToRay(pos);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            return hit.transform.gameObject;
        }

        return null;
    }
}
