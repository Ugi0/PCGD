using UnityEngine;

public class CollisionObject : MonoBehaviour
{
    public CollisionType collisionType = CollisionType.NONE;

    public void HandleHit()
    {
        switch(collisionType)
        {
            case CollisionType.GROUND:
                HandleTrafficCollisionHit();
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

    public void HandleTrafficCollisionHit()
    {
        Debug.Log("Ground hit!");
        PlayerController.instance.ResetPlayer();
    }

    public void HandleTrafficSignCollisionHit()
    {
        ScoreManager.instance.AddPoints();
        Debug.Log("Traffic sign hit!");
    }
}


public enum CollisionType
{
    NONE,
    GROUND,
    TRAFFIC_SIGN,
    BUILDING_WINDOW,
}