//using System;
//using UnityEngine;
//using UnityEngine.UI;
////using UnityEngine.Purchasing;
//using UnityEngine.SceneManagement;
//using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.Analytics;
//using Unity.Services.Core;
//using Unity.Services.Core.Environments;

//public class PurchaseManager : MonoBehaviour, IStoreListener
//{
//    public static PurchaseManager Agent;

//    private static IStoreController m_StoreController;
//    private static IExtensionProvider m_StoreExtensionProvider;

//    private static readonly string mPrefixProduct = "superherocarcrash_";

//    public static string NonConsumable_UnlockEverything = mPrefixProduct + "unlockeverything";

//    public string environment = "production";

//    private void Awake()
//    {
//        Agent = this;
//    }

//    async void Start()
//    {
//        try
//        {
//            var options = new InitializationOptions()
//                .SetEnvironmentName(environment);

//            await UnityServices.InitializeAsync(options);
//        }
//        catch (Exception)
//        {
//            // An error occurred during services initialization.
//        }

//        if (m_StoreController == null)
//            InitializePurchasing();
//    }

//    public void InitializePurchasing()
//    {
//        if (IsInitialized())
//            return;

//        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
//        builder.AddProduct(NonConsumable_UnlockEverything, ProductType.NonConsumable);

//        UnityPurchasing.Initialize(this, builder);
//    }

//    private bool IsInitialized()
//    {
//        return m_StoreController != null && m_StoreExtensionProvider != null;
//    }

//    public void BuyProduct_UnlockEverything() { BuyProductID(NonConsumable_UnlockEverything); }

//    /// <summary>
//    /// End of Public Methods
//    /// </summary>

//    void BuyProductID(string productId)
//    {
//        //Time.timeScale = 1f;
//        if (IsInitialized())
//        {
//            Product product = m_StoreController.products.WithID(productId);

//            if (product != null && product.availableToPurchase)
//            {
//                //  Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
//                IronSourceAdManager.instance.EnablePurchaseLoadingPopUp();
//                m_StoreController.InitiatePurchase(product);
//            }
//            else
//            {
//                IronSourceAdManager.instance.EnablePurchaseFailedPopUp();

//                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//            }
//        }
//        else
//        {
//            IronSourceAdManager.instance.EnablePurchaseFailedPopUp();
//            Debug.Log("BuyProductID FAIL. Not initialized.");
//        }
//        //  Time.timeScale = 0f;

//    }


//    public void RestorePurchases()
//    {
//        if (!IsInitialized())
//        {
//            Debug.Log("RestorePurchases FAIL. Not initialized.");
//            return;
//        }

//        // If we are running on an Apple device ... 
//        if (Application.platform == RuntimePlatform.IPhonePlayer ||
//            Application.platform == RuntimePlatform.OSXPlayer)
//        {
//            Debug.Log("RestorePurchases started ...");

//            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
//            apple.RestoreTransactions((result) =>
//            {
//                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//            });
//        }
//        else
//        {
//            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
//        }
//    }


//    //
//    // --- IStoreListener
//    //

//    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//    {
//        Debug.Log("OnInitialized: PASS");

//        m_StoreController = controller;
//        m_StoreExtensionProvider = extensions;
//    }


//    public void OnInitializeFailed(InitializationFailureReason error)
//    {
//        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//    }


//    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//    {
//        if (String.Equals(args.purchasedProduct.definition.id, NonConsumable_UnlockEverything, StringComparison.Ordinal))
//        {
//            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//            Preferences.NoAds = true;
//            for (int i = 0; i < 15; i++)
//            {
//                GameData.SetVehicleLockStatus(i);
//            }
//            Preferences.UnlockAllCars = true;
//            Preferences.UnlockEverything = true;
//            for (int i = 0; i < 10; i++)
//            {
//                GameData.SetHeroLockStatus(i);
//            }
//            Preferences.UnlockAllHeroes = true;
//        }
//        else
//        {
//            IronSourceAdManager.instance.DisblePurchaseLoadingPopUp();
//            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
//        }
//        ShopManager SM = FindObjectOfType<ShopManager>();
//        if (SM != null)
//        {
//            SM.UpdateUI();
//        }
//        var price = args.purchasedProduct.metadata.localizedPrice;
//        double lPrice = decimal.ToDouble(price);
//        var currencyCode = args.purchasedProduct.metadata.isoCurrencyCode;

//        var wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(args.purchasedProduct.receipt);

//        if (null == wrapper)
//        {
//            return PurchaseProcessingResult.Pending;
//        }

//        var payload = (string)wrapper["Payload"]; // For Apple this will be the base64 encoded ASN.1 receipt
//        var productId = args.purchasedProduct.definition.id;
//        IronSourceAdManager.instance.DisblePurchaseLoadingPopUp();
//        return PurchaseProcessingResult.Complete;
//    }

//    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//    {
//        IronSourceAdManager.instance.DisblePurchaseLoadingPopUp();

//        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
//    }
//}