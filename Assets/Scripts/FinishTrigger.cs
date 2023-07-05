using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI"))
        {
            other.GetComponent<VehicleBehaviour>().AIFinishCallBack();
        }
        else if (other.CompareTag("Player"))
        {

            other.GetComponent<VehicleBehaviour>().PlayerFinishCallBack();
        }
    }
}
