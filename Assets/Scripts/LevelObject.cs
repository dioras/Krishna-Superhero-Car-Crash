using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelObject : MonoBehaviour
{
    public Text LevelNumber;
    public GameObject[] Stars;
    public GameObject LockedBG;
    public GameObject UnlockedBG;
    public GameObject SelectedLevel;
    public Image RenderedImage;
    public Image GreyRenderedImage;
    public int _levelnumnber;

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() => ButtonClicked(_levelnumnber));
        SelectedLevel.SetActive(false);
        if (GameData.GetLevelLockStatus(_levelnumnber) == 1)
        {
            UpdateLevelLockStatus();
            UpdateStars();
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }

    }

    public void UpdateStars()
    {
        switch (GameData.GetLevelStars(_levelnumnber))
        {
            case 0:
                Stars[0].SetActive(false);
                Stars[1].SetActive(false);
                Stars[2].SetActive(false);
                break;

            case 1:
                Stars[0].SetActive(true);
                Stars[1].SetActive(false);
                Stars[2].SetActive(false);
                break;

            case 2:
                Stars[0].SetActive(true);
                Stars[1].SetActive(true);
                Stars[2].SetActive(false);
                break;

            case 3:
                Stars[0].SetActive(true);
                Stars[1].SetActive(true);
                Stars[2].SetActive(true);
                break;
        }
    }

    public void UpdateLevelLockStatus()
    {
        if (GameData.GetLevelLockStatus(_levelnumnber) == 0) //0 = locked; 1 = Unlocked
        {
            LockedBG.SetActive(true);
            //GreyRenderedImage.gameObject.SetActive(true);
            UnlockedBG.SetActive(false);
        }
        else if (GameData.GetTotalLevelsUnlocked() - 1 == _levelnumnber)
        {
            SelectedLevel.SetActive(true);
            UnlockedBG.SetActive(false);
            LockedBG.SetActive(false);
            LevelNumber.color = Color.red;
        }
        else
        {
            GetComponent<Button>().interactable = true;
            transform.GetChild(3).gameObject.SetActive(true);
              LockedBG.SetActive(false);
            //GreyRenderedImage.gameObject.SetActive(false);
            UnlockedBG.SetActive(true);
            LevelNumber.color = Color.green;
        }
    }
    void ButtonClicked(int buttonNo)
    {
        //LevelMenu.agent.ResetSelectedLevels();
        //Output this to console when the Button3 is clicked
        Debug.Log("Button clicked = " + buttonNo);
        SelectedLevel.SetActive(true);
        UnlockedBG.SetActive(false);
        LevelMenu.agent.OnSelectLevel(buttonNo);
    }
}
