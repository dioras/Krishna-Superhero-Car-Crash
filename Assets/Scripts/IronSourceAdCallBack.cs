using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceAdCallBack : MonoBehaviour
{
    public void InterstialAdClosed()
    {
        switch (IronSourceAdManager.instance.interstitialAdVal)
        {
            case 0:
                FindObjectOfType<GameStatus>()?.OnRestartAdComplete();
                break;
            case 1:
                FindObjectOfType<GameStatus>()?.OnNextLevelAdClosed();

                break;
            case 2:
                FindObjectOfType<GameStatus>()?.OnHomeAdClosed();

                break;

            default:
                Debug.Log("Improper Index");
                break;
        }
    }

    public void RewardedAdClosed() //REWARD: DailyReward
    {

        if (IronSourceAdManager.instance.rewardEarned)
        {

            switch (IronSourceAdManager.instance.rewardedAdval)
            {
                case 0:
                    FindObjectOfType<GameStatus>()?.OnRewardedAdCompleted_RestartLC();
                    break;
                case 3:
                    HeroMenu.Agent.OnAdForHero();
                    break;
                case 4:
                    GarageMenu.Agent.OnADForCar();
                    break;
                case 5:
                    GameData.SetTotalCoins(GameData.GetTotalCoins() + 500);
                    Debug.Log("testing");
                    break;
                case 6:
                   // GameplayUIManager.Agent.PlayCoinAnim();
                    GameStatus.Agent.OnRewardsDoubled();
                    break;
                case 7:
                    GameData.SetTotalCoins(GameData.GetTotalCoins() + 1000);
                    Debug.Log("testing");
                    break;
                case 8: //RewardCar
                    FindObjectOfType<GarageMenu>().OnADForCar();
                    Debug.Log("testing");
                    break;
                case 9: //Skip
                    FindObjectOfType<GameplayUIManager>().SkipLevel();
                    break;
                case 10: //Repair
                    GameManager.Agent.OnRepairVehicle();
                    break;
                case 11:
                    BCG_UIInteractionButton.agent.afterAd();
                    break;
                default:
                    Debug.Log("Improper Index");
                    break;
            }
            IronSourceAdManager.instance.rewardEarned = false; //NOTE: DO NOT REMOVE THIS

        }
        else
        {
            IronSourceAdManager.instance.rewardEarned = false;
        }
    }
}
