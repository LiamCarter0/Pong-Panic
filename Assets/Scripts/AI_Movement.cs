using UnityEngine;

public class AI_Movement : MonoBehaviour
{
    //public Transform[] balls;        // Reference to the ball's Transform
    public float speed = 6f;       // Speed at which the AI paddle moves
    public float moveThreshold = 0.1f; // How closely the AI should follow the ball
    private Transform closestBall;

    public float minY = -4f;
    public float maxY = 3.8f;

    void Update()
    {
        closestBall = FindClosestBall();

        if (closestBall != null)
        {
            // Get the ball's y position
            float targetY = closestBall.position.y;

            // Only move the paddle if the ball is outside the threshold
            if (Mathf.Abs(targetY - transform.position.y) > moveThreshold)
            {
                // Calculate the new y position for the AI paddle
                float newY = Mathf.MoveTowards(transform.position.y, targetY, speed * Time.deltaTime);
                newY = Mathf.Clamp(newY, -4f, 3.8f);
                // Update the paddle's position (only y-axis movement)
                transform.position = new Vector2(transform.position.x, newY);
            }
        }
    }

    private Transform FindClosestBall()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        Transform nearestBall = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject ball in balls)
        {
            float distance = Mathf.Abs(ball.transform.position.x - transform.position.x);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestBall = ball.transform;
            }
        }

        return nearestBall;
    }

    public void SetMovementSpeed(float movespeed)
    {
        speed = movespeed;
    }

    public void SetMovementThresh(float thresh)
    {
        moveThreshold = thresh;
    }
}
