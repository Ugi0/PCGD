using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    public GameObject[] gameObjects; //indexes: 0: silhouette, 1: clouds, 2: buildings, 3: road, 4: bush
    public float[] speeds;
    public float[] maxPositions;
    public float[] minPositions;
    private bool isSkating;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(isSkating)
        {
            for(int i=0; i<gameObjects.Length; i++)
            {
                UpdatePosition(i);
            }
        }
        else
        {
            UpdatePosition(1);
        }
    }

    void UpdatePosition(int index)
    {
        if(gameObjects[index].transform.localPosition.x > minPositions[index])
        {
            gameObjects[index].transform.localPosition = new Vector2(gameObjects[index].transform.localPosition.x+speeds[index]*Time.deltaTime, gameObjects[index].transform.localPosition.y);
        }
        else
        {
            gameObjects[index].transform.localPosition = new Vector2(maxPositions[index], gameObjects[index].transform.localPosition.y);
        }
    }

    public void SkatingTransition(bool skate)
    {
        isSkating = skate;
    }

}
