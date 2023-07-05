using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GarageMenu : MonoBehaviour
{
    public static GarageMenu Agent;
    private GameObject InstantiatedVehicle;
    [SerializeField] private GameObject InstantiationPosition;
    [SerializeField] private GameObject[] mVehiclesGUI, mVehicleSpecifications, mVehiclesPrefab;
    private int currentVehicleIndex = 0;
    [SerializeField] private ScrollRect mScrollRect;
    [SerializeField] private GameObject VehicleLockImage;
    [SerializeField] private GameObject WatchAdButton;
    [SerializeField] private GameObject BuyButton, ReadyButton, ReadyButtonP;
    [SerializeField] private Text BuyText;
    [SerializeField] private int[] mVehiclePrices;
    //Vehicle Prizes: 0,5000,10000,15000,20000,25000,30000,35000,40000,60000,90000,120000,150000,200000,300000(place 0 to make car watch ad)
    [SerializeField] private Transform GarageSetupParent;
    [SerializeField] private AudioClip[] EngineSounds;
    [SerializeField] private AudioSource EngineSound;

    [SerializeField] private float rotationSensitivity;
    private void Awake()
    {
        Agent = this;
        //
    }
    private void OnEnable()
    {
        OnSelectVehicle(currentVehicleIndex);
        CheckForreadyButton();
    }
    public void CheckForreadyButton()
    {
        if (FindObjectOfType<TopPanelManager>().PlayVal == 1)
        {
            if(WatchAdButton.activeInHierarchy)
            {
                ReadyButton.SetActive(false);
                ReadyButtonP.SetActive(false);
            }
            else
            {
                ReadyButton.SetActive(false);
                ReadyButtonP.SetActive(true);
            }
            //WatchAdButton.SetActive(false);
        }
        else
        {
            if (WatchAdButton.activeInHierarchy)
            {
                ReadyButton.SetActive(false);
                ReadyButtonP.SetActive(false);
            }
            else
            {
                ReadyButton.SetActive(true);
                ReadyButtonP.SetActive(false);
            }
        }
    }
    public void Start()
    {
        currentVehicleIndex = GameData.GetCurrentVehicle();
        OnSelectVehicle(currentVehicleIndex);
    }

    public void NextArrow()
    {
        currentVehicleIndex++;
        if (currentVehicleIndex > mVehiclesGUI.Length - 1)
            currentVehicleIndex = 0;
        OnSelectVehicle(currentVehicleIndex);
    }

    public void scrollset(int currentIndex)
    {
        switch (currentIndex)
        {
            case 0:
            case 1:
            case 2:
                mScrollRect.horizontalScrollbar.value = 0f;
                break;
            case 3:
                mScrollRect.horizontalScrollbar.value = 0.075f;
                break;
            case 4:
                mScrollRect.horizontalScrollbar.value = 0.18f;
                break;
            case 5:
                mScrollRect.horizontalScrollbar.value = 0.286f;
                break;
            case 6:
                mScrollRect.horizontalScrollbar.value = 0.389f;
                break;
            case 7:
                mScrollRect.horizontalScrollbar.value = 0.49f;
                break;
            case 8:
                mScrollRect.horizontalScrollbar.value = 0.6f;
                break;
            case 9:
                mScrollRect.horizontalScrollbar.value = 0.705f;
                break;
            case 10:
                mScrollRect.horizontalScrollbar.value = 0.81f;
                break;
            case 11:
                mScrollRect.horizontalScrollbar.value = 0.918f;
                break;
            case 12:
            case 13:
            case 14:
                mScrollRect.horizontalScrollbar.value = 1f;
                break;
        }
    }

    public void PreviousArrow()
    {
        currentVehicleIndex--;
        if (currentVehicleIndex < 0)
            currentVehicleIndex = mVehiclesGUI.Length - 1;
        OnSelectVehicle(currentVehicleIndex);
    }

    public void OnSelectVehicle(int Index)
    {
        currentVehicleIndex = Index;

            EngineSound.clip = EngineSounds[currentVehicleIndex];
            EngineSound.Play();

        scrollset(currentVehicleIndex);
        for (int i = 0; i < mVehiclesGUI.Length; i++)
            mVehiclesGUI[i].transform.Find("SelectedImage").gameObject.SetActive(false);
        mVehiclesGUI[Index].transform.Find("SelectedImage").gameObject.SetActive(true);

        for (int i = 0; i < mVehicleSpecifications.Length; i++)
            mVehicleSpecifications[i].SetActive(false);
        mVehicleSpecifications[Index].SetActive(true);

        if (InstantiatedVehicle)
        {
            Destroy(InstantiatedVehicle);
            CharacterIKGarage.instance.UnParentHeroFromCar();
        }
        InstantiatedVehicle = Instantiate(mVehiclesPrefab[Index], InstantiationPosition.transform.position, Quaternion.identity);
        InstantiatedVehicle.GetComponent<RCC_CarControllerV3>().idleEngineSoundVolume = 0f;
        InstantiatedVehicle.transform.SetParent(GarageSetupParent);
        CharacterIKGarage.instance.SetupCharacter(InstantiatedVehicle.transform,GameData.GetCurrentHero());
        InstantiatedVehicle.GetComponent<RCC_CarControllerV3>().canControl = false;
        Destroy(InstantiatedVehicle.GetComponent<VehicleBehaviour>());
        Destroy(InstantiatedVehicle.GetComponent<Animator>());

        Destroy(InstantiatedVehicle.GetComponent<BCG_EnterExitVehicle>());
        InstantiatedVehicle.SetActive(true);

        //Vehicle locked
        if (GameData.GetVehicleLockStatus(Index) == 0)
        {
            if(mVehiclePrices[currentVehicleIndex] == 0)
            {
                BuyButton.SetActive(false);
                WatchAdButton.SetActive(true);
                ReadyButton.SetActive(false);
                ReadyButtonP.SetActive(false);
            }
            else if (GameData.GetTotalCoins() >= mVehiclePrices[currentVehicleIndex])
            {
                BuyButton.GetComponent<Button>().interactable = true;
                BuyButton.SetActive(true);
                WatchAdButton.SetActive(false);
            }
            else
            {
                BuyButton.GetComponent<Button>().interactable = false;
                BuyButton.SetActive(true);
                WatchAdButton.SetActive(false);
            }
                

            BuyText.text = mVehiclePrices[Index].ToString("n0");
            //BuyButton.SetActive(true);
            ReadyButton.SetActive(false);
            ReadyButtonP.SetActive(false);
            VehicleLockImage.SetActive(true);
            //WatchAdButton.SetActive(true);
        }
        //Vehicle Unlocked
        else
        {
            BuyButton.SetActive(false);
            VehicleLockImage.SetActive(false);
            WatchAdButton.SetActive(false);
            CheckForreadyButton();
            GameData.SetCurrentVehicle(currentVehicleIndex);
        }
    }
    //void Update()
    //{
    //    // Check for touch input
    //    if (Input.touchCount > 0)
    //    {
    //        // Get touch position
    //        Vector2 touchPos = Input.GetTouch(0).position;

    //        // Check if touch is in top half of screen
    //        if (touchPos.y > Screen.height / 1.8f)
    //        {
    //            // Rotate car based on touch position
    //            float rotationAmount = touchPos.x + Screen.width / 1.5f;
    //            InstantiatedVehicle.transform.Rotate(Vector3.up * rotationAmount * rotationSensitivity);
    //        }
    //    }
    //}
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Input.GetTouch(0).position;
            if (touchPos.y > Screen.height / 2.2f)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    float deltaYRotation = -touch.deltaPosition.x;
                    InstantiatedVehicle.transform.Rotate(Vector3.up * deltaYRotation * rotationSensitivity);
                }
            }
        }
    }
    public void OnReadyButtonTap()
    {
        if (GameData.GetVehicleLockStatus(currentVehicleIndex) == 1)
            GameData.SetCurrentVehicle(currentVehicleIndex);
    }

    public void OnBuyVehicle()
    {
        //Enough Coins
        if (GameData.GetTotalCoins() >= mVehiclePrices[currentVehicleIndex])
        {
            GameData.SetTotalCoins(GameData.GetTotalCoins() - mVehiclePrices[currentVehicleIndex]);
            GameData.SetVehicleLockStatus(currentVehicleIndex);
            BuyButton.SetActive(false);
            CheckForreadyButton();
            GameData.SetCurrentVehicle(currentVehicleIndex);
        }
    }
    public void ShowRewardedAd()
    {
        IronSourceAdManager.instance.ShowRewardedAd(8);
    }
    public void RewardCar()
    {
        GameData.SetTotalCoins(GameData.GetTotalCoins() - mVehiclePrices[currentVehicleIndex]);
        GameData.SetVehicleLockStatus(currentVehicleIndex);
        BuyButton.SetActive(false);
        CheckForreadyButton();
        GameData.SetCurrentVehicle(currentVehicleIndex);
        OnSelectVehicle(currentVehicleIndex);
    }
    public void OnADForCar()
    {
        GameData.SetVehicleLockStatus(currentVehicleIndex);
        BuyButton.SetActive(false);
        CheckForreadyButton();
        GameData.SetCurrentVehicle(currentVehicleIndex);
        OnSelectVehicle(currentVehicleIndex);
    }
    public void WatchRewardAD()
    {
        IronSourceAdManager.instance.ShowRewardedAd(4);
    }
}
