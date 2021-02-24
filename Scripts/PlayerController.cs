using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action<String,String> OnPlayerEvent;
    public Action<float> CreaterObjects;
    private Rigidbody rb;
    public float powerUpTime;
    private bool powerUP;
    private float powerUpStartTime;
    public Joystick joystick;
    public float currentspeed, maxSpeed, minSpeed, verticalSpeed, horizontalSpeed, boostSpeed;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        currentspeed = 0;
        powerUP = false;
    }

    private void Update()
    {
        if (powerUP)
        {
            if (powerUpStartTime > Time.realtimeSinceStartup)
            {
                OnPlayerEvent.Invoke("PowerUp", "POWER UP!\n" + (powerUpStartTime - Time.realtimeSinceStartup).ToString("0"));
            }
            else
            {
                OnPlayerEvent.Invoke("PowerUp", "");
                powerUP = false;
            }
        }
        
    }

    void FixedUpdate ()
    {
        Vector3 movemont;
        
        float moveHorizontal = horizontalSpeed * Input.GetAxis("Horizontal");
        //float moveHorizontal = horizontalSpeed * joystick.Horizontal;
        float moveVertical = verticalSpeed * Input.GetAxis("Vertical");
        //float moveVertical = verticalSpeed * joystick.Vertical;
        currentspeed = rb.velocity.magnitude;
        if (moveVertical > 0)
        {
            if(rb.velocity.magnitude < maxSpeed)
            {
                movemont = new Vector3(moveHorizontal, 0.0f, (3.0f + moveVertical) * verticalSpeed);
            }
            else
            {
                movemont = new Vector3(moveHorizontal, 0.0f, (3.0f) * verticalSpeed);
            }
        }
        else if(moveVertical < 0)
        {
            if (rb.velocity.magnitude > minSpeed)
            {
                movemont = new Vector3(moveHorizontal, 0.0f, (3.0f + moveVertical) * verticalSpeed/8);
            }
            else
            {
                movemont = new Vector3(moveHorizontal, 0.0f, (3.0f) * verticalSpeed);
            }
        }
        else
        {
            movemont = new Vector3(moveHorizontal, 0.0f, (3.0f) * verticalSpeed);
        }

        rb.AddForce(movemont);

    }

    
    public void OnTriggerEnter(Collider others)
    {
        if (others.gameObject.GetComponent<ObjectType>().value == 3)
        {
            if (OnPlayerEvent != null)
            {
                OnPlayerEvent.Invoke("Finish","");
            }
        }
        else if (others.gameObject.GetComponent<ObjectType>().value == 4)
        {
            if (OnPlayerEvent != null)
            {
                OnPlayerEvent.Invoke("Die", "");
            }
        }
        else if (others.gameObject.GetComponent<ObjectType>().value == 5)
        {
            rb.AddForce(new Vector3(0.0f, 0.0f, (3.0f) * boostSpeed));
        }
        else if(others.gameObject.GetComponent<ObjectType>().value == 6)
        {
            OnPlayerEvent.Invoke("Recycle", transform.position.z.ToString());
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (OnPlayerEvent != null)
        {
            if (collision.gameObject.GetComponent<ObjectType>().value == 0 && !powerUP)
            {
                OnPlayerEvent.Invoke("Die","");
            }
            else if (collision.gameObject.GetComponent<ObjectType>().value == 2)
            {
                Destroy(collision.gameObject);
                OnPlayerEvent.Invoke("Coin","");
            }
            else if (collision.gameObject.GetComponent<ObjectType>().value == 99)
            {
                Destroy(collision.gameObject);
                powerUpStartTime = Time.realtimeSinceStartup + powerUpTime;
                powerUP = true;
            }
        }

    }






}
