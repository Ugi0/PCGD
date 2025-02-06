using UnityEngine;

[CreateAssetMenu(fileName = "NewThrowable", menuName = "Throwable Object")]
public class ThrowableData : ScriptableObject
{
    [Header("Flight Settings")]
    public ThrowableType flightPattern = ThrowableType.NONE;
    public float gravityScale = 1f;
    public float drag = 0f;
    public float mass = 1f;
    public float angularDamping = 0.05f;
    public float initialPower = 15.0f;
    public float torque = 0.1f;

    [Header("Effects")]
    public GameObject hitEffectPrefab;
    public AudioClip throwSound;
}

public enum ThrowableType
{
    NONE,
    IMPULSE,
    FORCE,
    STRAIGHT,
}
