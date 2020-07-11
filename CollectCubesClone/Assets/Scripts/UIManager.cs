using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    public RectTransform progressBar;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI dragToStartText;
    public TextMeshProUGUI levelCompletedText;

    private float barLenght;
    private float unitPerCube;
    private bool showDragToStartText;

    public void Start()
    {
        barLenght = progressBar.rect.width;
    }

    internal void UpdateProgressBar()
    {
        progressBar.offsetMin += new Vector2(unitPerCube, 0);  
    }

    internal void SetProgressBar(int totalCubeNumber, int currentLevel)
    {
        // reset progress
        progressBar.offsetMin = new Vector2(0, 0);
        // calculate the progress unit for one collected cube
        unitPerCube = barLenght / totalCubeNumber;

        currentLevelText.SetText(currentLevel.ToString());
        nextLevelText.SetText((currentLevel + 1).ToString());
    }

    internal void ShowStartInstruction()
    {
        dragToStartText.alpha = 1.0f;
        StartCoroutine(FadeInOutText(true));
    }

    internal void HideStartInstruction()
    {
        dragToStartText.alpha = 0.0f;
        StartCoroutine(FadeInOutText(false));
    }

    IEnumerator FadeInOutText(bool show)
    {
        while(show)
        {
            // fade out
            dragToStartText.CrossFadeAlpha(0.0f, 0.5f, false);
            yield return new WaitForSeconds(0.5f);
            // fade in
            dragToStartText.CrossFadeAlpha(1.0f, 0.5f, false);
            yield return new WaitForSeconds(0.5f);
            // fade out
            dragToStartText.CrossFadeAlpha(0.0f, 0.5f, false);
        }
    }

    public void SetLevelCompletedText(int completedLevel)
    {
        levelCompletedText.SetText("LEVEL " + completedLevel.ToString() + "\nCOMPLETED");
    }

    internal void HideLevelCompletedText()
    {
        levelCompletedText.SetText("");
    }
}
