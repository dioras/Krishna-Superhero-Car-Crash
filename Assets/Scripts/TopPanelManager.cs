using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] UiPanels;
    [SerializeField] private GameObject ExitPopup;
    public GameObject BackButton,ExitButton;
    public GameObject CurrencyPanelUI,HomeButton , WatchAdButton;
    public bool inMenuScene;
    public HeroMenu hm;

    public int PlayVal;
    public GameObject BGM;
    public GameObject LevelSelectionPanel;
    public GameObject GarageScene, GaragePanel;
    public GameObject HeroScene, HeroPanel;
    public GameObject MenuPanel;
    public GameObject LoadingGUI;

    [SerializeField] private GameObject MainMenuBG;
    // Start is called before the first frame update
    void Start()
    {
        if (inMenuScene)
        {
            UpdateUIPanels(0);
        }
        IronSourceAdManager.instance.ShowBanner(0);
        //float scWidth = Screen.width;
        //float scHeight = Screen.height;
        //float ScreenARatio = scWidth / scHeight;
        //if (ScreenARatio >= 2f)
        //{
        //    ExitPopup.transform.localScale = new Vector3(1f, 1f, 1f);
        //}
        //else if (ScreenARatio >= 1.8f)
        //{
        //    ExitPopup.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        //}
        //else if (ScreenARatio >= 1.4f)
        //{
        //    ExitPopup.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        //}
        //else
        //{
        //    ExitPopup.transform.localScale = new Vector3(1f, 1f, 1f);
        //}

    }
    public void UpdateUIPanels(int index)
    {
        for (int i = 0; i < UiPanels.Length; i++)
        {
            UiPanels[i].SetActive(false);
        }
        UiPanels[index].SetActive(true);
        if(index == 0)
        {
            ExitButton.SetActive(true);
            BackButton.SetActive(false);
            HomeButton.SetActive(false);
            CurrencyPanelUI.SetActive(true);
            //WatchAdButton.SetActive(true);
        }
        else
        {
            ExitButton.SetActive(false);
            BackButton.SetActive(true);
            HomeButton.SetActive(false);
            CurrencyPanelUI.SetActive(true);
            //WatchAdButton.SetActive(false);
        }
        if(index ==1)
        {
            MainMenuBG.SetActive(false);
        }
        
    }
    public void OpenSettingsPanel()
    {
        UpdateUIPanels(3);
        UiPanels[0].SetActive(true);
        HomeButton.SetActive(true);
        CurrencyPanelUI.SetActive(false);
        BackButton.SetActive(false);
        ExitButton.SetActive(false);
    }
    public void OnExitbutton()
    {
        ExitPopup.SetActive(true);
    }
    public void OnHomeButton()
    {

        if (UiPanels[5].activeInHierarchy)
        {
            PlayVal = 0;
        }
        if (PlayVal == 0)
        {
            UpdateUIPanels(0);
            hm.OnBackButtonFog();
            MainMenuBG.SetActive(true);
            GarageScene.SetActive(false);
        }
        else
        {
            if (LevelSelectionPanel.activeInHierarchy)
            {
                UpdateUIPanels(0);
                hm.OnBackButtonFog();
            }
            if (GaragePanel.activeInHierarchy) {
                BackButtontoLevels();
            }
            if (HeroPanel.activeInHierarchy)
            {
                BackButtontoGarage();
            }
        }
    }
    private void Update()
    {
        if (UiPanels[4].activeSelf)
        {
            ExitButton.SetActive(false);
            BackButton.SetActive(true);
            HomeButton.SetActive(false);
            CurrencyPanelUI.SetActive(true);
        }

    }
    public void OnBackCharacter()
    {

    }
    public void onWatchAD()
    {
        IronSourceAdManager.instance.ShowRewardedAd(5);
    }
    public void onWatchAD1000()
    {
        IronSourceAdManager.instance.ShowRewardedAd(7);
    }
    public void OnPlayLevelPanel()
    {
        if (BGM.activeInHierarchy)
            BGM.SetActive(false);

        UpdateUIPanels(4);
        FindObjectOfType<GarageMenu>().OnReadyButtonTap();
        MainMenuBG.SetActive(false);
        GarageScene.SetActive(true);

    }
    public void OnReadyButtonHero()
    {
        FindObjectOfType<HeroMenu>().OnReadyButton2Tap();
        PlayBtn();
    }
    public void OnReadyButtonCar()
    {
        PlayBtn();

       // UpdateUIPanels(1);
    }
    public void BackButtontoLevels()
    {
        GaragePanel.SetActive(false);
        GarageScene.SetActive(false);
        MainMenuBG.SetActive(true);
        LevelSelectionPanel.SetActive(true);
    }

    public void BackButtontoGarage()
    {
        GaragePanel.SetActive(true);
        GarageScene.SetActive(true);
        MainMenuBG.SetActive(false);
        FindObjectOfType<HeroMenu>().OnBackButtonFog();
        HeroPanel.SetActive(false);
        HeroScene.SetActive(false);

    }

    public void PlayBtn()
    {
        LoadingGUI.SetActive(true);
        SceneManager.LoadScene(2);
    }
}
