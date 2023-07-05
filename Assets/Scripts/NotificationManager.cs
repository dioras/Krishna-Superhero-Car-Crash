using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct NotificationData
{
    public string heading;
    public string body;
    public int daysDelay;
};
public class NotificationManager : MonoBehaviour
{
    public List<NotificationData> CustomNotifications;

    public static NotificationManager Agent;
    private void Awake()
    {
        //Don't destroy on load
        if (Agent == null)
        {
            CustomNotifications = new List<NotificationData>();
            Agent = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

#if UNITY_IOS

    private void Start()
    {
        //Register for notifications
        RequestAuthorization();

        //Cancel pending notifications
        CancelNotifications();
    }

    private IEnumerator RequestAuthorization()
    {
        var authorizationOption = Unity.Notifications.iOS.AuthorizationOption.Alert | Unity.Notifications.iOS.AuthorizationOption.Badge;
        using (var req = new Unity.Notifications.iOS.AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            //Debug.Log(res);
        }
    }

    private void CancelNotifications()
    {
        Unity.Notifications.iOS.iOSNotificationCenter.RemoveAllScheduledNotifications();
    }

    private void ScheduleNotification()
    {
        foreach (NotificationData item in CustomNotifications)
        {
            var timeTrigger = new Unity.Notifications.iOS.iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(item.daysDelay * 24, 0, 0),
                Repeats = false
            };

            var notification = new Unity.Notifications.iOS.iOSNotification()
            {
                // You can specify a custom identifier which can be used to manage the notification later.
                // If you don't provide one, a unique string will be generated automatically.
                Identifier = "_notification_01",
                Title = item.heading,
                Body = item.body,
                // Subtitle = "This is a subtitle, something, something important...",
                ShowInForeground = true,
                ForegroundPresentationOption = (Unity.Notifications.iOS.PresentationOption.Alert | Unity.Notifications.iOS.PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };
            Unity.Notifications.iOS.iOSNotificationCenter.ScheduleNotification(notification);
        }
    }

#else
    private void Start()
    {
        //Register for notifications
        RequestAuthorization();

        //Cancel pending notifications
        CancelNotifications();
    }

    private void RequestAuthorization()
    {
        var channel = new Unity.Notifications.Android.AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Unity.Notifications.Android.Importance.Default,
            Description = "Generic notifications",
        };
        Unity.Notifications.Android.AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void CancelNotifications()
    {
        Unity.Notifications.Android.AndroidNotificationCenter.CancelAllNotifications();
    }

    private void ScheduleNotifications()
    {

        foreach (NotificationData item in CustomNotifications)
        {
            var notification = new Unity.Notifications.Android.AndroidNotification();
            notification.Title = item.heading;
            notification.Text = item.body;
            notification.FireTime = System.DateTime.Now.AddDays(item.daysDelay);
            notification.SmallIcon = "icon_0";

            Unity.Notifications.Android.AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }

    }
#endif


    private void OnApplicationQuit()
    {
#if UNITY_ANDROID
        ScheduleNotifications();
#elif UNITY_IOS
        ScheduleNotification();
#endif
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
#if UNITY_ANDROID
            ScheduleNotifications();
#elif UNITY_IOS
            ScheduleNotification();
#endif
        }
        else
            CancelNotifications();
    }
}







