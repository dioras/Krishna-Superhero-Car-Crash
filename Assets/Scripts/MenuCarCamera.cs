using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCarCamera : MonoBehaviour
{
    public Transform Car;
    public Vector3 PositionOffset;
    public float lookHeight;
    public float lookXOffset;
    [SerializeField] private bool lookCamera;
    [SerializeField] private Animator CameraAnim;

    private void Start()
    {
        if (!lookCamera)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.GetChild(0).transform.position = new Vector3(0, 0, 0);
            CameraAnim.Play("MenuCarCamera");
        }
    }
    private void Update()
    {
        if (!lookCamera)
        {
            var target = Car.position + PositionOffset;
            Vector3 refVel = target - transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref refVel, 0f);
        }
        transform.LookAt(Car.position + new Vector3(lookXOffset, lookHeight, 0));
    }
    private void OnEnable()
    {
        if (!lookCamera)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.GetChild(0).transform.position = new Vector3(0, 0, 0);
            CameraAnim.Play("MenuCarCamera");
        }
    }
}
