using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : Competitor
{
    public Transform finishPoint;
    public float speed;

    private float speedMultiplier;
    private NavMeshAgent navMesh;

    public override void Start()
    {
        base.Start();

        navMesh = GetComponent<NavMeshAgent>();
        speedMultiplier = Random.Range(1f, 1.5f);
        animator.SetFloat("speedMultiplier", speedMultiplier);
        navMesh.speed = Random.Range(5, 7);
        navMesh.angularSpeed = Random.Range(100, 120);
    }

    public override void StartRunning()
    {
        base.StartRunning();
        navMesh.SetDestination(finishPoint.position);
        navMesh.isStopped = false;
    }

    public override void StopRunning()
    {
        base.StopRunning();
        navMesh.isStopped = true;
    }

    public override IEnumerator ReturnStartPoint()
    {
        navMesh.isStopped = true;
        return base.ReturnStartPoint();
    }

    public override void Throw(Vector3 contactPoint)
    {
        rb.isKinematic = false;
        navMesh.enabled = false;

        Vector3 dir = contactPoint - transform.position;
        dir = dir.normalized;
        rb.AddForce(-dir * 100, ForceMode.Impulse);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (gameManager == null || gameManager.gameState != GameState.RACE_STAGE)
            return;

        if (other.gameObject.CompareTag("obstacle"))
        {
            base.CollideWithObstacle();
        }

        if (other.gameObject.CompareTag("finishLine"))
        {
            base.ReachFinishLine();
        }

        if (other.gameObject.CompareTag("rotator"))
        {

        }
    } 
}
