using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    public static FirebaseInit Agent;

    private void Awake()
    {
        Agent = this;
    }

    void Start()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        // Log an event with no parameters.
        Firebase.Analytics.FirebaseAnalytics
        .LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin);
    }

    public void LogLevelStartEvent(string LevelIndex)
    {
        // Log an event with an int parameter.
        Debug.Log("Firebase Analytics level_start_" + LevelIndex);
        Firebase.Analytics.FirebaseAnalytics
          .LogEvent("Level_Started_" + LevelIndex);
    }
    public void LogLevelFailedEvent(string LevelIndex)
    {
        // Log an event with an int parameter.
        Debug.Log("Firebase Analytics level_failed_" + LevelIndex);
        Firebase.Analytics.FirebaseAnalytics
          .LogEvent("Level_Failed_" + LevelIndex);
    }
    public void LogLevelCompletedEvent(string LevelIndex)
    {
        // Log an event with an int parameter.
        Debug.Log("Firebase Analytics level_completed_" + LevelIndex);
        Firebase.Analytics.FirebaseAnalytics
          .LogEvent("Level_Completed_"+LevelIndex);
    }
    public void OnRiderSelection(string RiderIndex)
    { // Log an event with an int parameter.
        Debug.Log("Firebase Analytics Selected_Rider_" + RiderIndex);
        Firebase.Analytics.FirebaseAnalytics
                 .LogEvent("Selected_Rider_" + RiderIndex);
    }
    public void OnBicycleSelection(string BicycleIndex)
    { // Log an event with an int parameter.
        Debug.Log("Firebase Analytics Selected_Bicycle_" + BicycleIndex);
        Firebase.Analytics.FirebaseAnalytics
                 .LogEvent("Selected_Bicycle_" + BicycleIndex);
    }

}
