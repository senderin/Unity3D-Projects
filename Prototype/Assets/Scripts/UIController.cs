using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject hudPanel;
    public GameObject startPanel;
    public TextMeshProUGUI rankText;
    public TextMeshPro gameEndText;
    public GameObject restartButton;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        hudPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void SetGamePlayUI()
    {
        hudPanel.SetActive(true);
        startPanel.SetActive(false);
    }

    internal void UpdateRankText(int rank)
    {
        rankText.SetText(rank.ToString());
    }

    public void SetGameEndText(bool playerWon)
    {
        if (playerWon)
            gameEndText.SetText("Victory!");
        else
            gameEndText.SetText("Defeat!");

        restartButton.SetActive(true);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }
}
