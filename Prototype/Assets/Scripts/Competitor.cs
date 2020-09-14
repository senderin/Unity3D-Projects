using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public abstract class Competitor : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public bool isWinner = false;
    [HideInInspector]
    public Vector3 initialPosition;
    [HideInInspector]
    public bool isRunning;
    [HideInInspector]
    public bool isPlayer = false;
    [HideInInspector]
    public GameManager gameManager;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
        animator.SetTrigger("idle");

        initialPosition = transform.position;
        gameManager = FindObjectOfType<GameManager>();
    }

    public virtual void StartRunning()
    {
        isRunning = true;
        animator.SetTrigger("run");
    }

    public virtual void StopRunning()
    {
        isRunning = false;
        if (isWinner)
            animator.SetTrigger("win");
        else
            animator.SetTrigger("lose");
    }

    public void CollideWithObstacle()
    {
        animator.SetTrigger("fall");
        isRunning = false;
        StartCoroutine("ReturnStartPoint");
    }

    public void ReachFinishLine()
    {
        isWinner = true;
        StopRunning();
        gameManager.FinishRaceStage(this.gameObject);
    }


    public virtual IEnumerator ReturnStartPoint()
    {
        yield return new WaitForSeconds(1f);
        transform.position = initialPosition;
        StartRunning();
    }

    public abstract void Throw(Vector3 contactPoint);
}
