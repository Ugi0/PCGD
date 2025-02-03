using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene reloading

public class ThrowableObject : MonoBehaviour
{
    private Vector3 startPosition; // Store the starting position
    private Rigidbody2D rb; // Rigidbody2D component reference

    void Start()
    {
        startPosition = transform.position; // Save initial position
        rb = GetComponent<Rigidbody2D>();    // Get Rigidbody2D component
    }

    void Update()
    {
        // Check if the object is too far away from the target (missed target)
        if (Mathf.Abs(transform.position.x) > 30 || Mathf.Abs(transform.position.y) > 10)
        {
            ThrowScript.instance.ResetThrow(); //create new throwable object
        }
    }

    void ResetGame()
    {
        // Option 1: Reload the current scene (this resets everything in the scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Option 2: You could manually reset objects here instead of reloading the scene
        // ResetPosition(); // Uncomment this if you want to manually reset positions
    }
}
