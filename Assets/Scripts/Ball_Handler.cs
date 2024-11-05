using System.Collections;
using System.Collections.Generic; // Add this line for List<>
using UnityEngine;
public class Ball_Handler : MonoBehaviour
{
    public GameObject ballPrefab; // Assign your ball prefab here in the Inspector
    private List<GameObject> player1Balls = new List<GameObject>();
    private List<GameObject> player2Balls = new List<GameObject>();

    private float launchSpeed = 6f;

    void Start()
    {
        SpawnPlayer1Ball();
        SpawnPlayer2Ball();
    }

    // Spawns a new ball at the specified position
    public void SpawnPlayer1Ball()
    {
        if (ballPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(-7.9f, -1f, 0); // Set the desired spawn position here
            GameObject newBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            //Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
            player1Balls.Add(newBall);
            LaunchBallRight();
        }
        else
        {
            Debug.LogError("Ball Prefab is not assigned in the Inspector!");
        }
    }

    public void SpawnPlayer2Ball()
    {
        if (ballPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(7.9f, -1f, 0); // Set the desired spawn position here
            GameObject newBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            //Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
            player2Balls.Add(newBall);
            LaunchBallLeft();
        }
        else
        {
            Debug.LogError("Ball Prefab is not assigned in the Inspector!");
        }
    }

    public void HandleBallLost(GameObject lostBall)
    {
        if (player1Balls.Contains(lostBall))
        {
            player1Balls.Remove(lostBall); // Remove the lost ball from the list
            Destroy(lostBall);              // Destroy the lost ball
            SpawnPlayer1Ball();             // Spawn a new player 1 ball
        }
        else if (player2Balls.Contains(lostBall))
        {
            player2Balls.Remove(lostBall); // Remove the lost ball from the list
            Destroy(lostBall);              // Destroy the lost ball
            SpawnPlayer2Ball();             // Spawn a new player 2 ball
        }
    }



    public void LaunchBallRight()
    {
        foreach (GameObject ball in player1Balls)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 launchDirection = new Vector2(1, Random.Range(-1f, 1f)).normalized;
                rb.linearVelocity = launchDirection * launchSpeed; // Use 'velocity' for 2D Rigidbody
            }
        }
    }

    public void LaunchBallLeft()
    {
        foreach (GameObject ball in player2Balls)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 launchDirection = new Vector2(-1, Random.Range(-1f, 1f)).normalized;
                rb.linearVelocity = launchDirection * launchSpeed; // Use 'velocity' for 2D Rigidbody
            }
        }
    }

}