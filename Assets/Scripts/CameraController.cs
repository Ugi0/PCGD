using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Camera MainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       MainCamera.aspect = 920/400;
    }
}