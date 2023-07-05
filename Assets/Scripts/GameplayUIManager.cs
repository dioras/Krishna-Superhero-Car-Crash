using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using DG.Tweening;

public class GameplayUIManager : MonoBehaviour
{
    public static GameplayUIManager Agent;
    private GameManager gameManager;
    [SerializeField] private RCC_Camera rccCamera;
    public GameObject cineCam;
    public GameObject FollowCarCam;
    public GameObject FollowCar;

    [Header("Game")]
    public Text TimerText;
    public Button ResetCarButton;
    public GameObject BackCamera;
    [Header("Vehicle Selection")]
    public GameObject InteractButton;
    public GameObject Adicon;

    [Header("Level Complete")]
    public Text LC_TimerText;
    public Image LC_StarImageFill;
    public Text LC_RankText, LC_RankRewardText, LC_TimeRewardText, LC_RestartRewardText, LC_TotalRewardText, LC_TotalRewardPopupText, LC_TotalDoubledReward;
    public GameObject Star1, Star2, Star3;

    [Header("Panels")]
    public GameObject levelCompleteScreen;
    public GameObject levelFailedScreen;
    public Text LF_TimerText;
    public GameObject pauseMenu;
    public GameObject currencyScreen;
    public GameObject IntroCameraPanel;
    public GameObject VehicleSelectionCanvas;
    public GameObject LoadingScreen;
    public GameObject GameplayUI;
    public Text KMHLabel;
    public Image NOSFill;
    public Image NOSFillJoyStick;
    [SerializeField] private GameObject[] PlayerControls;
    public RCC_MobileButtons rCC_MobileButtons;
    public RCC_UIController Buttons_GasButton, Buttons_BrakeButton, Steering_GasButton, Steering_BrakeButton, Sensor_GasButton, Sensor_BrakeButton, Slider_GasButton, Slider_BrakeButton;
    public RCC_UIJoystick Joystick_Joystick, Slider_JoyStick;
    public GameObject CommonNosButton, JoystickNosButton;
    public Image PlayerImage, AI1Image, AI2Image;
    public Sprite[] PlayerCarImages;
    public GameObject DoubleRewardPopup;
    public GameObject RepairButton;
    public GameObject NextButton;
    public bool isAnimationOver;
    [SerializeField] Vector3 tempPos;
    [SerializeField] GameObject[] Coins;
    [SerializeField] Transform CoinTarget;
    public GameObject DoubleRewardsButton;

    void Awake()
    {
        Agent = this;

        StartCoroutine(LoadScreenOff());

#if UNITY_EDITOR
        GetComponent<RCC_MobileButtons>().enabled = false;
#endif
        OnControlsChange();
    }
    IEnumerator LoadScreenOff()
    {
        yield return new WaitForSecondsRealtime(4f);
        LoadingScreen.SetActive(false);
        VehicleSelectionCanvas.SetActive(true);
    }
    private void Start()
    {
        gameManager = GameManager.Agent;
        if(GameData.GetCurrentLevel()== 19)
        {
            NextButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameManager.heroVehicle)
        {
            if (gameManager.heroVehicle.GetComponent<RCC_CarControllerV3>().CheckManualResetCar())
                ResetCarButton.interactable = true;
            else
                ResetCarButton.interactable = false;
        }
    }

    void LateUpdate()
    {
        if (gameManager.heroVehicle)
            KMHLabel.text = gameManager.heroVehicle.GetComponent<RCC_CarControllerV3>().speed.ToString("00") + "\nKMH";
    }
    [HideInInspector] public GameObject PlayerRank;
    public void OnLookBackPressed()
    {
        GameManager.Agent.CanvasRankP.SetActive(false);
        GameManager.Agent.CanvasRankAI1.SetActive(false);
        GameManager.Agent.CanvasRankAI2.SetActive(false);

        //rccCamera.RCC_InputManager_OnLookBack(true);
    }

    public void OnLookBackReleased()
    {
        //GameManager.Agent.CanvasRankP.SetActive(true);
        //GameManager.Agent.CanvasRankAI1.SetActive(true);
        //GameManager.Agent.CanvasRankAI2.SetActive(true);

        //rccCamera.RCC_InputManager_OnLookBack(false);
    }

