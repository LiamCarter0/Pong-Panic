using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Ball_Handler ballHandler;
    private GameManager gameManager;
    private Rigidbody2D rb;
    
    public GameObject lastPaddleHit; // Stores the last paddle that hit the ball
    public GameObject player1Paddle; // Reference to Player 1's paddle
    public GameObject player2Paddle; 
    public float nearZeroThresh = 0.05f;

    private int loseBallPenalty = 3;

    private AudioSource audioSource;
    public AudioClip paddleHitSound;    // Assigned in the Inspector
    public AudioClip brickHitSound;     
    public AudioClip destructionSound;    


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ballHandler = Object.FindFirstObjectByType<Ball_Handler>();
        gameManager = Object.FindFirstObjectByType<GameManager>();
        
        lastPaddleHit = player1Paddle;
        loseBallPenalty = gameManager.ballPenalty;

        rb = GetComponent<Rigidbody2D>();
    }

   

    // Checks if the ball's velocity is zero and launches it if true
    //was creating horrible buggy ball movement so scrapped
   /* private void CheckAndLaunchIfStopped()
    {
        if (Mathf.Abs(rb.linearVelocity.magnitude) < nearZeroThresh || Mathf.Abs(rb.linearVelocity.y) < nearZeroThresh)
        {
            Vector2 horizontalDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

            rb.linearVelocity = horizontalDirection * 6f;
            Debug.Log("Ball velocity near zero. Relaunching in horizontal direction.");
        }
    }*/

    void OnTriggerEnter2D(Collider2D trigger)
    {
       
        if (trigger.CompareTag("Lose Ball Left"))
        {
            // Deduct points from player 1
            ScoreManager.Instance.AddPoints(player2Paddle, loseBallPenalty);
        }
        else if (trigger.CompareTag("Lose Ball Right"))
        {
            // Deduct points from player 2
            ScoreManager.Instance.AddPoints(player1Paddle, loseBallPenalty);
        }

        //Debug.Log("Lose Ball trigger detected, calling HandleBallLost(). Trigger tag: " + trigger.tag);
        //PlaySound(destructionSound);
        //ballHandler.HandleBallLost(gameObject);
        StartCoroutine(HandleBallDestruction());
    }

    //necessessary to play the sound before the ball can be destroyed
    private IEnumerator HandleBallDestruction()
    {
        PlaySound(destructionSound); // Play the destruction sound
        yield return new WaitForSeconds(0.5f); // Wait for half a second
        ballHandler.HandleBallLost(gameObject); // Call the HandleBallLost method
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2 or AI"))
        {
            // Store the paddle that last hit the ball
            lastPaddleHit = other.gameObject;

            PlaySound(paddleHitSound);
            //Debug.Log("yay");
        }

        //multiply rb.linearvelocity by 1.1 up to 5 and then by 1.05 to a cap of 10 speed
        if (rb != null)
        {
            if (rb.linearVelocity.magnitude <= 5f)
            {
               rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity * 1.25f, 5f);
                
            }
            else
            {
                rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity * 1.05f, 10f);
            }

            if (other.gameObject.CompareTag("Brick"))
            {
                rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity * 1.5f, 15f);

                PlaySound(brickHitSound);
            }
        }
    }

    public void SetPenalty(int newPenalty)
    {
        loseBallPenalty = newPenalty;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
