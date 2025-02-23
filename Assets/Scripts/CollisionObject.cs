using UnityEngine;

public class CollisionObject : MonoBehaviour
{
    public CollisionType collisionType = CollisionType.NONE;
    public Sprite brokenSprite;

    public void HandleHit()
    {

        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().sprite = brokenSprite;

        switch (collisionType)
        {
            case CollisionType.GROUND:
                HandleGroundCollisionHit();
                break;
            case CollisionType.TRAFFIC_SIGN:
                HandleTrafficSignCollisionHit();
                break;
            case CollisionType.NONE:
                Debug.LogError("Collision type not set!");
                break;
            default:
                Debug.LogError("Unimplemented collision type!");
                break;
        }
    }

    public void HandleGroundCollisionHit()
    {
        GameStateManager.instance.HitGround();
        Debug.Log("Ground hit!");
        PlayerController.instance.ResetPlayer();
    }

    public void HandleTrafficSignCollisionHit()
    {
        GameStateManager.instance.AddPoints();
        Debug.Log("Traffic sign hit!");
        AudioManager.Instance.PlaySFX("HitStreetSign");
    }
}


public enum CollisionType
{
    NONE,
    GROUND,
    TRAFFIC_SIGN,
    BUILDING_WINDOW,
}