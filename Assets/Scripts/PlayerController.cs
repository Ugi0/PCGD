using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("References")]
    public GameObject powerBar;
    public GameObject[] throwables;
    public GameObject aimingHand;
    public GameObject throwingHand;
    public CustomSlider powerSlider;
    public Animator playerAnimator;
    public Animator skateboardAnimator;
    public GameObject crosshair;
    public GameObject crosshairPosition;
    public GameObject oldCrossHair;

    [Header("Settings")]
    public float poweringSpeed = 3.0f;
    public float aimingSpeed = 150f; // Degrees per second

    private bool powerIncreasing = true;
    private float currentPower;
    private PlayerState playerState;
    private GameObject currentThrowable;
    private Vector3 aimDirection;
    private Vector3 oldAimDirection;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetPlayer();
        oldCrossHair = GameObject.Find("Crosshair_previous");
        oldCrossHair.SetActive(false);
    }

    public void StartSkating()
    {
        playerState = PlayerState.SKATING;
        ResetPlayer();
        playerAnimator.SetBool("SkatePhase", true);
        GameStateManager.instance.StartTransition();
        powerSlider.HidePrevious();
    }


    public void StopSkating()
    {   
        AnimateSkateboard(false);
        GameStateManager.instance.StopDelayedAction("StopSkating");
        playerAnimator.SetBool("SkatePhase", false);
        BackgroundManager.instance.SkatingTransition(false);
    }

    public void AnimateSkateboard(bool animate)
    {
        if(animate)
        {
            skateboardAnimator.speed = 1;
        }
        else
        {
            skateboardAnimator.speed = 0;
        }
    }

    public void ResetThrow()
    {
        int randomIndex = Random.Range(0, throwables.Length); // for now
        currentThrowable = throwables[randomIndex];
        InstantiateThrowable();
    }

    public void ResetPlayer()
    {
        AnimateSkateboard(false);
        crosshair.transform.SetParent(aimingHand.transform);
        crosshair.transform.SetLocalPositionAndRotation(crosshairPosition.transform.localPosition,crosshairPosition.transform.localRotation);
        crosshair.transform.localScale = crosshairPosition.transform.localScale;
        powerBar.SetActive(false);
        aimingHand.SetActive(false);
        currentPower = 0f;
        powerSlider.SetCurrent(0f);
        playerAnimator.SetBool("ThrowPhase", false);
        if(playerState != PlayerState.SKATING)
        {
            BecomeIdle();
        }
    }

    public void BecomeIdle()
    {
        playerState = PlayerState.IDLE;
        playerAnimator.speed = 1.0f;
        ResetThrow();
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
        powerSlider.SetCurrent(currentPower);
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
                StartThrowing();
                break;

            case PlayerState.WAITING:
                ExecuteThrow();
                break;
        }
    }

    public void showOldThrow(bool show = true) {
        if (show) {
            oldCrossHair.SetActive(true);
            oldCrossHair.transform.SetParent(aimingHand.transform);
            oldCrossHair.transform.SetLocalPositionAndRotation(crosshairPosition.transform.localPosition,crosshairPosition.transform.localRotation);
            oldCrossHair.transform.localScale = crosshairPosition.transform.localScale;
            oldCrossHair.transform.SetParent(null);
        } else {
            oldCrossHair.SetActive(false);
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
        powerSlider.SetCurrent(0f);
        currentPower = 0f;
        powerIncreasing = true;
        crosshair.transform.SetParent(null);
        playerAnimator.speed = poweringSpeed;

        oldAimDirection = aimDirection;
        showOldThrow();

        playerAnimator.SetBool("AimPhase", false);
        playerAnimator.SetBool("PowerPhase", true);
    }

    private void StartThrowing()
    {
        playerState = PlayerState.WAITING;
        playerAnimator.speed = 0;
    }

    void ExecuteThrow()
    {
        playerState = PlayerState.THROWING;
        powerBar.SetActive(false);
        aimingHand.SetActive(false);
        playerAnimator.speed = 2.0f;
        playerAnimator.SetBool("PowerPhase", false);
        playerAnimator.SetBool("ThrowPhase", true);

        powerSlider.SetPrevious(currentPower);
        
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
    WAITING,
    SKATING,
}