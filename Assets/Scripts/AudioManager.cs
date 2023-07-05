using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Agent;
    public AudioSource LevelComplete, LevelFailed;
    public GameObject CameraMob, CameraTab;
    private void Awake()
    {
        Agent = this;
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        AudioListener.volume = GameData.GetAudioVolume();
        StartCoroutine(cheskAspect());
    }

    IEnumerator cheskAspect()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Menu")
        {
            if (AspectRatio.instance.TabAspect)
            {
                CameraMob.SetActive(false);
                CameraTab.SetActive(true);
            }
            else
            {
                CameraTab.SetActive(false);
                CameraMob.SetActive(true);
            }
        }
        yield return null;
    }
}
