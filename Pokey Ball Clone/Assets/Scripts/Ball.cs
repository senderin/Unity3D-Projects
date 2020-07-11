using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform hole;
    public Line lineGenerator;
    public float maxDragDistance = 1f;
    public float minDragDistance = 0.1f;
    public float draggedDistance = 0;
    public bool isReleased = false;
    public bool isLevelCompleted = false;

    private Rigidbody rb;
    private Vector3 ballInitialPosition;
    private Vector3 holeInitialPosition;
    private bool isPinned;
    private Vector3 offset;
    private Vector3 screenPoint;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lineGenerator = FindObjectOfType<Line>();

        ballInitialPosition = transform.position;
        holeInitialPosition = hole.position;
    }

    public void Update()
    {
        if (isLevelCompleted)
            return;

        // the score is updated according to the y coordinate of the ball
        GameManager.Instance.UpdateScore(transform.position.y);
    }

    void OnMouseDrag()
    {
        Drag();
    }

    void OnMouseDown()
    {
        Pin();
    }

    void OnMouseUp()
    {
        Release();
    }

    private void Drag()
    {
        if (!isPinned || isLevelCompleted)
            return;

        isReleased = false;
        lineGenerator.SetDrawLine(true);

        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;

        // ball only drags to down
        if (cursorPosition.y < transform.position.y)
        {
            // limits the draggable length
            if (draggedDistance <= maxDragDistance)
            {
                draggedDistance += Mathf.Abs(transform.position.y - cursorPosition.y);
                // ball follows the input
                transform.position = new Vector3(transform.position.x, cursorPosition.y, transform.position.z);
            }
        }
    }

    private void Pin()
    {
        if (isLevelCompleted)
            return;

        GameObject objectToPin = FindAt(transform.position);

        // If it coincides with the block, cannot be pinned
        if (objectToPin != null && objectToPin.CompareTag("CannotPin"))
            return;

        if (objectToPin != null)
            Debug.Log(objectToPin.tag);

        // If it is pinned to a bonus block, updates the number of the blocks
        if (objectToPin != null && ( objectToPin.CompareTag("bonus_2") || objectToPin.CompareTag("bonus_4") || objectToPin.CompareTag("bonus_8")))
        {
            GameManager.Instance.UpdateNumCoins(objectToPin.tag);
            isLevelCompleted = true;
            GameManager.Instance.NotifyLevelCompleted();
        }

        hole.position = new Vector3(hole.position.x, transform.position.y, hole.position.z);
        lineGenerator.SetDrawLine(true);

        // returns the input to the game world position
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        rb.isKinematic = true;
        isPinned = true;
    }

    private void Release()
    {
        if (!isPinned)
            return;

        // the ball stays as pinned in minor input changes
        if (draggedDistance < minDragDistance)
        {
            draggedDistance = 0;
        }

        else
        {
            lineGenerator.SetDrawLine(false);
            rb.isKinematic = false;
            isReleased = true;
            isPinned = false;

            // dragged distance has an effect on velocity
            rb.velocity = new Vector3(0, 5 + draggedDistance * 10, 0);
            draggedDistance = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if hits the ground, the level restarts in 1 second
        if (collision.gameObject.CompareTag("ground"))
        {
            StartCoroutine("Restart");
        }
    }

    IEnumerator Restart() {
        yield return new WaitForSeconds(1f);
        InitializePositions();
        isPinned = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("coin")) {
            Destroy(other.gameObject);
            GameManager.Instance.UpdateNumCoins("bonus_1");
        }
    }

    public void InitializePositions()
    {
        transform.position = ballInitialPosition;
        hole.position = holeInitialPosition;
        rb.isKinematic = true;
        GetComponent<MeshRenderer>().enabled = true;
        lineGenerator.SetDrawLine(true);
        Camera.main.GetComponent<CameraFollow>().Reset();
    }

    public GameObject FindAt(Vector3 pos) 
    {
        // returns the game object at the certain position if there is
        if (Physics.Raycast(pos, transform.forward, out RaycastHit hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }
        return null;
    }
}
