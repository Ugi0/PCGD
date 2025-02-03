using UnityEngine;

public class Target : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ThrowableObject"))
        {
            ScoreManager.instance.AddPoints(); // Add points to the score
            ThrowScript.instance.ResetThrow(); //make new throwable object
        }
    }
}

