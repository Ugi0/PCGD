using UnityEditor;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    public ThrowableData data;
    private bool hitGround; //makes sure that object doesn't hit ground and reset player multiple times

    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = data.gravityScale;
        rb.linearDamping = data.drag;
        hitGround = false;
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

    // TODO: change this system?
    private void CheckTargetHit(GameObject hitObject)
    {
        // Check if hit object is a valid target
        if(!hitObject.CompareTag("CollisionObject"))
        {
            return;
        }
        CollisionObject collisionObject = hitObject.GetComponent<CollisionObject>();
        
        if (collisionObject != null && !hitGround)
        {
            HandleObjectHit(collisionObject);
        }
    }

    private void HandleObjectHit(CollisionObject collisionObject)
    {
        if(data.hitEffectPrefab != null)
            Instantiate(data.hitEffectPrefab, transform.position, Quaternion.identity);

        collisionObject.HandleHit();

        if(collisionObject.collisionType == CollisionType.GROUND)
        {
            hitGround = true;
            Destroy(gameObject, 3f);
        }
    }


}
