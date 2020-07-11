using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelClearText;
    public TextMeshProUGUI numCoinsText;
    public GameObject levelCompletedText;
    public RectTransform levelBar;
    public TextMeshProUGUI bestScoreText;
    public GameObject confettiEffectPrefab;

    private float barLenght;
    private float unitPerDistance;

    public void Awake()
    {
        barLenght = levelBar.rect.width;
    }

    public void UpdateScore(int score) {
        scoreText.SetText(score.ToString());
    }

    public void UpdateLevelTexts(int currentLevel) {
        currentLevelText.SetText(currentLevel.ToString());
        nextLevelText.SetText((currentLevel + 1).ToString());
    }

    internal void LoadNewLevelUI(int level)
    {
        UpdateScore(0);
        UpdateNumCoins();
        UpdateLevelTexts(level);
        SetLevelBar();
        UpdateLevelBar(0);
        ShowBestScore();
    }

    public void UpdateNumCoins()
    {
        numCoinsText.SetText(GameManager.Instance.numCoins.ToString());
    }

    private void ShowBestScore()
    {
        bestScoreText.SetText("best score " + GameManager.Instance.bestScore.ToString());
        Invoke("HideBestScoreText", 2f);
    }

    private void HideBestScoreText() { 
        bestScoreText.SetText("");
    }

    internal void SetLevelBar()
    {
        int wallLength = (int)GameObject.Find("Wall").transform.localScale.y;
        // reset progress
        levelBar.offsetMin = new Vector2(0, 0);
        // calculate the progress unit for the distance
        unitPerDistance = barLenght / wallLength;
    }

    internal void UpdateLevelBar(int posY)
    {
        if (unitPerDistance <= 0)
            SetLevelBar();
        levelBar.offsetMin = new Vector2(unitPerDistance * posY, 0);
    }

    internal void ShowLevelCompletedEffect()
    {
        Vector3 pos = new Vector3(0, Camera.main.transform.position.y, -2);
        GameObject effect = Instantiate(confettiEffectPrefab, pos, Quaternion.identity);
        Destroy(effect, 1.5f);
        levelCompletedText.SetActive(true);
        Invoke("HideLevelCompletedText", 1f);
    }

    internal void HideLevelCompletedText()
    {
        levelCompletedText.SetActive(false);
    }
}
