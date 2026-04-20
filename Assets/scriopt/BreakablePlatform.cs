using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public float breakDelay = 0.05f;
    public float fallGravity = 4f;

    private bool isBroken = false;
    private Rigidbody2D rb;
    private Collider2D col;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb == null) return;

        if (playerRb.linearVelocity.y <= 0f)
        {
            isBroken = true;
            Invoke(nameof(BreakPlatform), breakDelay);
        }
    }

    private void BreakPlatform()
    {
        if (col != null) col.enabled = false;

        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallGravity;

        Destroy(gameObject, 1.2f);
    }
}