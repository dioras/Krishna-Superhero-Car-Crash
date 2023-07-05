using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraCon : MonoBehaviour
{

    public Transform car;              // The car that the camera will follow
    public float distance = 5.0f;      // The distance between the camera and the car
    public float height = 2.0f;        // The height of the camera above the car
    public float rotationDamping = 3.0f;   // The rate at which the camera rotates
    public float heightDamping = 2.0f;

    public float lookSpeed;
    private void Start()
    {
        car = GameManager.Agent.heroVehicle.transform;
    }

    private void Update()
    {
        // Calculate the desired rotation angle based on the car's current rotation
        float wantedRotationAngle = car.eulerAngles.y;
        float wantedHeight = car.position.y + height;

        // Calculate the current rotation angle and height
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Smoothly rotate the camera towards the desired angle
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Smoothly move the camera towards the desired height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane
        Vector3 targetPosition = car.position - currentRotation * Vector3.forward * distance;
        targetPosition.y = currentHeight;

        // Set the position and rotation of the camera
        transform.position = targetPosition;
        //transform.rotation = currentRotation;

        Vector3 lookDirection = car.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDirection), lookSpeed * Time.deltaTime);
    }
}

