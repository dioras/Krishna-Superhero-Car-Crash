using System;
using UnityEngine;

public class Preferences
{
    public static bool UnlockAllCars
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString("UnlockAllCars", bool.FalseString));
        }
        set
        {
            PlayerPrefs.SetString("UnlockAllCars", value.ToString());
        }
    }
    public static bool UnlockAllHeroes
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString("UnlockAllHeroes", bool.FalseString));
        }
        set
        {
            PlayerPrefs.SetString("UnlockAllHeroes", value.ToString());
        }
    }
    public static bool UnlockAllLevels
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString("UnlockAllLevels", bool.FalseString));
        }
        set
        {
            PlayerPrefs.SetString("UnlockAllLevels", value.ToString());
        }
    }
    public static bool NoAds
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString("NoAds", bool.FalseString));
        }
        set
        {
            PlayerPrefs.SetString("NoAds", value.ToString());
        }
    }
    public static bool UnlockEverything
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString("UnlockEverything", bool.FalseString));
        }
        set
        {
            PlayerPrefs.SetString("UnlockEverything", value.ToString());
        }
    }
    public static int TEST
    {
        get
        {
            return PlayerPrefs.GetInt("TEST", 0);
        }
        set
        {
            PlayerPrefs.SetInt("TEST", value);
        }
    }
}
