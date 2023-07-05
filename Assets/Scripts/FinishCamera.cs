using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FinishCamera : MonoBehaviour
{
    public Transform Car;
    public Vector3 PositionOffset;
    public float lookHeight;
    [SerializeField] private bool isFollowCamera;
    private bool PositionChanging;

    [SerializeField] private float targetValue;
    [SerializeField] private float heightChangeTime;
    //[SerializeField] private CinemachineVirtualCamera CVirtualCam;

    // Start is called before the first frame update
    void Start()
    {
        Car = GameManager.Agent.heroVehicle.transform.GetChild(2).transform;
    }

    // Update is called once per frame
    void Update()
    {
        //CVirtualCam.UpdateCameraState(Vector3.zero, Time.deltaTime);
        //Vector3 cameraPosition = CVirtualCam.State.FinalPosition;
        //Quaternion cameraOrientation = CVirtualCam.State.FinalOrientation;
        var target = Car.position + PositionOffset;
        Vector3 refVel = target - transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref refVel, 0f);
        transform.LookAt(Car.position + new Vector3(0, lookHeight, 0));
        //Debug.Log(Car.rotation.x + "TestRotation");
        //if (isFollowCamera)
        //{
        //    if (Car.rotation.x > 0.4)
        //    {
        //        targetValue = -1.1f;
        //    }
        //    else if (Car.rotation.x > 0.25)
        //    {
        //        targetValue = -0.7f;
        //    }
        //    else
        //    {
        //        targetValue = 0f;
        //    }

        //    if (lookHeight != targetValue)
        //    {
        //            lookHeight = Mathf.Lerp(lookHeight, targetValue,heightChangeTime* Time.deltaTime);
        //    }

        //}
    }
}
