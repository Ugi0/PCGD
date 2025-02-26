using UnityEditor;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    public ThrowableData data;
    private bool alreadyHit;

    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = data.gravityScale;
        rb.linearDamping = data.drag;
        alreadyHit = false;
    }

    public void Launch(Vector2 direction)
    {
        switch (data.flightPattern)
        {
            case ThrowableType.IMPULSE:
                rb.AddForce(direction * data.initialPower, ForceMode2D.Impulse);
                rb.AddTorque(data.torque, ForceMode2D.Impulse);
                break;
            case ThrowableType.FORCE:
                rb.AddForce(direction * data.initialPower, ForceMode2D.Force);
                break;
            case ThrowableType.STRAIGHT:
                Debug.LogError("Flight pattern not implemented!");
                break;
            case ThrowableType.NONE:
                Debug.LogError("Flight pattern not set!");
                break;
            default:
                Debug.LogError("Flight pattern not implemented!");
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckTargetHit(collision.gameObject);
    }

    private void CheckTargetHit(GameObject hitObject)
    {
        // Check if hit object is a valid target
        if(hitObject.CompareTag("CollisionObject"))
        {
            CollisionObject collisionObject = hitObject.GetComponent<CollisionObject>();
        
            if (collisionObject != null && !alreadyHit)
            {
                HandleObjectHit(collisionObject);
            }
        }
        else if(hitObject.CompareTag("Obstacle"))
        {
            Debug.Log("Bird hit!");
            hitObject.GetComponent<MovingObstacle>().StopMoving();
            hitObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            AudioManager.Instance.PlaySFX("BirdHit");
            hitObject.GetComponent<CollisionObject>().HandleGroundCollisionHit();
        }
    }

    private void HandleObjectHit(CollisionObject collisionObject)
    {
        if(data.hitEffectPrefab != null)
            Instantiate(data.hitEffectPrefab, transform.position, Quaternion.identity);

        collisionObject.HandleHit();
        alreadyHit = true;
        Destroy(gameObject, .5f);
    }
}
