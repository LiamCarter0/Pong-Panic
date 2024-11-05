using UnityEngine;

public class Brick : MonoBehaviour
{
    public int points = 1; // Points awarded for breaking this brick
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            Ball ballScript = col.gameObject.GetComponent<Ball>();
           
            ScoreManager.Instance.AddPoints(ballScript.lastPaddleHit, points);
            
            // Destroy the brick after collision
            Destroy(gameObject);

            
        }

        if (col.gameObject.CompareTag("Brick Deleter"))
        {
            Destroy(gameObject);
        }
    }
}
