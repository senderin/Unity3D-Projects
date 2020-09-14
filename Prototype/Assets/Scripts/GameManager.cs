using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState
{
    START_UI,
    RACE_STAGE,
    WALL_PAINTING_STAGE
}

public class GameManager : MonoBehaviour
{
    public List<Competitor> competitors;
    public List<Transform> spawnPositions;
    public GameState gameState { get; private set; }

    private CameraController cameraController;
    private CustomPainter customPainter;
    private UIController uiController;

    private void Awake()
    {
        gameState = GameState.START_UI;
        uiController = GetComponent<UIController>();
        cameraController = FindObjectOfType<CameraController>();
        customPainter = FindObjectOfType<CustomPainter>();
        customPainter.enabled = false;
    }

    public void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetKey("escape"))
        {
            Application.Quit();
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (gameState == GameState.START_UI)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
                StartRaceStage();
        }

        else if(gameState == GameState.RACE_STAGE)
        {
            uiController.UpdateRankText(GetRank());
        }
    }

    public void StartRaceStage()
    {
        gameState = GameState.RACE_STAGE;
        uiController.SetGamePlayUI();

        foreach (Competitor competitor in competitors)
        {
            competitor.StartRunning();
        }
    }

    public void FinishRaceStage(GameObject winner)
    {
        foreach (Competitor competitor in competitors)
        {
            competitor.StopRunning();
        }

        cameraController.FocusOnWinner(winner.transform);
        uiController.SetGameEndText(winner.GetComponent<Competitor>().isPlayer);
        LoadWallPaintingStage();
    }

    private void LoadWallPaintingStage()
    {
        gameState = GameState.WALL_PAINTING_STAGE;
        cameraController.enabled = false;
        customPainter.enabled = true;
    }

    private int GetRank()
    {
        List<Competitor> sortedZPosList = competitors.OrderBy(x => x.transform.position.z).ToList();
        int rank = 11 - (sortedZPosList.FindIndex(x => x.isPlayer == true) + 1);

        return rank;
    }
}
