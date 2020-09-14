using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Transform target;

    private Vector3 offset;
    private float newZPos;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        offset = target.position - transform.position;
    }

    void Update()
    {
        if (gameManager.gameState == GameState.RACE_STAGE)
        {
            newZPos = (target.position - offset).z;
            transform.position = new Vector3(transform.position.x, transform.position.y, newZPos);
        }

    }

    public void FocusOnWinner(Transform winner)
    {
        target = winner;
        Vector3 moveTo = target.position - offset;
        moveTo.x = 0;
        transform.DOMove(moveTo, 2f);
    }

    
}