    public void OnControlsChange()
    {
        for (int i = 0; i < PlayerControls.Length; i++)
            PlayerControls[i].SetActive(false);
        PlayerControls[GameData.GetControlIndex()].SetActive(true);

        switch (GameData.GetControlIndex())
        {
            case 0:
                //Buttons
                RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.TouchScreen;
                rCC_MobileButtons.gasButton = Buttons_GasButton;
                rCC_MobileButtons.brakeButton = Buttons_BrakeButton;
                if (!CommonNosButton.activeSelf)
                    CommonNosButton.SetActive(true);
                rCC_MobileButtons.NOSButton = CommonNosButton.GetComponent<RCC_UIController>();
                break;
            case 1:
                //Steering
                RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.SteeringWheel;
                rCC_MobileButtons.gasButton = Steering_GasButton;
                rCC_MobileButtons.brakeButton = Steering_BrakeButton;
                if (!CommonNosButton.activeSelf)
                    CommonNosButton.SetActive(true);
                rCC_MobileButtons.NOSButton = CommonNosButton.GetComponent<RCC_UIController>();
                break;
            case 2:
                //Sensor
                RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.Gyro;
                rCC_MobileButtons.gasButton = Sensor_GasButton;
                rCC_MobileButtons.brakeButton = Sensor_BrakeButton;
                if (!CommonNosButton.activeSelf)
                    CommonNosButton.SetActive(true);
                rCC_MobileButtons.NOSButton = CommonNosButton.GetComponent<RCC_UIController>();
                break;
            case 3:
                //Joystick
                RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.Joystick;
                rCC_MobileButtons.joystick = Joystick_Joystick;
                CommonNosButton.SetActive(false);
                rCC_MobileButtons.NOSButton = JoystickNosButton.GetComponent<RCC_UIController>();
                break;
            case 4:
                //Slider
                RCC_Settings.Instance.mobileController = RCC_Settings.MobileController.Joystick;
                rCC_MobileButtons.joystick = Slider_JoyStick;
                rCC_MobileButtons.gasButton = Slider_GasButton;
                rCC_MobileButtons.brakeButton = Slider_BrakeButton;
                if (!CommonNosButton.activeSelf)
                    CommonNosButton.SetActive(true);
                rCC_MobileButtons.NOSButton = CommonNosButton.GetComponent<RCC_UIController>();
                break;
        }
    }
    public void watchAD()
    {
        IronSourceAdManager.instance.ShowRewardedAd(6);
        DoubleRewardPopup.SetActive(false);
    }
    public void watchADSkip()
    {
        IronSourceAdManager.instance.ShowRewardedAd(9);
      //  DoubleRewardPopup.SetActive(false);
    }
    public void onWatchAD1000()
    {
        IronSourceAdManager.instance.ShowRewardedAd(7);
    }

    public void SkipLevel()
    {
        GameData.SetLevelLockStatus(gameManager.currentLevelIndex + 1);
        GameStatus.Agent.OnHome();
    }
    public void PlayCoinAnim()
    {
        StartCoroutine(PlayAnimation(Coins, CoinTarget));
    }
    public IEnumerator PlayAnimation(GameObject[] array, Transform targetPos)
    {
        isAnimationOver = false;

        for (int i = 0; i < array.Length; i++)
        {
            tempPos = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50));

            array[i].transform.localScale = Vector3.one;
            array[i].transform.localPosition = tempPos;
            array[i].SetActive(true);
            yield return new WaitForSeconds(0.05f);
        }

        for (int i = 0; i < array.Length; i++)
        {
            array[i].transform.DOMove(targetPos.position, 1).OnComplete(() =>
            {
                //CurrencyManager.Agent.UpdateCurrency();
            });

            array[i].transform.DOScale(0.5f, 1.5f);
            yield return new WaitForSeconds(0.05f);
        }

        GameStatus.Agent.OnRewardsDoubled();
        yield return new WaitForSeconds(1);

        for (int i = 0; i < array.Length; i++)
        {
            array[i].SetActive(false);
        }

        yield return new WaitForSeconds(0.05f);
        DoubleRewardsButton.SetActive(false);
        isAnimationOver = true;
    }
}
