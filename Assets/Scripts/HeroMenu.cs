using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Reflection;
using UnityEngine.Rendering.PostProcessing;

public class HeroMenu : MonoBehaviour
{
    public static HeroMenu Agent;
    [SerializeField] private GameObject[] mHeroes;
    private int currentHeroIndex;
    [SerializeField] private int[] mHeroPrices;
    //Hero Prizes: 0,5000,10000,15000,20000,30000,50000,70000,100000,150000(Place 0 to make hero watch ad)
    [SerializeField] private GameObject BuyButton, ReadyButton, ReadyButtonP, HeroLockImage , watchADButton;

    [SerializeField] private Text BuyText;

    [Header("Hero Stats")]
    [SerializeField] private Image heroImage;
    [SerializeField] private Slider stuntExperienceSlider, controllingSlider, driftSkillsSlider, strengthSlider;
    [SerializeField] private Text stuntExperienceSliderText, controllingSliderText, driftSkillsSliderText, strengthSliderText, heroNameText, heroRankText;
    [SerializeField] private Image heroRankImage;
    [SerializeField] private GameObject GarageSetup;
    [SerializeField] private GameObject HeroGarageSetup;
    [SerializeField] private GameObject heroScrollContent;

    public Color GarageFogColour;
    public Color HeroGarageFogColour;

    [SerializeField] private PostProcessVolume PostPro;
    [SerializeField] private PostProcessProfile GarageProfile;
    [SerializeField] private PostProcessProfile HeroProfile;
    [System.Serializable]
    public class HeroStats
    {
        public Sprite heroImageSprite;
        public float stuntExperienceVal, controllingVal, driftSkillsVal, strengthVal;
        public string heroNameVal, heroRankVal;
        public Color rankColourVal;
    }
    public HeroStats[] heroStats;
    public void OnEnable()
    {
        CheckForReadyButton();
        PostPro.profile = HeroProfile;
        RenderSettings.fogColor = HeroGarageFogColour;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 2.9f;
        RenderSettings.fogEndDistance = 10.95f;
        GarageSetup.SetActive(false);
        HeroGarageSetup.SetActive(true);
        mHeroes[currentHeroIndex].transform.position = new Vector3(mHeroes[currentHeroIndex].transform.position.x, 0f, mHeroes[currentHeroIndex].transform.position.z);
        OnSelectHero(GameData.GetCurrentHero());
    }
    public void CheckForReadyButton()
    {
        if (FindObjectOfType<TopPanelManager>().PlayVal == 1)
        {
            ReadyButton.SetActive(false);
            ReadyButtonP.SetActive(true);
        }
        else
        {
            ReadyButton.SetActive(true);
            ReadyButtonP.SetActive(false);
        }
    }

    private void Start()
    {
        OnSelectHero(GameData.GetCurrentHero());
        Agent = this;
    }
   
    public void OnBackButtonFog()
    {
        PostPro.profile = GarageProfile;
        GarageSetup.SetActive(true);
        HeroGarageSetup.SetActive(false);
        RenderSettings.fogColor = GarageFogColour;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.015f;
        CharacterIKGarage.instance.SetupCharacter(null, GameData.GetCurrentHero());
    }
    public void OnSelectHero(int index)
    {
        currentHeroIndex = index;
        if(index ==8 || index == 9)
        {
            heroScrollContent.GetComponent<ScrollRect>().verticalScrollbar.value = 0f;
        }
        if(index == 0 || index == 1)
        {
            heroScrollContent.GetComponent<ScrollRect>().verticalScrollbar.value = 1f;
        }
        for (int i = 0; i < mHeroes.Length; i++)
        {
            mHeroes[i].SetActive(false);
        }
        mHeroes[index].transform.position = new Vector3(-0.85f, 0f, 1.24f);
        mHeroes[index].transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        mHeroes[index].SetActive(true);

        //Stats
        heroImage.sprite = heroStats[currentHeroIndex].heroImageSprite;
        stuntExperienceSlider.value = heroStats[currentHeroIndex].stuntExperienceVal/10;
        controllingSlider.value = heroStats[currentHeroIndex].controllingVal/10;
        driftSkillsSlider.value = heroStats[currentHeroIndex].driftSkillsVal/10;
        strengthSlider.value = heroStats[currentHeroIndex].strengthVal/10;
        stuntExperienceSliderText.text = heroStats[currentHeroIndex].stuntExperienceVal.ToString();
        controllingSliderText.text = heroStats[currentHeroIndex].controllingVal.ToString();
        driftSkillsSliderText.text = heroStats[currentHeroIndex].driftSkillsVal.ToString();
        strengthSliderText.text = heroStats[currentHeroIndex].strengthVal.ToString();
        heroNameText.text = heroStats[currentHeroIndex].heroNameVal;
        heroRankText.text = heroStats[currentHeroIndex].heroRankVal;
        heroRankImage.color = heroStats[currentHeroIndex].rankColourVal;

        //Hero locked
        if (GameData.GetHeroLockStatus(index) == 0)
        {
            if (mHeroPrices[currentHeroIndex] == 0)
            {
                watchADButton.SetActive(true);
                BuyButton.SetActive(false);
            }
            else if (GameData.GetTotalCoins() >= mHeroPrices[currentHeroIndex])
            {
                BuyButton.GetComponent<Button>().interactable = true;
                watchADButton.SetActive(false);
                BuyButton.SetActive(true);
            }
            else
            {
                BuyButton.GetComponent<Button>().interactable = false;
                watchADButton.SetActive(false);
                BuyButton.SetActive(true) ;
                ReadyButton.SetActive(false);
            }

            BuyText.text = mHeroPrices[index].ToString("n0");
            //BuyButton.SetActive(true);
            ReadyButton.SetActive(false);
            ReadyButtonP.SetActive(false);

            HeroLockImage.SetActive(true);
            //watchADButton.SetActive(true);
        }
        //Hero Unlocked
        else
        {
            BuyButton.SetActive(false);
            CheckForReadyButton();
            HeroLockImage.SetActive(false);
            watchADButton.SetActive(false);
            GameData.SetCurrentHero(currentHeroIndex);
        }
    }
    public void OnReadyButton2Tap()
    {
        if (GameData.GetHeroLockStatus(currentHeroIndex) == 1)
            GameData.SetCurrentHero(currentHeroIndex);
    }
    public void OnReadyButtonTap()
    {
        if (GameData.GetHeroLockStatus(currentHeroIndex) == 1)
            GameData.SetCurrentHero(currentHeroIndex);

        OnBackButtonFog();
    }

    public void OnBuyHero()
    {
        //Enough Coins
        if (GameData.GetTotalCoins() >= mHeroPrices[currentHeroIndex])
        {
            GameData.SetTotalCoins(GameData.GetTotalCoins() - mHeroPrices[currentHeroIndex]);
            GameData.SetHeroLockStatus(currentHeroIndex);
            BuyButton.SetActive(false);
            HeroLockImage.SetActive(false);
            watchADButton.SetActive(false);
            CheckForReadyButton();       
            GameData.SetCurrentHero(currentHeroIndex);
        }
    }
    public void OnAdForHero()
    {
        GameData.SetHeroLockStatus(currentHeroIndex);
        BuyButton.SetActive(false);
        HeroLockImage.SetActive(false);
        watchADButton.SetActive(false);
        CheckForReadyButton();
        GameData.SetCurrentHero(currentHeroIndex);
        Debug.Log("test");
    }
    public void WatchAD()
    {
        IronSourceAdManager.instance.ShowRewardedAd(3);
    }
}
