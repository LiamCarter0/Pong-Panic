using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;
using TMPro.Examples;

public class GameManager : MonoBehaviour
{
    //game management variables
    public int gameDuration = 60;     // Game duration in seconds (1 minute)
    private int timeRemaining;        // Time left in the game
    private bool gameActive = false;   // Whether the game is currently active
    private int gamemode = 0;           // 0 for default, 1 for hard, 2 for chaos
    public int ballPenalty = 3;            //reference for the ball script so i can manipulate 'loseBallPenalty'
    
    private AI_Movement aiMovement;     // Reference to the AI_Movement class
    private UIManager uiManager;        //used for visibility of panels and their buttons
    private Ball_Handler ballHandler;

    private bool multiballTriggered = false;         //for 30s
    private bool extraMultiballTriggered = true;    // for chaos mode

    //Dynamic Texts
    public TextMeshProUGUI timerText;          
    public TextMeshProUGUI winnerText;    

    //Buttons
    public Button restartButton;
    public Button normalButton;
    public Button hardButton;
    public Button chaosButton;
    public Button playButtom;
    public Button howToPlayButton;
    public Button backToMenu;

    //Button actions


    //Panels
    public GameObject gameOverPanel;                 // Assign the Game Over panel in the Inspector
    public GameObject difficultyPanel;
    public GameObject titleScreenPanel;
    public GameObject howToPlayPanel;

    void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        aiMovement = FindFirstObjectByType<AI_Movement>();
        ballHandler = FindFirstObjectByType<Ball_Handler>();

        if (uiManager == null)
        {
            Debug.LogError("UIManager not found!");
        }
        if (aiMovement == null)
        {
            Debug.LogError("AI_Movement not found!");
        }
    }


    void Start()
    {
        Time.timeScale = 0f;  // Pauses all physics and updates
        //uiManager = FindFirstObjectByType<UIManager>();

        difficultyPanel.SetActive(false);   // Hide unwanted panels at start
        gameOverPanel.SetActive(false);
        howToPlayPanel.SetActive(false);

        //uiManager.ShowPanelWithListeners(titleScreenPanel, StartGame);
        ShowTitleScreen();

    }

    void ShowTitleScreen()
    {
        uiManager.HidePanelWithListeners(howToPlayPanel);
        uiManager.HidePanelWithListeners(gameOverPanel);
        titleScreenPanel.SetActive(true);
        Button[] buttons = titleScreenPanel.GetComponentsInChildren<Button>(true);
        buttons[0].onClick.AddListener(ShowDiffScreen);
        buttons[1].onClick.AddListener(ShowHowScreen);

    }
    void ShowHowScreen()
    {
        uiManager.HidePanelWithListeners(titleScreenPanel);
        howToPlayPanel.SetActive(true);
        Button[] buttons = howToPlayPanel.GetComponentsInChildren<Button>(true);
        buttons[0].onClick.AddListener(ShowTitleScreen);
    }
    void ShowDiffScreen()
    {
        uiManager.HidePanelWithListeners(titleScreenPanel);
        difficultyPanel.SetActive(true);
        Button[] buttons = difficultyPanel.GetComponentsInChildren<Button>(true);
        buttons[0].onClick.AddListener(() => StartGame(0));
        buttons[1].onClick.AddListener(() => StartGame(1));
        buttons[2].onClick.AddListener(() => StartGame(2));

    }


    void StartGame(int mode)
    {

        if (uiManager == null)
        {
            Debug.LogError("uiManager is null. Make sure it's assigned.");
            return;
        }
        if (aiMovement == null)
        {
            Debug.LogError("aiMovement is null. Make sure it's assigned.");
            return;
        }


        //important for reseting game later
        gamemode = mode;

        //turning panels off
        uiManager.HidePanelWithListeners(difficultyPanel);
        uiManager.HidePanelWithListeners(gameOverPanel);

        switch (gamemode)
        {
            case 0: // Normal mode
                aiMovement.SetMovementSpeed(6f);
                aiMovement.SetMovementThresh(0.1f);
                ballPenalty = 3;
                break;
            case 1: // Hard mode
                aiMovement.SetMovementSpeed(10f);
                aiMovement.SetMovementThresh(0.05f);
                ballPenalty = 5;
                break;
            case 2: // Chaos mode
                aiMovement.SetMovementSpeed(12f);
                aiMovement.SetMovementThresh(0.03f);
                ballPenalty = 6;
                extraMultiballTriggered = false;
                break;
        }


        //turning physics and the game on
        Time.timeScale = 1f;
        gameActive = true;

        timeRemaining = gameDuration;
        UpdateTimerDisplay();
        StartCoroutine(CountdownTimer());
    }

    IEnumerator CountdownTimer()
    {
        while (timeRemaining > 0 && gameActive)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            timeRemaining--;                     // Reduce time by 1

            if (!extraMultiballTriggered && timeRemaining < 60)
            {
                extraMultiballTriggered = true; // Mark multiball as triggered
                ballHandler.SpawnPlayer1Ball();
                ballHandler.SpawnPlayer2Ball();

            }

            if (!multiballTriggered && timeRemaining <= 30)
            {
                multiballTriggered = true; // Mark multiball as triggered
                ballHandler.SpawnPlayer1Ball();
                ballHandler.SpawnPlayer2Ball();

            }

            UpdateTimerDisplay();
        }

        // End the game if the timer reaches zero
        if (timeRemaining <= 0)
        {
            EndGame();
        }
    }

    // Updates the UI timer display (optional)
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = timeRemaining / 60;
            int seconds = timeRemaining % 60;
            timerText.text = $":{seconds:00}";
        }
    }

    // End the game and reset
    void EndGame()
    {
        // Stop the game
        Time.timeScale = 0f;
        gameActive = false; 
        multiballTriggered = false;
        //Debug.Log("Game Over! Time's up!");

        DetermineWinner(); // Display the winner

        gameOverPanel.SetActive(true);
        Button[] buttons = gameOverPanel.GetComponentsInChildren<Button>(true);
        buttons[0].onClick.AddListener(() => StartGame(gamemode));
        buttons[1].onClick.AddListener(ShowTitleScreen);



    }


    void DetermineWinner()
    {
        int player1Score = ScoreManager.Instance.Player1Score;
        int player2Score = ScoreManager.Instance.Player2Score;

        if (player1Score > player2Score)
        {
            winnerText.text = "Player 1 Wins!";
        }
        else if (player2Score > player1Score)
        {
            winnerText.text = "Player 2 Wins!";
        }
        else
        {
            winnerText.text = "It's a Draw!";
        }
    }


}
