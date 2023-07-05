using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class LoadingBG : MonoBehaviour
{
    public GameObject BG1, BG2;
    public RectTransform BG2Image;

    private void Awake()
    {
        //int temp = UnityEngine.Random.Range(0, 2);
        //if(temp == 0)
        //{
        //    BG1.SetActive(true);
        //    BG2.SetActive(false);
        //}
        //else if(temp == 1)
        //{
        //    BG2.SetActive(true);
        //    BG1.SetActive(false);
        //}
        BG2.SetActive(true);
        BG1.SetActive(false);
        StartCoroutine(LateStart());

    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.01f);

        if (BG2.activeInHierarchy)
        {
            if (AspectRatio.instance.TabAspect) //TAB
            {
                BG2Image.localScale = new UnityEngine.Vector3(1.42f, 1.42f, 1.42f);
            }
            else if (AspectRatio.instance.MobileAspect) //MOBILE
            {
                BG2Image.localScale = new UnityEngine.Vector3(1.25f, 1.25f, 1.25f);
            }
            else
            {
                BG2Image.localScale = new UnityEngine.Vector3(1.25f, 1.25f, 1.25f);
            }
        }
    }
}
