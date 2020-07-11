using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public int level = 1;
    public int bestScore;
    public int numCoins;

    private int lastScore;
    private UIController uiController;
    private LevelGenerator levelGenerator;
    private BackgroundController backgroundController;
    private Ball ball;

    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;

        levelGenerator = FindObjectOfType<LevelGenerator>();
        uiController =  FindObjectOfType<UIController>();
        backgroundController =  FindObjectOfType<BackgroundController>();
        ball = FindObjectOfType<Ball>();

        LoadProgress();
        LoadLevel();
    }

    public void SaveProgress() {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("bestScore", bestScore);
        PlayerPrefs.SetInt("numCoins", numCoins);
        PlayerPrefs.Save();
    }

    public void LoadProgress() {
        level = GetKeyValue("level");
        bestScore = GetKeyValue("bestScore");
        numCoins = GetKeyValue("numCoins");
    }

    private int GetKeyValue(string key) {
        int value = 0;
        if (PlayerPrefs.HasKey(key))
            value = PlayerPrefs.GetInt(key);
        return value;
    }

    public void LoadLevel() {
        levelGenerator.GenerateLevel(level);
        uiController.LoadNewLevelUI(level);
        backgroundController.SetBackground(level);
    }

    public IEnumerator LoadNextLevel() {
        yield return new WaitForSeconds(1f);
        ball.InitializePositions();
        ball.isLevelCompleted = false;
        level++;
        LoadLevel();
        SaveProgress();
    }

    public void UpdateScore(float posY) {
        int score = (int)posY * 10;
        if (score == lastScore)
            return;

        uiController.UpdateScore(score);
        uiController.UpdateLevelBar((int)posY);

        lastScore = score;
        if (lastScore > bestScore) { 
            bestScore = lastScore;
            SaveProgress();
        }
    }

    internal void UpdateNumCoins(string tag)
    {
        char[] seperator = { '_' };
        numCoins += int.Parse(tag.Split(seperator)[1]);
        uiController.UpdateNumCoins();
        SaveProgress();
    }

    internal void NotifyLevelCompleted()
    {
        uiController.ShowLevelCompletedEffect();
        StartCoroutine(GameManager.Instance.LoadNextLevel());
    }
}
