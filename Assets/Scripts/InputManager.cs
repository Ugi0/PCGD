using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction pressAction;
    private InputAction positionAction;
    public GameObject player;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        pressAction = playerInput.actions["Press"];
        positionAction = playerInput.actions["Position"];
    }

    void Update()
    {
        if(pressAction.triggered)
        {
            if(TouchingShooter())
            {
                player.GetComponent<PlayerController>().HandleInput();
            }
        }
    }

    bool TouchingShooter()
    {
        Vector2 touchpos = Camera.main.ScreenToWorldPoint(positionAction.ReadValue<Vector2>());
        if (player.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchpos))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}