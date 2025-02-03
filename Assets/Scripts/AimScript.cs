using UnityEngine;

public class AimScript : MonoBehaviour
{
    public GameObject powerBar;
    public GameObject throwableObject;
    public GameObject aimingHand;
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
        throwableObject.GetComponent<CircleCollider2D>().enabled = true;
        Rigidbody2D rigidbody2D = throwableObject.GetComponent<Rigidbody2D>();
        rigidbody2D.constraints = RigidbodyConstraints2D.None;
        throwPowerScaler = (position-minXPosition)*(100 / (maxXPosition - minXPosition));
        rigidbody2D.AddForce(transform.right*throwPower*throwPowerScaler);
    }
}
