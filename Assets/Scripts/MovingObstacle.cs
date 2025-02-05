using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float speed = 3f; // Speed of movement
    public float height = 2f; // Distance it moves up and down
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Store initial position
    }

    void Update()
    {
        // Create up and down movement using Mathf.Sin
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("ThrowableObject"))
    {
        ThrowScript.instance.ResetThrow(); // Reset the throwable object
    }
}
}

