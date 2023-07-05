using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class TestInstant : MonoBehaviour
{
    public Transform Parent;
    public LevelObject LevelObj;
    public Sprite[] RenderImages;
    public Sprite[] GreyRenderImages;

    public int j;

    // Start is called before the first frame update
    //void Start()
    //{
    //    for (int i = 0; i < 50; i++)
    //    {
    //        LevelObject lo = Instantiate(LevelObj, Parent);
    //        int temp = i + 1;
    //        lo.LevelNumber.text = "<size=" + 73 + ">" + "LEVEL " + "</size>" + "<size=" + 100 + ">" + temp.ToString("00") + "</size>";
    //        lo._levelnumnber = i;
    //        lo.gameObject.name = "LevelButton_" + temp.ToString("00");

    //        if (i < 25)
    //        {
    //            lo.RenderedImage.sprite = RenderImages[i];
    //            lo.GreyRenderedImage.sprite = GreyRenderImages[i];
    //        }
    //        else
    //        {

    //            lo.RenderedImage.sprite = RenderImages[j];
    //            lo.GreyRenderedImage.sprite = GreyRenderImages[j];
    //            j++;
    //        }

    //    }
    //}
}
