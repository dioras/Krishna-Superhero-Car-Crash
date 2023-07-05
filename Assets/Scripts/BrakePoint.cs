using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class BrakePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("BRAKEE");
            GameManager.Agent.heroVehicle.GetComponent<RCC_CarControllerV3>().speed = 0f;
        }
    }
}
