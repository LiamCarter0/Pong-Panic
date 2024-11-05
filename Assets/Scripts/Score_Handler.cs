using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;


    private int player1Score = 0;
    private int player2Score = 0;

    public int Player1Score => player1Score; // Property to access Player 1's score
    public int Player2Score => player2Score; // Property to access Player 2's score

    public TextMeshProUGUI player1ScoreText; // Reference to Player 1's score Text
    public TextMeshProUGUI player2ScoreText; // Reference to Player 2's score Text

    void Start()
    {
        UpdateScoreDisplay();
    }
    void Awake()
    {
        // Singleton pattern to easily reference the ScoreManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to add points based on the paddle that last hit the ball
    public void AddPoints(GameObject paddle, int points)
    {
        if (paddle.CompareTag("Player1"))
        {
            player1Score += points;
            //Debug.Log("Player 1 Score: " + player1Score);
        }
        else if (paddle.CompareTag("Player2 or AI"))
        {
            player2Score += points;
            //Debug.Log("Player 2 Score: " + player2Score);
        }
        else
        {
            Debug.Log("oh so broken");
        }
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {

        player1ScoreText.text = "Player 1 Score: " + player1Score;
        player2ScoreText.text = "Player 2 Score: " + player2Score;
    }
}
