using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 1;
    public LevelGenerator levelGenerator;
    public PlayerController player;
    private int numOfLevels;

    private UIManager uiManager;
    private CollectionZoneController collectionZone;
    private int numOfCubes;
    private bool isLevelStart;

    void Awake()
    {
        PlayerPrefs.DeleteAll();
        // if it is first launch of the game
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        }

        else
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
    }

    public void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        collectionZone = GameObject.Find("CollectionZone").GetComponent<CollectionZoneController>();

        numOfLevels = levelGenerator.mapTextures.Count;
        isLevelStart = false;

        InitializeLevel();
    }

    public void Update()
    {
        // check player position to detect whether level starts or not
        if(!isLevelStart && player.gameObject.transform.position != player.initialPosition)
        {
            isLevelStart = true;
            uiManager.HideStartInstruction();
        }
    }

    // initialize the level parameters
    private void InitializeLevel()
    {
        // generate the level pattern
        levelGenerator.LoadLevel(currentLevel);

        // get number of cubes that will collect and reset progress bar
        numOfCubes = levelGenerator.GetNumOfCubes();
        uiManager.SetProgressBar(numOfCubes, currentLevel);

        uiManager.ShowStartInstruction();
    }

    void MoveNextLevel()
    {
        if(currentLevel + 1 <= numOfLevels)
        {
            // increase level and save
            currentLevel++;
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            // destroy collected cubes
            collectionZone.ResetCollectedCubes();
            // update level parameters
            InitializeLevel();
        }
    }

    public void UpdateProgress(int numOfTotalCollectedCubes)
    {
        if(numOfTotalCollectedCubes == numOfCubes)
        {
            StartCoroutine(LevelOver());
        }
        else
        {
            uiManager.UpdateProgressBar();
        }
    }

    IEnumerator LevelOver()
    {
        uiManager.SetLevelCompletedText(currentLevel);
        yield return new WaitForSeconds(1.5f);
        uiManager.HideLevelCompletedText();
        MoveNextLevel();
        player.ResetPlayerTransformation();
        isLevelStart = false;
    }
}
