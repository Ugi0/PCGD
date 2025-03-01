using System.Collections;
using UnityEngine;

public class PoliceCarManager : MonoBehaviour
{
    public static PoliceCarManager instance;

    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 15f;
    public Animator animator;

    void Awake()
    {
        instance = this;
    }

    public IEnumerator MoveSequence()
    {
        yield return MoveToTarget(pointA);
        yield return MoveToTarget(pointB);
        animator.SetBool("IsStopped", true);
        Time.timeScale = 0f; // Pause game
    }

    IEnumerator MoveToTarget(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

}
