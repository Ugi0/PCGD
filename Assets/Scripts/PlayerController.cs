using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("References")]
    public GameObject powerBar;
    public GameObject[] throwables;
    public GameObject aimingHand;
    public GameObject throwingHand;
    public Slider powerSlider;
    public Animator playerAnimator;
    public GameObject crosshair;
    public GameObject crosshairPosition;

    [Header("Settings")]
    public float poweringSpeed = 3.0f;
    public float aimingSpeed = 150f; // Degrees per second

    private bool powerIncreasing = true;
    private float currentPower;
    private PlayerState playerState;
    private GameObject currentThrowable;
    private Vector3 aimDirection;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        int randomIndex = Random.Range(0, throwables.Length); // for now
        currentThrowable = throwables[randomIndex];
        InstantiateThrowable();
        crosshair.transform.SetParent(aimingHand.transform);
        crosshair.transform.SetLocalPositionAndRotation(crosshairPosition.transform.localPosition,crosshairPosition.transform.localRotation);
        crosshair.transform.localScale = crosshairPosition.transform.localScale;
        powerBar.SetActive(false);
        aimingHand.SetActive(false);
        currentPower = 0f;
        powerSlider.value = 0f;
        BecomeIdle();
    }

    void BecomeIdle()
    {
        playerState = PlayerState.IDLE;
        playerAnimator.SetBool("ThrowPhase", false);
        playerAnimator.speed = 1.0f;
    }

    void Update()
    {
        switch (playerState)
        {
            case PlayerState.AIMING:
                UpdateAim();
                break;

            case PlayerState.POWERING:
                UpdatePower();
                break;
        }
    }

    void UpdateAim()
    {
        aimDirection = aimingHand.transform.right;
    }

    void UpdatePower()
    {
        if (powerIncreasing)
        {
            currentPower += poweringSpeed * Time.deltaTime;
            if (currentPower >= 1.0f)
            {
                currentPower = 1.0f;
                powerIncreasing = false;
            }
        }
        else
        {
            currentPower -= poweringSpeed * Time.deltaTime;
            if (currentPower <= 0.0f)
            {
                currentPower = 0.0f;
                powerIncreasing = true;
            }
        }
        powerSlider.value = currentPower;
    }

    void OnMouseUp()
    {
        switch (playerState)
        {
            case PlayerState.IDLE:
                StartAiming();
                break;

            case PlayerState.AIMING:
                LockAim();
                break;

            case PlayerState.POWERING:
                ExecuteThrow();
                break;
        }
    }

    void StartAiming()
    {
        playerState = PlayerState.AIMING;
        aimingHand.SetActive(true);
        playerAnimator.SetBool("AimPhase", true);
        playerAnimator.speed = aimingSpeed;
    }

    void LockAim()
    {
        playerState = PlayerState.POWERING;
        powerBar.SetActive(true);
        currentPower = 0f;
        powerIncreasing = true;
        powerSlider.value = 0f;
        crosshair.transform.SetParent(null);
        playerAnimator.speed = poweringSpeed;

        playerAnimator.SetBool("AimPhase", false);
        playerAnimator.SetBool("PowerPhase", true);
    }

    void ExecuteThrow()
    {
        playerState = PlayerState.THROWING;
        powerBar.SetActive(false);
        aimingHand.SetActive(false);
        playerAnimator.speed = 2.0f;
        playerAnimator.SetBool("PowerPhase", false);
        playerAnimator.SetBool("ThrowPhase", true);
        
        Rigidbody2D rigidbody2D = currentThrowable.GetComponent<Rigidbody2D>();
        rigidbody2D.constraints = RigidbodyConstraints2D.None; //removes throwable object's rigidbody constrains to allow it simulate physics
        currentThrowable.transform.SetParent(null);

        Vector2 throwDirection = CalculateThrowDirection();
        Throw(throwDirection);
    }

    Vector2 CalculateThrowDirection()
    {
        return aimDirection.normalized * currentPower;
    }

    void Throw(Vector2 direction)
    {
        ThrowableObject throwable = currentThrowable.GetComponent<ThrowableObject>();
        throwable.Initialize();
        throwable.Launch(direction);
        AudioManager.Instance.PlaySFX("Throw");
    }

    private void InstantiateThrowable()
    {
        currentThrowable = Instantiate(
            currentThrowable,
            throwingHand.transform.position,
            Quaternion.identity
        );
        currentThrowable.transform.SetParent(throwingHand.transform);
    }
}

public enum PlayerState
{
    IDLE,
    AIMING,
    POWERING,
    THROWING,
    WALKING,
}