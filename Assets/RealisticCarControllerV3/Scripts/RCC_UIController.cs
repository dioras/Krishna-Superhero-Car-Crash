//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2022 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;
/// <summary>
/// UI input (float) receiver from UI Button. 
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/RCC UI Controller Button")]
public class RCC_UIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private Button button;
    private Slider slider;
    public Image NOS;
    public DOTweenAnimation NOSAnim;
    internal float input;
    private float sensitivity { get { return RCC_Settings.Instance.UIButtonSensitivity; } }
    private float gravity { get { return RCC_Settings.Instance.UIButtonGravity; } }
    public bool pressing;

    void Awake() {

        button = GetComponent<Button>();
        slider = GetComponent<Slider>();

    }

    public void OnPointerDown(PointerEventData eventData) {

        pressing = true;
        if(NOS!= null){
            FindObjectOfType<VehicleBehaviour>().PlayHaptics();
        }

    }

    public void OnPointerUp(PointerEventData eventData) {

        if (NOS != null && pressing == true)
        {
            FindObjectOfType<VehicleBehaviour>().PlayHaptics();
        }
        pressing = false;
       
    }

    void OnPress(bool isPressed) {

        if (isPressed)
            pressing = true;
        else
            pressing = false;

    }

    void LateUpdate() {
        if (NOS != null)
        {
            if (NOS.fillAmount == 1 && NOSAnim != null)
            {
                NOSAnim.DOPlay();
                NOSAnim.enabled = true;
            }
            else if (NOSAnim.enabled == true && NOS.fillAmount != 1)
            {
                NOSAnim.DORewind();
                NOSAnim.DOPause();
                NOSAnim.enabled = false;
            }
        }


        if (button && !button.interactable) {

            pressing = false;
            input = 0f;
            return;

        }

        if (slider && !slider.interactable) {

            pressing = false;
            input = 0f;
            slider.value = 0f;
            return;

        }

        if (slider) {

            if (pressing)
                input = slider.value;
            else
                input = 0f;

            slider.value = input;

        } else {

            if (pressing)
                input += Time.deltaTime * sensitivity;
            else
                input -= Time.deltaTime * gravity;

        }

        if (input < 0f)
            input = 0f;

        if (input > 1f)
            input = 1f;

    }

    void OnDisable() {

        input = 0f;
        pressing = false;

    }

}
