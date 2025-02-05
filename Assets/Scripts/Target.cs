using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] GameObject spawnArea;
    [SerializeField] float distanceFromPlayer;
    [SerializeField] float heightOfTarget;

    void Start() {
        float colliderWidth = spawnArea.GetComponent<BoxCollider2D>().bounds.size.x;
        float colliderHeight = spawnArea.GetComponent<BoxCollider2D>().bounds.size.y;
        Vector3 rescale = gameObject.transform.localScale;
        rescale.y = heightOfTarget;
        gameObject.transform.localScale = rescale;
        gameObject.transform.position = new Vector2(
            Random.Range(spawnArea.transform.position.x - colliderWidth, spawnArea.transform.position.x) + distanceFromPlayer, 
            Random.Range(spawnArea.transform.position.y - colliderHeight, spawnArea.transform.position.y)
        );
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ThrowableObject"))
        {
            ScoreManager.instance.AddPoints(); // Add points to the score
            ThrowScript.instance.ResetThrow(); //make new throwable object
        }
    }
}

