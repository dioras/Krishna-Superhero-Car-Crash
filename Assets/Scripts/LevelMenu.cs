using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LevelMenu : MonoBehaviour
{
    private int _SelectLevelIndex;
    public static LevelMenu agent;
    [SerializeField] private GameObject[] mLevels;
    [SerializeField] private Material GrayScaleMaterial;
    [SerializeField] private GameObject LoadingGUI;
    [SerializeField] private GameObject[] LevelPages;
    [SerializeField] private Image[] PageIndicator;
    [SerializeField] private Sprite InactiveSprite, ActiveSprite;
    [SerializeField] private GameObject NextPageBtn , PrevPageBtn;

    private int pageindex = 0;
    public int levelindex = 0;
    public int lastlevelIndex = 0;

    private void Awake()
    {
        if (agent == null)
            agent = this;
    }
    private void OnEnable()
    {
        FindObjectOfType<TopPanelManager>().PlayVal = 1;

        OnStart();
    }

    public void OnStart()
    {
        levelindex = (GameData.GetTotalLevelsUnlocked() - 1);
        ResetPages();
        pageindex = GameData.GetCurrentLevel() / 6;
        LevelPages[pageindex].SetActive(true);
        PageIndicator[pageindex].sprite = ActiveSprite;
        if (LevelPages[0].activeSelf)
        {
            PrevPageBtn.SetActive(false);
        }
        else if(LevelPages[LevelPages.Length - 1].activeSelf)
        {
            NextPageBtn.SetActive(false);
        }
        OnSelectLevel(levelindex);
        //for (int i = 0; i < mLevels.Length; i++)
        //{
        //    Debug.LogError("LEVELS UNLOCKED = " + GameData.GetTotalLevelsUnlocked());
        //    Debug.LogError("LEVELS UNLOCKED JJJ = " + i);

        //    if (GameData.GetTotalLevelsUnlocked() == i+1)
        //    {
        //        Debug.LogError("LEVELS UNLOCKED  = " + i);

        //        OnSelectLevel(i);
        //    }
        //}
    }
    

    public void OnSelectLevel(int index)
    {
        lastlevelIndex = _SelectLevelIndex;
        if (lastlevelIndex >= 0)
        {
            mLevels[lastlevelIndex].GetComponent<LevelObject>().UnlockedBG.SetActive(true);
            mLevels[lastlevelIndex].GetComponent<LevelObject>().SelectedLevel.SetActive(false);
        }
        _SelectLevelIndex = index;
        mLevels[_SelectLevelIndex].GetComponent<LevelObject>().UnlockedBG.SetActive(false);
        mLevels[_SelectLevelIndex].GetComponent<LevelObject>().SelectedLevel.SetActive(true);
        GameData.SetCurrentLevel(index);
    }

    public void PlayBtn()
    {
        LoadingGUI.SetActive(true);
        SceneManager.LoadScene(2);
    }

    private IEnumerator LoadAsyncScene()
    {
        yield return null;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
                asyncLoad.allowSceneActivation = true;

            yield return null;
        }
    }
    public void ResetPages()
    {
        for (int i = 0; i < LevelPages.Length; i++)
        {
            LevelPages[i].SetActive(false);
            PageIndicator[i].sprite = InactiveSprite;
        }
    }
    public void NextPage()
    {
        ResetPages();
        PrevPageBtn.SetActive(true);
        if(pageindex < LevelPages.Length-1)
        {
            pageindex++;
        }
        if(pageindex == LevelPages.Length-1)
        {
            NextPageBtn.SetActive(false);
        }
        LevelPages[pageindex].SetActive(true);
        PageIndicator[pageindex].sprite = ActiveSprite;
    }
    public void PrevPage()
    {
        ResetPages();
        NextPageBtn.SetActive(true);
        if (pageindex > 0)
        {
            pageindex--;
        }
        if (pageindex == 0)
        {
            PrevPageBtn.SetActive(false);
        }
        LevelPages[pageindex].SetActive(true);
        PageIndicator[pageindex].sprite = ActiveSprite;
    }
}
