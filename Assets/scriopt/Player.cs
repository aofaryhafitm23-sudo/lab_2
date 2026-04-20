using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 14f;

    [Header("Wrap")]
    public float leftBorder = -3.2f;
    public float rightBorder = 3.2f;

    private Rigidbody2D rb;
    private float horizontalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                horizontalInput = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                horizontalInput = 1f;
        }

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        Vector3 pos = transform.position;

        if (pos.x < leftBorder)
        {
            pos.x = rightBorder;
            transform.position = pos;
        }
        else if (pos.x > rightBorder)
        {
            pos.x = leftBorder;
            transform.position = pos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Platform")) return;
        if (collision.contactCount == 0) return;
        if (rb.linearVelocity.y > 0f) return;

        ContactPoint2D contact = collision.GetContact(0);

        if (contact.normal.y <= 0.5f) return;

        BouncePlatform bounce = collision.gameObject.GetComponent<BouncePlatform>();

        if (bounce != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounce.bounceForce);
            Debug.Log("BOUNCE WORKED: " + bounce.bounceForce);
            return;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
}