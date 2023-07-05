using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRank : MonoBehaviour
{
    public Text RankText;
    float PlayerDistance;
    float AI1Distance;
    float AI2Distance;

    [HideInInspector] public bool PlayerB;
    [HideInInspector] public bool AI1;
    [HideInInspector] public bool AI2;

    public void Start()
    {
       
    }
    private void FixedUpdate()
    {
        if(PlayerB)
        CheckPlayerDistance();

        if (AI1)
            CheckAI1Distance();

        if (AI2)
            CheckAI2Distance();
    }

    void CheckPlayerDistance()
    {
        PlayerDistance = GameManager.Agent.PlayerDistance;
        AI1Distance = GameManager.Agent.AI1Distance;
        AI2Distance = GameManager.Agent.AI2Distance;

        if (PlayerDistance < AI1Distance && PlayerDistance < AI2Distance)
        {
            RankText.text = "<b>1</b>st";
        }else if(PlayerDistance > AI1Distance && PlayerDistance < AI2Distance)
        {
            RankText.text = "<b>2</b>nd";
        }
        else if (PlayerDistance < AI1Distance && PlayerDistance > AI2Distance)
        {
            RankText.text = "<b>2</b>nd";
        }
        else
        {
            RankText.text = "<b>3</b>rd";
        }
    }


    void CheckAI1Distance()
    {
        PlayerDistance = GameManager.Agent.PlayerDistance;
        AI1Distance = GameManager.Agent.AI1Distance;
        AI2Distance = GameManager.Agent.AI2Distance;

        if (AI1Distance < PlayerDistance && AI1Distance < AI2Distance)
        {
            RankText.text = "<b>1</b>st";
        }
        else if (AI1Distance > PlayerDistance && AI1Distance < AI2Distance)
        {
            RankText.text = "<b>2</b>nd";
        }
        else if (AI1Distance < PlayerDistance && AI1Distance > AI2Distance)
        {
            RankText.text = "<b>2</b>nd";
        }
        else
        {
            RankText.text = "<b>3</b>rd";
        }
    }

    void CheckAI2Distance()
    {
        PlayerDistance = GameManager.Agent.PlayerDistance;
        AI1Distance = GameManager.Agent.AI1Distance;
        AI2Distance = GameManager.Agent.AI2Distance;

        if (AI2Distance < AI1Distance && AI2Distance < PlayerDistance)
        {
            RankText.text = "<b>1</b>st";
        }
        else if (AI2Distance > AI1Distance && AI2Distance < PlayerDistance)
        {
            RankText.text = "<b>2</b>nd";
        }
        else if (AI2Distance < AI1Distance && AI2Distance > PlayerDistance)
        {
            RankText.text = "<b>2</b>nd";
        }
        else
        {
            RankText.text = "<b>3</b>rd";
        }
    }
}
