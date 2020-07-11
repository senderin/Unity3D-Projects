using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public GameObject wall;

    public GameObject blockPrefab;
    public GameObject coinPrefab;
    public GameObject bonusBlocks;

    public float minLength = 20;
    public float maxLength = 50;

    private List<GameObject> blockPool;

    public void Awake()
    {
        blockPool = new List<GameObject>();
    }

    public void ReplaceBlocks(int level) {
        GameObject blockParent = GameObject.Find("BlockParent");
        if (blockParent == null)
        {
            blockParent = new GameObject();
            blockParent.name = "BlockParent";
        }

        // determines a random number of blocks
        int numOfBlocks = Random.Range(5 * level, 10 * level);
        // available blocks in the pool
        int numOfAvailableBlocks = blockPool.Count;
        int numToCreate = 0, numToMakeInvisible = 0;
        // how many blocks will make invisible
        if (numOfBlocks < blockPool.Count)
            numToMakeInvisible = numOfAvailableBlocks - numOfBlocks;
        // how many blocks will create
        else
            numToCreate = numOfBlocks - blockPool.Count;

        // the lowest point where a block can be placed
        Vector3 nextPos = new Vector3(0, 4f, 0);

        if(numOfAvailableBlocks > 0) {
            // uses the blocks from the pool
            for (int i = 0; i < numOfAvailableBlocks; i++)
            {
                if (nextPos.y > wall.transform.localScale.y)
                    break;
                nextPos = new Vector3(0, Random.Range(nextPos.y + 3, nextPos.y + 1f), 0);
                blockPool[i].transform.position = nextPos;
                blockPool[i].transform.SetParent(blockParent.transform);
                blockPool[i].SetActive(true);
            }

            if (numToMakeInvisible > 0)
            {
                // makes other blocks invisible
                for (int i = (numOfBlocks + 1); i < blockPool.Count; i++)
                {
                    blockPool[i].SetActive(false);
                }
            }
        }

        if (numToCreate > 0)
        {
            // creates blocks and adds to pool to reuse later 
            for (int i = 0; i < numToCreate; i++)
            {
                if (nextPos.y > wall.transform.localScale.y)
                    break;
                GameObject newBlock = Instantiate(blockPrefab, nextPos, Quaternion.identity);
                // there is 1 unit distance between the consecutive blocks
                nextPos = new Vector3(0, Random.Range(nextPos.y + 1, nextPos.y + 2f), 0);
                blockPool.Add(newBlock);
                newBlock.transform.SetParent(blockParent.transform);
            }
         }
    }

    public void InitializeWall(int level) {
        Vector3 initialScale = wall.transform.localScale;
        // increases by 10 in each level (starting from minLength) 
        float newLength = minLength + (10 * (level - 1));
        // limits the length of the wall
        if (newLength > maxLength)
            newLength = maxLength;

        wall.transform.localScale = new Vector3(initialScale.x, newLength, initialScale.z);

        Vector3 wallPosition = wall.transform.position;
        wall.transform.position = new Vector3(wallPosition.x, newLength/2, wall.transform.position.z);

        // assigns random color
        // wall.GetComponent<Renderer>().material.color = GetColor();
    }

    internal void GenerateLevel(int level)
    {
        // in each level, wall, blocks and bonus blocks are rearranged.
        InitializeWall(level);
        ReplaceBonusBlocks();
        ReplaceBlocks(level);

        // destroys coins from previous level
        RemoveCoins();
        // 75% chance to create coins
        if (Random.Range(1, 10) <= 8)
            CreateCoins(level);
    }

    private void RemoveCoins()
    {
        GameObject coinsParent = GameObject.Find("CoinParent");
        if (coinsParent == null)
            return;

        foreach (Transform child in coinsParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateCoins(int level)
    {
        int numofCoins = 10;
        Vector3 ballPos = FindObjectOfType<Ball>().transform.position;

        GameObject coinsParent = GameObject.Find("CoinParent");
        if (coinsParent == null) { 
            coinsParent = new GameObject();
            coinsParent.name = "CoinParent";
        }

        // the lowest point where a coin can be placed
        Vector3 nextPos = new Vector3(0, Random.Range(10f, wall.transform.localScale.y), ballPos.z);
        for (int i = 0; i < numofCoins; i++)
        {
            if (nextPos.y > wall.transform.localScale.y)
                break;

            GameObject newCoin = Instantiate(coinPrefab, nextPos, Quaternion.identity);
            newCoin.transform.localRotation = Quaternion.Euler(90, 0, 0);
            int oscillationCoef = i % 10;
            newCoin.GetComponent<CoinMove>().oscillationCoef = oscillationCoef;
            newCoin.transform.SetParent(coinsParent.transform);

            nextPos = new Vector3((nextPos.x + 0.1f), (nextPos.y + 0.5f), ballPos.z);
        }
    }

    private void ReplaceBonusBlocks()
    {
        Vector3 wallPos = wall.transform.position;
        float finishLineOffset = 1.5f;
        // calculates the top of the wall to place the bonus blocks
        Vector3 position = new Vector3(wallPos.x, (wallPos.y + wall.transform.localScale.y/2 + finishLineOffset), wallPos.z);
        bonusBlocks.transform.position = position;
    }

    public Color GetColor() {
        return Random.ColorHSV();
    }

}
