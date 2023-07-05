using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VungleScript : MonoBehaviour
{
    string appID = "";
    string iosAppID = "6421e1c4a7b7a6cf39748462";
    string androidAppID = "6421e22c0953c98c239c326d";

    private void Awake()
    {
#if UNITY_IPHONE
        appID = iosAppID;
#elif UNITY_ANDROID
    appID = androidAppID;
#endif
        Vungle.init(appID);
    }
}
