using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    public List<Texture2D> mapTextures;
    public GameObject cubePrefab;
    public GameObject levelPattern;
    public GameObject patternCenter;

    private int numOfUsedObjects;
    private int numOfAvailableObjects;
    private int numOfCubeInPattern;

    // Call GenerateLevel function by passing texture parameter corresponding level
    public void LoadLevel(int currentLevel)
    {
        string textureName = "level" + currentLevel.ToString();
        Texture2D result = mapTextures.Find(x => x.name == textureName);
        if (result != null)
        {
            GenerateLevel(result);
        }
        else
        {
            Debug.LogError("Not found texture parameter corresponding level: " + currentLevel.ToString());
        }

    }

    private void GenerateLevel (Texture2D mapTexture)
    {
        ResetLevelPattern();

        numOfUsedObjects = 0;
        numOfCubeInPattern = 0;

        // store number of the available objects at the beginning of the level generation
        numOfAvailableObjects = levelPattern.transform.childCount;
    
        for (int i = 0; i < mapTexture.width; i++)
        {
            for (int j = 0; j < mapTexture.height; j++)
            {
                Color pixelColor = mapTexture.GetPixel(i, j);
                GenerateObject(i, j, pixelColor);
                numOfCubeInPattern++;
            }
        }

        // if there are more objects than needed, then remove them
        if(numOfUsedObjects != 0 && levelPattern.transform.childCount > numOfCubeInPattern)
        {
            for (int i = numOfUsedObjects; i < levelPattern.transform.childCount; i++)
            {
                Destroy(levelPattern.transform.GetChild(numOfUsedObjects).gameObject);
            }
        }

        ShrinkLevelPattern();
    }

    private void GenerateObject(float x, float y, Color pixelColor)
    {
        // Nothing to create in transparent pixel
        if (pixelColor.a == 0)
        {
            return;
        }

        GameObject cube;
        Vector2 position = new Vector2(x, y);

        // when there is no cube objects at the beginning and when needed more cubes, instantiate new one
        if (numOfAvailableObjects == 0 || (numOfAvailableObjects == numOfUsedObjects))
        {
            cube = Instantiate(cubePrefab, position, Quaternion.identity, transform);
            cube.GetComponent<Renderer>().material.color = new Color(pixelColor.r, pixelColor.g, pixelColor.b, pixelColor.a);
            cube.transform.SetParent(levelPattern.transform);
        }

        // use available ones
        else
        {
            cube = levelPattern.transform.GetChild(numOfUsedObjects).gameObject;
            numOfUsedObjects++;
            cube.transform.position = position;
            cube.GetComponent<Renderer>().material.color = new Color(pixelColor.r, pixelColor.g, pixelColor.b, pixelColor.a);

            // activate rigidbody and collider
            cube.GetComponent<BoxCollider>().enabled = true;
            cube.GetComponent<Rigidbody>().detectCollisions = true;
        }
    }

    private void ResetLevelPattern()
    {
        levelPattern.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        levelPattern.transform.localScale = new Vector3(1f, 1f, 1f);
        levelPattern.transform.position = Vector3.zero;
    }

    private void ShrinkLevelPattern()
    {
        levelPattern.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        // shrink the pattern
        levelPattern.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        // place the pattern to center
        levelPattern.transform.position = patternCenter.transform.position;
    }

    internal int GetNumOfCubes()
    {
        return levelPattern.transform.childCount;
    }
}
