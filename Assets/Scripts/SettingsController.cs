using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider AudioSlider;
    [SerializeField] private Toggle HapticToggle;
    [SerializeField] private Button[] ControlButtons;
    [SerializeField] private GameObject[] ControlsButtonSelected;
    [SerializeField] private Sprite ControlNormalSprite, ControlHighlightedSprite;
    [SerializeField] private GameObject[] graphicsQuality;

    private void Awake()
    {
        //Volume Slider
        AudioSlider.value = GameData.GetAudioVolume();
        AudioListener.volume = AudioSlider.value;
        //Haptics
        if (GameData.Haptics)
            HapticToggle.isOn = true;
        else
            HapticToggle.isOn = false;

        //Controls
        for (int i = 0; i < ControlButtons.Length; i++)
            ControlButtons[i].image.sprite = ControlNormalSprite;
        ControlButtons[GameData.GetControlIndex()].image.sprite = ControlHighlightedSprite;
        if(QualitySettings.GetQualityLevel()==0)
        {
            GraphicQualit(0);
        }
        else if (QualitySettings.GetQualityLevel() == 3)
        {
            GraphicQualit(1);
        }
        else
        {
            GraphicQualit(2);
        }
    }

    public void OnVolumeChange()
    {
        GameData.SetAudioVolume(AudioSlider.value);
        AudioListener.volume = AudioSlider.value;

    }

    public void OnHapticChange()
    {
        GameData.Haptics = HapticToggle.isOn;
       // GameData.SetCustomHaptics(HapticToggle.isOn);
    }

    public void OnControlSelect(int index)
    {
        for (int i = 0; i < ControlButtons.Length; i++)
            ControlsButtonSelected[i].SetActive(false);
        ControlsButtonSelected[index].SetActive(true);
            //ControlButtons[i].image.sprite = ControlNormalSprite;
        //ControlButtons[index].image.sprite = ControlHighlightedSprite;
        GameData.SetControlIndex(index);
    }

    public void CommonURL(string link) { Application.OpenURL(link); }

    public void GraphicQualit(int index)
    {
        foreach (GameObject QS in graphicsQuality)
        {
            QS.SetActive(false);
        }
        graphicsQuality[index].SetActive(true);
        switch (index)
        {
            case 1:
                QualitySettings.SetQualityLevel(0);
                break;
            case 2:
                QualitySettings.SetQualityLevel(2);
                break;
            case 3:
                QualitySettings.SetQualityLevel(5);
                break;
            default:
                break;
        }    }

}
