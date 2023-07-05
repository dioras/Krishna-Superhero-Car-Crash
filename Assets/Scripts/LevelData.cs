using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static LevelData Agent;
    public RCC_AIWaypointsContainer aiWaypointsContainer1, aiWaypointsContainer2;
    public GameObject mDestinationParticles;
    public Transform[] cameraPositions;
    public Transform HeroVehicleSelectionPosition;

    private void Awake()
    {
        Agent = this;
    }

    private void Start()
    {
        if(aiWaypointsContainer1 != null)
        aiWaypointsContainer1.gameObject.SetActive(true);
        if(aiWaypointsContainer2 != null)
        aiWaypointsContainer2.gameObject.SetActive(true);
    }
}
