using UnityEngine;
using System.Collections;

public class CollectionZoneController : MonoBehaviour
{
    public float gravitationalForce;
    public Color collectedCubeColor;

    private int numOfCollectedCubes;
    private GameManager gameManager;

    public void Start()
    {
        numOfCollectedCubes = 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            // change color of the cube
            other.gameObject.GetComponent<Renderer>().material.color = collectedCubeColor;

            // apply gravitational force
            Vector3 directionOfCube = (transform.position - other.gameObject.transform.position).normalized;
            rb.AddForce(directionOfCube * gravitationalForce);

            // deactivate rigidbody and collider of the cube after 0.4 seconds
            StartCoroutine(DeactivateCube(other.gameObject));

            numOfCollectedCubes++;

            // notify to game manager
            gameManager.UpdateProgress(numOfCollectedCubes);
        }
    }

    public void ResetCollectedCubes()
    {
        //collectedCubes.Clear();
        numOfCollectedCubes = 0;
    }

    IEnumerator DeactivateCube(GameObject cube)
    {
        yield return new WaitForSeconds(0.4f);
        cube.GetComponent<BoxCollider>().enabled = false;
        cube.GetComponent<Rigidbody>().detectCollisions = false;
    }
}
