using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class IronSourceAdManager : MonoBehaviour
{
    public static IronSourceAdManager instance;
    [SerializeField] private bool showTestAds;
    public bool rewardEarned;
    public int interstitialAdVal;
    public int rewardedAdval;
    public GameObject PopupCanvas;
    public GameObject NoAdsPanel;
    public GameObject PurchaseFailedPopup;
    public GameObject PurchaseLoadingPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

#if UNITY_IOS
        IronSource.Agent.setMetaData("is_test_suite", "enable");
#endif
        GameAnalyticsSDK.GameAnalytics.Initialize();
    }

    public void Start()
    {

#if UNITY_ANDROID
        string appKey = "194df442d";
#elif UNITY_IPHONE
        string appKey = "194ddeafd";
#else
        string appKey = "unexpected_platform";
#endif


        
        Debug.Log("unity-script: IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Debug.Log("unity-script: unity version" + IronSource.unityVersion());

        // SDK init
        Debug.Log("unity-script: IronSource.Agent.init");
        IronSource.Agent.init(appKey);

        //Add Init Event
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        LoadRewardedAd();

        if (!Preferences.NoAds)
        {
            LoadInterstitialAd();
            LoadBanner();
        }

    }

    void SdkInitializationCompletedEvent()
    {
        //Launch test suite
#if UNITY_IOS
        IronSource.Agent.launchTestSuite();
#endif
        Debug.Log("unity-script: I got SdkInitializationCompletedEvent");
    }

    #region REWARDED AD
    void LoadRewardedAd()
    {
        IronSourceRewardedVideoEvents.onAdOpenedEvent += ReardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += ReardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += ReardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += ReardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += ReardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += ReardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += ReardedVideoOnAdClickedEvent;
    }

    void ReardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdOpenedEvent With AdInfo " + adInfo.ToString());
    }
    void ReardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        LoadRewardedAd();
        FindObjectOfType<IronSourceAdCallBack>().RewardedAdClosed();
        Debug.Log("unity-script: I got ReardedVideoOnAdClosedEvent With AdInfo " + adInfo.ToString());
    }
    void ReardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdAvailable With AdInfo " + adInfo.ToString());
    }
    void ReardedVideoOnAdUnavailable()
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdUnavailable");
    }
    void ReardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        LoadRewardedAd();
        PopupCanvas.SetActive(true);
        NoAdsPanel.SetActive(true);
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent With Error" + ironSourceError.ToString() + "And AdInfo " + adInfo.ToString());
    }
    void ReardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        rewardEarned = true;
        Debug.Log("unity-script: I got ReardedVideoOnAdRewardedEvent With Placement" + ironSourcePlacement.ToString() + "And AdInfo " + adInfo.ToString());
    }
    void ReardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got ReardedVideoOnAdClickedEvent With Placement" + ironSourcePlacement.ToString() + "And AdInfo " + adInfo.ToString());
    }

    public void ShowRewardedAd(int index)
    {
        rewardedAdval = index;
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            PopupCanvas.SetActive(true);
            NoAdsPanel.SetActive(true);
            LoadRewardedAd();

            Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
        }
    }
    #endregion


    void LoadInterstitialAd()
    {
        IronSource.Agent.loadInterstitial();
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
    }

    #region INTERSTITIAL
    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdReadyEvent With AdInfo " + adInfo.ToString());
    }

    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
    {
        Debug.Log("unity-script: I got InterstitialOnAdLoadFailed With Error " + ironSourceError.ToString());
    }

    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdOpenedEvent With AdInfo " + adInfo.ToString());
    }

    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdClickedEvent With AdInfo " + adInfo.ToString());
    }

    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdShowSucceededEvent With AdInfo " + adInfo.ToString());
    }

    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        LoadInterstitialAd();
        FindObjectOfType<IronSourceAdCallBack>().InterstialAdClosed();
        Debug.Log("unity-script: I got InterstitialOnAdShowFailedEvent With Error " + ironSourceError.ToString() + " And AdInfo " + adInfo.ToString());
    }

    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        LoadInterstitialAd();
        FindObjectOfType<IronSourceAdCallBack>().InterstialAdClosed();
        Debug.Log("unity-script: I got InterstitialOnAdClosedEvent With AdInfo " + adInfo.ToString());
    }

    public void ShowInterstitial(int index)
    {
        interstitialAdVal = index;
        if (IronSource.Agent.isInterstitialReady() && !Preferences.NoAds) 
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            LoadInterstitialAd();
            FindObjectOfType<IronSourceAdCallBack>().InterstialAdClosed();
            Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
        }
    }
    #endregion

    #region BANNER

    void LoadBanner() {
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
    }

    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdLoadedEvent With AdInfo " + adInfo.ToString());
    }

    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
    {
        Debug.Log("unity-script: I got BannerOnAdLoadFailedEvent With Error " + ironSourceError.ToString());
    }

    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdClickedEvent With AdInfo " + adInfo.ToString());
    }

    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdScreenPresentedEvent With AdInfo " + adInfo.ToString());
    }

    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdScreenDismissedEvent With AdInfo " + adInfo.ToString());
    }

    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got BannerOnAdLeftApplicationEvent With AdInfo " + adInfo.ToString());
    }

    // 0: Bottom; 1: Top
    public void ShowBanner(int val)
    {
        if (!Preferences.NoAds)
        {
            Debug.Log("unity-script: loadBannerButtonClicked");

            if (val == 0)
            {
                IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
            }
            else {
                IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.TOP);
            }
        }
    }

    public void DestroyBanner()
    {
        Debug.Log("unity-script: loadBannerButtonClicked");
        IronSource.Agent.destroyBanner();
    }
    #endregion

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("TargetHUD", "Menu");
    }
    private void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }

    public void EnablePurchaseFailedPopUp()
    {
        PopupCanvas.SetActive(true);
        PurchaseFailedPopup.SetActive(true);
    }
    public void EnablePurchaseLoadingPopUp()
    {
        PopupCanvas.SetActive(true);
        PurchaseLoadingPanel.SetActive(true);
    }
    public void DisblePurchaseLoadingPopUp()
    {
        PopupCanvas.SetActive(false);
        PurchaseLoadingPanel.SetActive(false);
    }

}
