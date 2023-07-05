using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public GameObject Part1, Part2;
    public GameObject WayPointPart1, WayPointPart2;
    private void Awake()
    {
        if(GameData.GetCurrentLevel() > 24)
        {
            if(Part1)
            Part1.SetActive(false);

            if(Part2)
            Part2.SetActive(true);

            if(WayPointPart1)
            WayPointPart1.SetActive(false);

            if(WayPointPart2)
            WayPointPart2.SetActive(true);
        }
        else
        {

            if (Part1)
                Part1.SetActive(true);

            if (Part2)
                Part2.SetActive(false);

            if (WayPointPart1)
                WayPointPart1.SetActive(true);

            if (WayPointPart2)
                WayPointPart2.SetActive(false);
        }
    }
}
