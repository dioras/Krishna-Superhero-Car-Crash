using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
//using GameAnalyticsSDK.Events;

public class GameStatus : MonoBehaviour
{
    public static GameStatus Agent;
    private GameManager gameManager;
    private GameplayUIManager gameplayUIManager;
    private TimerController timerController;
    [HideInInspector] public bool PlayerReachedDestination;
    [HideInInspector] public int PlayerRank = 0;


    private void Awake()
    {
        Agent = this;
    }

    private void Start()
    {
        gameManager = GameManager.Agent;
        gameplayUIManager = GameplayUIManager.Agent;
        timerController = TimerController.Agent;

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_start_" + GameData.GetCurrentLevel() + 1);
    }

    public void OnHome()
    {
        if (GameData.GetCurrentLevel() != 0 && !IronSourceAdManager.instance.rewardEarned)
        {
            IronSourceAdManager.instance.ShowInterstitial(2);
        }
        else
        {
            gameplayUIManager.LoadingScreen.SetActive(true);
            Time.timeScale = 1;
            SceneManager.LoadScene(1);
        }
    }
    public void OnHomeAdClosed()
    {
        gameplayUIManager.LoadingScreen.SetActive(true);
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    int randomHero;
    public void OnRestartLC()
    {
        IronSourceAdManager.instance.ShowRewardedAd(0);
    }

    public void OnRewardedAdCompleted_RestartLC()
    {
        GameData.GetCurrentHero();
        while (randomHero == GameData.GetCurrentHero())
        {
            //if (GameData.GetCurrentHero() < GameData.GetTotalUnlockedHeroes())
            //{
            //    randomHero = GameData.GetCurrentHero() + 1;
            //}
            //else
            //{
            //    randomHero = GameData.GetCurrentHero() - 1;
            //}
            randomHero = Random.Range(0, GameData.GetTotalUnlockedHeroes());
        }
        GameData.SetCurrentHero(randomHero);
        OnRestart();
    }
    public void OnRestart()
    {
        if (GameData.GetCurrentLevel() != 0 && !IronSourceAdManager.instance.rewardEarned)
        {
            IronSourceAdManager.instance.ShowInterstitial(0);
        }
        else
        {
            gameplayUIManager.LoadingScreen.SetActive(true);

            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OnRestartAdComplete()
    {
        gameplayUIManager.LoadingScreen.SetActive(true);

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnPauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        gameplayUIManager.pauseMenu.SetActive(true);
        gameplayUIManager.currencyScreen.SetActive(true);
    }

    public void OnResumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        gameplayUIManager.pauseMenu.SetActive(false);
        gameplayUIManager.currencyScreen.SetActive(false);
    }

    public void OnNextLevel()
    {
        if (GameData.GetCurrentLevel() != 0 && !IronSourceAdManager.instance.rewardEarned)
        {
            IronSourceAdManager.instance.ShowInterstitial(1);
        }
        else
        {
            gameplayUIManager.LoadingScreen.SetActive(true);
            Time.timeScale = 1;
            if (gameManager.currentLevelIndex >= 19)
            {
                GameData.MenuGUIIndex = 1;
                SceneManager.LoadScene(1);
            }
            else
            {
                GameData.SetCurrentLevel(GameData.GetTotalLevelsUnlocked()-1);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    //public void OnNextLevelAdClosed()
    //{
    //    gameplayUIManager.LoadingScreen.SetActive(true);
    //    Time.timeScale = 1;
    //    GameData.MenuGUIIndex = 1;
    //    SceneManager.LoadScene(1);
    //}
    public void OnNextLevelAdClosed()
    {
        gameplayUIManager.LoadingScreen.SetActive(true);
        Time.timeScale = 1;
        if (gameManager.currentLevelIndex >= 19)
        {
            GameData.MenuGUIIndex = 1;
            SceneManager.LoadScene(1);
        }
        else
        {
            GameData.SetCurrentLevel(GameData.GetTotalLevelsUnlocked()-1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }     
    }

    private int PositionReward, TimeReward, RestartReward, TotalReward;

    public void OnLevelComplete()
    {
        //Time.timeScale = 0.2f;
        Invoke(nameof(CarExplosion), 1.5f);
        Invoke(nameof(DisplayLevelCompleteScreen), 4f);

        gameManager.LevelFinishCamera.SetActive(true);
        //Timer
        gameplayUIManager.LC_TimeRewardText.text = gameplayUIManager.TimerText.text;

        //Position
        switch (PlayerRank)
        {
            case 1:
                gameplayUIManager.Star1.SetActive(true);
                gameplayUIManager.Star2.SetActive(true);
                gameplayUIManager.Star3.SetActive(true);
                break;
            case 2:
                gameplayUIManager.Star1.SetActive(true);
                gameplayUIManager.Star2.SetActive(true);
                gameplayUIManager.Star3.SetActive(false);
                break;
            case 3:
                gameplayUIManager.Star1.SetActive(true);
                gameplayUIManager.Star2.SetActive(false);
                gameplayUIManager.Star3.SetActive(false);
                break;
        }

        gameplayUIManager.LC_StarImageFill.fillAmount = 3 / PlayerRank;
        gameplayUIManager.LC_RankText.text = PlayerRank.ToString("00") + "/03";
        //PositionReward = PlayerRank * 500;
        switch (PlayerRank)
        {
            case 1:
                PositionReward = (3 + GameData.GetCurrentLevel() + 1) * 1000;
                break;
            case 2:
                PositionReward = (2 + GameData.GetCurrentLevel() + 1) * 1000;
                break;
            case 3:
                PositionReward = (1 + GameData.GetCurrentLevel() + 1) * 1000;
                break;
        }
        gameplayUIManager.LC_RankRewardText.text = PositionReward.ToString("00");

        int starsEarned = 0;
        switch (PlayerRank)
        {
            case 1:
                RestartReward = 1000;
                starsEarned = 3;
                break;
            case 2:
                RestartReward = 500;
                starsEarned = 2;
                break;
            case 3:
                RestartReward = 0000;
                starsEarned = 1;
                break;
        }

        GameData.SetLevelStars(gameManager.currentLevelIndex, starsEarned);

        //Time Reward
        if (timerController.GetMinutes() < 1)
            TimeReward = 500;
        else
            TimeReward = 0;
        gameplayUIManager.LC_TimeRewardText.text = TimeReward.ToString("00");

        //Restart Reward
       // RestartReward = 500;
        gameplayUIManager.LC_RestartRewardText.text = RestartReward.ToString("00");

        //Total Rewards
        TotalReward = PositionReward + TimeReward + RestartReward;
        GameData.SetTotalCoins(GameData.GetTotalCoins() + TotalReward);
        gameplayUIManager.LC_TotalRewardText.text = string.Format("{0:#,##,##0}", TotalReward);//.ToString("00");
        gameplayUIManager.LC_TotalRewardPopupText.text = TotalReward.ToString("00");
        gameplayUIManager.LC_TotalDoubledReward.text = string.Format("{0:#,##,##0}", (TotalReward * 2));
        //Unlock Next Level
        GameData.SetLevelLockStatus(gameManager.currentLevelIndex + 1);
//        Debug.LogError("----" + GameData.GetCurrentLevel());

    }
    private void CarExplosion()
    {
        gameManager.ExplosionParticles.transform.position = gameManager.heroVehicle.transform.position;
        gameManager.ExplosionParticles.SetActive(true);
        gameManager.ExplosionParticles.transform.SetParent(gameManager.heroVehicle.transform);
        gameManager.heroVehicle.GetComponent<Rigidbody>().AddExplosionForce(10000f,gameManager.heroVehicle.transform.position,10f);
        //gameManager.heroVehicle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        //gameManager.heroVehicle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
    }
    private void DisplayLevelCompleteScreen()
    {
        AudioManager.Agent.LevelComplete.Play();
        gameplayUIManager.LC_TimerText.text = timerController.timerText.text;

        Time.timeScale = 0;

        gameplayUIManager.GameplayUI.SetActive(false);
        gameplayUIManager.levelCompleteScreen.SetActive(true);
        gameplayUIManager.currencyScreen.SetActive(true);

        //gameManager.heroVehicle.SetActive(false);
        //gameManager.aiVehicle1.SetActive(false);
        //gameManager.aiVehicle2.SetActive(false);

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_complete_" + GameData.GetCurrentLevel() + 1);
    }

    public void OnLevelFailed()
    {
        AudioManager.Agent.LevelFailed.Play();

        gameManager.heroVehicle.SetActive(false);
        //gameManager.aiVehicle1?.SetActive(false);
        //gameManager.aiVehicle2?.SetActive(false);
        //gameplayUIManager.LF_TimerText.text = timerController.timerText.text;

        Time.timeScale = 0;
        gameplayUIManager.GameplayUI.SetActive(false);
        gameplayUIManager.levelFailedScreen.SetActive(true);
        gameplayUIManager.currencyScreen.SetActive(true);

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "level_fail_" + GameData.GetCurrentLevel() + 1);
    }
    public void OnRewardsDoubled()
    {
        gameplayUIManager.DoubleRewardsButton.SetActive(false);
        GameData.SetTotalCoins(GameData.GetTotalCoins() + (TotalReward*2));
        gameplayUIManager.LC_TotalRewardText.text = (TotalReward *2).ToString("00");
    }

    public void Reward1000()
    {
        IronSourceAdManager.instance?.ShowRewardedAd(7);
    }
}
