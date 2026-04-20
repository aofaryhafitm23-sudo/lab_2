using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float leftLimit = -2.5f;
    public float rightLimit = 2.5f;

    private int direction = 1;

    private void Update()
    {
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        if (transform.position.x >= rightLimit)
            direction = -1;
        else if (transform.position.x <= leftLimit)
            direction = 1;
    }
}