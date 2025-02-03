using UnityEngine;

public class ThrowScript : MonoBehaviour
{
    public static ThrowScript instance;
    public GameObject powerBar;
    private GameObject throwableObject;
    public GameObject throwableObjectPrefab;
    public GameObject aimingHand;
    public GameObject throwingHand;
    private bool aiming = false;
    private bool powering = false;
    public float aimingSpeed;
    public float poweringSpeed;
    public float maxZRotation;
    public float minZRotation;
    public float maxXPosition;
    public float minXPosition;
    private float rotation = 0;
    private float position = 0;
    public float throwPower;
    private float throwPowerScaler;

    private void Awake() {
        instance = this;
    }

    public void ResetThrow()
    {
        Destroy(throwableObject);
        CreateThrowableObject();
    }


    void Start()
    {
        CreateThrowableObject();
    }

    private void CreateThrowableObject()
    {
        throwableObject = Instantiate(throwableObjectPrefab, throwingHand.transform.position, throwingHand.transform.rotation);
        throwableObject.transform.SetParent(throwingHand.transform);
    }

    void OnMouseUp()
    {
        if(powering)
        {   
            powering = false;
            ThrowObject();
        }
        else if(!aiming)
        {
            aiming = true;
        }
        else
        {
            powering = true;
            aiming = false;
        }
    }

    void Update()
    {
        if(aiming)
        {
            transform.Rotate(0.0f,0.0f,aimingSpeed,Space.Self);
            rotation = transform.localRotation.z;
            if(rotation > maxZRotation || rotation < minZRotation)
            {
                aimingSpeed = -aimingSpeed;
            }
        }
        if(powering)
        {
            powerBar.transform.Translate(poweringSpeed,0,0);
            position = powerBar.transform.localPosition.x;
            if(position > maxXPosition || position < minXPosition)
            {
                poweringSpeed = -poweringSpeed;
            }
        }
    }

    private void ThrowObject()
    {
        throwableObject.transform.parent = null;
        Rigidbody2D rigidbody2D = throwableObject.GetComponent<Rigidbody2D>();
        rigidbody2D.constraints = RigidbodyConstraints2D.None;
        throwPowerScaler = (position-minXPosition)*(100 / (maxXPosition - minXPosition));
        rigidbody2D.AddForce(transform.right*throwPower*throwPowerScaler);
        powerBar.transform.localPosition = new Vector3(minXPosition,0,0);
    }
}
