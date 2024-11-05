using Unity.VisualScripting;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    //private float movementSpeed = 7.5f;

    private Rigidbody2D rb;
    private Vector2 playerMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerControl();
        
    }

    private void playerControl()
    {
        playerMove = new Vector2(0, Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = playerMove * 15f;
    }
}
