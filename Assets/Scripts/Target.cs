using UnityEngine;

public class Target : MonoBehaviour
{

    public bool canSpawnHighway;
    public bool canSpawnSidewalk;
    public bool canSpawnHouse;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("ThrowableObject")) {
            GameStateManager.instance.registerHit();
            Destroy(gameObject, .5f);
        }
    }
}