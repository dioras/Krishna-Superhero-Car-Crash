using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    public static AspectRatio instance;
    public bool TabAspect, MobileAspect;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        CheckAspectRatio();
    }

    public void CheckAspectRatio()
    {
        //if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        //{
        //    Debug.LogError("PORTRAIT= " + Camera.main.aspect);
        //    if (Camera.main.aspect >= 0.65f && Camera.main.aspect <= 0.79f) //TAB Portrait
        //    {
        //        TabAspect = true;
        //    }
        //    else if (Camera.main.aspect >= 0.45f && Camera.main.aspect <= 0.6f) //MOBILE Prtrait
        //    {
        //        MobileAspect = true;
        //    }
        //    else //MOBILES
        //    {
        //        MobileAspect = true;
        //    }
        //}
        //else
        //{
          //  Debug.LogError("LANDSCAPE = " + Camera.main.aspect);

            if (Camera.main.aspect >= 1.2f && Camera.main.aspect <= 1.4f) //TAB Landscape
            {
                TabAspect = true;
            }
            else if (Camera.main.aspect >= 1.6f && Camera.main.aspect <= 2.25f) //MOBILE Landscape
            {
                MobileAspect = true;
            }
            else //MOBILES
            {
                MobileAspect = true;
            }
        //}
    }
}
