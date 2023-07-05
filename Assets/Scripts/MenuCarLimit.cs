using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCarLimit : MonoBehaviour
{
    private Rigidbody rb;
    public float maxSpeed = 10f;
    public GameObject ramp;
    private Vector3 newRampPos;
    public static MenuCarLimit agent;
    private void awake()
    {
        agent = this;
    }
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Vector3 currentVelocity = rb.velocity;
        float currentSpeed = currentVelocity.magnitude;
        if (currentSpeed > maxSpeed)
        {
            Vector3 clampedVelocity = Vector3.ClampMagnitude(currentVelocity, maxSpeed);
            rb.velocity = clampedVelocity;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("RampTrigger"))
        {
            SpawnRamp();
            Debug.Log("TestingRamp");
        }
    }
    public void SpawnRamp()
    {
        GameObject newramp = Instantiate(ramp);
        newramp.transform.localScale = new Vector3(0.45f,0.45f,0.45f);
        newRampPos = newRampPos + new Vector3(0,-920.4f,3207.8f);
        newRampPos.x = 200;
        newramp.transform.position = newRampPos;
    }
    private void OnEnable()
    {
        StartForce();
    }
    public void StartForce()
    {
        transform.position = new Vector3(0, 0.2f, 0) + transform.position;
    }
}
