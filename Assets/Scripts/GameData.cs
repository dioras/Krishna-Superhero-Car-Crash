using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public static int MenuGUIIndex = 0;

    //Default preferences
    static GameData()
    {
        //Unlock 1st Level
        SetLevelLockStatus(0);

        //Unlock 1st 3 Heroes
        SetHeroLockStatus(0);
        SetHeroLockStatus(1);
        SetHeroLockStatus(2);


        //Unlock first 3 vehicles
        SetVehicleLockStatus(0);
        SetVehicleLockStatus(1);
        SetVehicleLockStatus(2);
    }

    #region Audio
    public static void SetAudioVolume(float volume)
    {
        PlayerPrefs.SetFloat("AudioVolume", volume);
        if (AudioManager.Agent)
            AudioManager.Agent.UpdateVolume();
    }

    public static float GetAudioVolume() { return PlayerPrefs.GetFloat("AudioVolume", 1); }
    public static bool Haptics
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString("Haptics", bool.TrueString));
        }
        set
        {
            PlayerPrefs.SetString("Haptics", value.ToString());
        }
    }
    public static void SetCustomHaptics(bool value) { PlayerPrefs.SetString("CustomHaptics", value.ToString()); }

    public static string GetCustomHaptics() { return PlayerPrefs.GetString("CustomHaptics", bool.TrueString); }
    #endregion

    #region Controls
    public static void SetControlIndex(int index)
    {
        PlayerPrefs.SetInt("ControlIndex", index);
        if (GameplayUIManager.Agent)
            GameplayUIManager.Agent.OnControlsChange();
    }

    public static int GetControlIndex() { return PlayerPrefs.GetInt("ControlIndex", 0); }
    #endregion

    #region Levels
    public static int TotalLevels = 50;

    public static int GetTotalLevelsUnlocked()
    {
        int TotalMissionsUnlocked = 0;
        for (int i = 0; i < TotalLevels; i++)
        {
            if (GetLevelLockStatus(i) == 1)
                TotalMissionsUnlocked++;
        }

        return TotalMissionsUnlocked;
    }
    public static void SetLevelLockStatus(int index) { PlayerPrefs.SetInt("LevelLock_" + index, 1); }

    public static int GetLevelLockStatus(int index) { return PlayerPrefs.GetInt("LevelLock_" + index, 0); }

    public static void SetCurrentLevel(int index) { PlayerPrefs.SetInt("CurrentLevel", index); }

    public static int GetCurrentLevel() { return PlayerPrefs.GetInt("CurrentLevel", 0); }
    #endregion

    #region Vehicles
    public static int TotalVehicles = 14;

    public static void SetVehicleLockStatus(int index)
    {
        PlayerPrefs.SetInt("VehicleLock_" + index, 1);
        //if (CustomGameAnalyticsManager.Agent)
        //    CustomGameAnalyticsManager.Agent.UpdateVehicleProgress(index);
    }

    public static int GetVehicleLockStatus(int index) { return PlayerPrefs.GetInt("VehicleLock_" + index, 0); }

    public static int GetHighestUnlockedVehicle()
    {
        int highestUnlockedVehicle = 0;
        for (int i = 0; i < TotalVehicles; i++)
        {
            if (GetVehicleLockStatus(i) == 1)
                highestUnlockedVehicle = i;
        }
        return highestUnlockedVehicle;
    }

    public static void SetCurrentVehicle(int index) { PlayerPrefs.SetInt("CurrentVehicle", index); }

    public static int GetCurrentVehicle() { return PlayerPrefs.GetInt("CurrentVehicle", 0); }
    #endregion

    #region Superheroes
    public static int TotalHeroes = 10;

    public static void SetHeroLockStatus(int index)
    {
        PlayerPrefs.SetInt("HeroLock_" + index, 1);
        //if (CustomGameAnalyticsManager.Agent)
        //    CustomGameAnalyticsManager.Agent.UpdateHeroProgress(index);
    }

    public static int GetHeroLockStatus(int index) { return PlayerPrefs.GetInt("HeroLock_" + index, 0); }

    public static int GetTotalUnlockedHeroes()
    {
        int unlockedHeroes = 0;
        for (int i = 0; i < TotalHeroes; i++)
            unlockedHeroes = unlockedHeroes + GetHeroLockStatus(i);
        return unlockedHeroes;
    }

    public static void SetCurrentHero(int index) { PlayerPrefs.SetInt("CurrentHero", index); }

    public static int GetCurrentHero() { return PlayerPrefs.GetInt("CurrentHero", 0); }
    #endregion

    #region Coins
    public static void SetTotalCoins(int tempCoins)
    {
        PlayerPrefs.SetInt("TotalCoins", tempCoins);
        if (DisplayCurrency.Agent)
            DisplayCurrency.Agent.UpdateCoins();
    }

    public static int GetTotalCoins() { return PlayerPrefs.GetInt("TotalCoins", 0); }
    #endregion

    #region Stars
    public static int GetTotalStars()
    {
        int TotalStars = 0;
        for (int i = 0; i < TotalLevels; i++)
            TotalStars += GetLevelStars(i);
        return TotalStars;
    }

    public static void SetLevelStars(int levelIndex, int tempStars)
    {
        PlayerPrefs.SetInt("LevelStars_" + levelIndex, tempStars);
        if (DisplayCurrency.Agent)
            DisplayCurrency.Agent.UpdateStars();
    }

    public static int GetLevelStars(int levelIndex) { return PlayerPrefs.GetInt("LevelStars_" + levelIndex, 0); }
    #endregion
}
