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

    [Header("Settings")]
    public float minAngle = -75f;
    public float maxAngle = 75f;
    public float poweringSpeed = 3.0f;
    public float aimingSpeed = 150f; // Degrees per second

    private float currentAngle;
    private bool aimingDirection = true; // True for increasing, False for decreasing
    private bool powerIncreasing = true;
    private float currentPower;
    private PlayerState playerState;
    private GameObject currentThrowable;

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

        playerState = PlayerState.IDLE;

        powerBar.SetActive(false);
        aimingHand.SetActive(false);
        currentAngle = (minAngle + maxAngle) / 2; // Start at the midpoint
        currentPower = 0f;
        powerSlider.value = 0f;
        playerAnimator.SetBool("AimPhase", false);
        playerAnimator.SetBool("PowerPhase", false);
        playerAnimator.speed = 0.0f;
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

        // Debug
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer();
            Debug.Log("Player Reset!");
        }
    }

    void UpdateAim()
    {
        float deltaAngle = aimingSpeed * Time.deltaTime;

        if (aimingDirection)
        {
            currentAngle += deltaAngle;
            if (currentAngle >= maxAngle)
            {
                currentAngle = maxAngle;
                aimingDirection = false;
            }
        }
        else
        {
            currentAngle -= deltaAngle;
            if (currentAngle <= minAngle)
            {
                currentAngle = minAngle;
                aimingDirection = true;
            }
        }

        aimingHand.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
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
        playerAnimator.speed = 1.0f;
    }

    void LockAim()
    {
        playerState = PlayerState.POWERING;
        powerBar.SetActive(true);
        currentPower = 0f;
        powerIncreasing = true;
        powerSlider.value = 0f;

        playerAnimator.SetBool("AimPhase", false);
        playerAnimator.SetBool("PowerPhase", true);
        playerAnimator.speed = 1.0f;
    }

    void ExecuteThrow()
    {
        playerState = PlayerState.THROWING;
        powerBar.SetActive(false);
        aimingHand.SetActive(false);
        
        Rigidbody2D rigidbody2D = currentThrowable.GetComponent<Rigidbody2D>();
        rigidbody2D.constraints = RigidbodyConstraints2D.None; //removes throwable object's rigidbody constrains to allow it simulate physics
        currentThrowable.transform.SetParent(null);

        Vector2 throwDirection = CalculateThrowDirection();
        Throw(throwDirection);

        // Reset after throw
        playerState = PlayerState.WALKING;

        playerAnimator.speed = 0.0f;
    }

    Vector2 CalculateThrowDirection()
    {
        float angleInRadians = currentAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
        return direction.normalized * currentPower;
    }

    void Throw(Vector2 direction)
    {
        ThrowableObject throwable = currentThrowable.GetComponent<ThrowableObject>();
        throwable.Initialize();
        throwable.Launch(direction);

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