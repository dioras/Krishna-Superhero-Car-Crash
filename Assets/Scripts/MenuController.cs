using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class MenuController : MonoBehaviour
{
    public static MenuController agent;
    [SerializeField] private GameObject MainMenuPanel, LevelsMenuPanel, GarageMenuPanel, GarageSetup,HeroGarageSetup,MainMenuBG;
    [SerializeField] private Text CareerModeStarsText;//, unlockedHeroesText;
    [SerializeField] private Image CareerModeStarFillImage;//, HeroesFillImage;
    [SerializeField] private Color GarageFogColor;
    [SerializeField] private PostProcessVolume PostPro;
    [SerializeField] private PostProcessProfile GarageProfile;
    [SerializeField] private GameObject BGM;
    [SerializeField] private GameObject OtherPack, CoinPack;

    [SerializeField] private bool test;

    [HideInInspector] public int menuindex =0;
    private void Awake()
    {
        
        if (agent == null)
            agent = this;

        if (test)
        {
            GameData.SetTotalCoins(99999999);
        }
        if (!GarageSetup.activeInHierarchy || HeroGarageSetup.activeInHierarchy || MainMenuBG.activeInHierarchy)
        {
            GarageSetup.SetActive(false);
            HeroGarageSetup.SetActive(false);
            MainMenuBG.SetActive(true);
            RenderSettings.fogColor = new Color32(126, 153, 184, 255);
        }
        if(HeroGarageSetup.activeInHierarchy)
        {
            MainMenuBG.SetActive(false);
        }
        PostPro.profile = GarageProfile;
        RenderSettings.fogColor = GarageFogColor;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.01f;
#if !UNITY_EDITOR
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
#endif

//        GarageMenuPanel.SetActive(true);
        GarageMenuPanel.SetActive(false);

        if (GameData.MenuGUIIndex == 0)
        {
            MainMenuPanel.SetActive(true);
            LevelsMenuPanel.SetActive(false);
        }
        else if (GameData.MenuGUIIndex == 1)
        {
            GameData.MenuGUIIndex = 0;
            MainMenuPanel.SetActive(false);
            LevelsMenuPanel.SetActive(true);
        }
    }
    int i = 0;
    public void UnlockLevelTestBtn()
    {
        GameData.SetLevelLockStatus(i + 1);
        i++;
    }
    private void OnEnable()
    {
        FindObjectOfType<TopPanelManager>().PlayVal = 0;
        menuindex = 0;

        if (!BGM.activeInHierarchy)
            BGM.SetActive(true);
    }
    public void OnClickGarageButton()
    {
        if (BGM.activeInHierarchy)
            BGM.SetActive(false);
        MainMenuBG.SetActive(false);
        GarageSetup.SetActive(true);
    }
    private void Start()
    {
        //if (AspectRatio.instance.TabAspect)
        //{
        //    OtherPack.transform.localScale = new Vector3(0.82f,0.8f,0.8f);
        //    CoinPack.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        //}
        //else
        //{
        //    OtherPack.transform.localScale = new Vector3(1f, 1f, 1f);
        //    CoinPack.transform.localScale = new Vector3(1f,1f, 1f);
        //}
        //GarageMenu.Agent.Start();

       // unlockedHeroesText.text = GameData.GetTotalUnlockedHeroes() + "/" + GameData.TotalHeroes;

        //Career Mode Stars
        CareerModeStarsText.text = GameData.GetTotalStars() + "/" + (GameData.TotalLevels * 3);
        CareerModeStarFillImage.fillAmount = GameData.GetTotalStars() / (float)(GameData.TotalLevels * 3);

        //HeroesFillImage.fillAmount = GameData.GetTotalUnlockedHeroes() / (float)(GameData.TotalHeroes);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
