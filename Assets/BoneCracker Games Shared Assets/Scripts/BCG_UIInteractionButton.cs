//----------------------------------------------
//            BCG Shared Assets
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BCG_UIInteractionButton : MonoBehaviour, IPointerClickHandler {

    public static BCG_UIInteractionButton agent;
    public GameObject adicon;
    private void Awake()
    {
        agent = this;
    }
    private void Start()
    {
        adicon = GameplayUIManager.Agent.Adicon;
    }
    public void OnPointerClick(PointerEventData eventData) {

#if BCG_ENTEREXIT

        if(adicon.activeInHierarchy)
        {
            IronSourceAdManager.instance.ShowRewardedAd(11);
        }
        else
        {
            if (BCG_EnterExitManager.Instance.activePlayer != null)
                BCG_EnterExitManager.Instance.Interact();
        }

#endif

    }
    public void afterAd()
    {
        if (BCG_EnterExitManager.Instance.activePlayer != null)
            BCG_EnterExitManager.Instance.Interact();
    }
}
