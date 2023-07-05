using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NSubstitute;

public class ShopManager : MonoBehaviour
{
    //public GameObject PurchasedImageUnlockEverything;

    public GameObject BuyBtnUnlockEverything;

    public GameObject PurchasedBuyImageBtn;
    public DOTweenAnimation UnlockAllHeroes;
    private void OnEnable()
    {
       // BuyUnlockAllLevels();
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (Preferences.UnlockEverything)
        {
            PurchasedBuyImageBtn.SetActive(true);
            BuyBtnUnlockEverything.GetComponent<Button>().interactable = false;
        }


        DisplayCurrency DC = FindObjectOfType<DisplayCurrency>();

        if (DC != null)
            DC.UpdateCoins();


    }
    public void UnlockEverything()
    {
        //Preferences.NoAds = true;
        //for (int i = 0; i < 15; i++)
        //{
        //    GameData.SetVehicleLockStatus(i);
        //}
        //Preferences.UnlockAllCars = true;
       
        //for (int i = 0; i< 10; i++)
        //{
        //    GameData.SetHeroLockStatus(i);
        //}
        //Preferences.UnlockAllHeroes = true;
       // PurchasedBuyImageBtn.SetActive(true);
        //BuyBtnUnlockEverything.GetComponent<Button>().interactable = false;
       // PurchaseManager.Agent.BuyProduct_UnlockEverything();

    }
    public void BuyUnlockAllLevels()
    {
        for (int i = 0; i < 50; i++)
        {
            GameData.SetLevelLockStatus(i);
        }
        Preferences.UnlockAllLevels = true;
      //  PurchaseManager.Agent.BuyProduct_UnlockAllLevelss();
    }
}
