using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
//using MoreMountains.NiceVibrations;
using UnityEngine;

public class VehicleBehaviour : MonoBehaviour
{
    private GameStatus gameStatus;
    private Component[] particleSystems;
    public GameObject Character;
    public bool isLocked;

    public GameObject AIImage;
    private void Start()
    {
        gameStatus = GameStatus.Agent;
        GetComponent<Animator>().enabled = false;
    }

    public void AIFinishCallBack()
    {
        GetComponentInChildren<PlayerRank>()?.gameObject.SetActive(false);
        if (!gameStatus.PlayerReachedDestination)
            gameStatus.PlayerRank++;

        GetComponent<RCC_CarControllerV3>().canControl = false;
        GetComponent<RCC_CarControllerV3>().handbrakeInput = 1;

        if (GetComponent<RCC_AICarController>())
            GetComponent<RCC_AICarController>().enabled = false;
    }

    public void PlayerFinishCallBack()
    {
        //FindObjectOfType<RCC_Camera>().LoookBack();

        if (GetComponentInChildren<PlayerRank>() != null)
            GetComponentInChildren<PlayerRank>()?.gameObject.SetActive(false);

        gameStatus.PlayerReachedDestination = true;
        gameStatus.PlayerRank++;
        GetComponent<RCC_CarControllerV3>().canControl = false;
        GetComponent<RCC_CarControllerV3>().handbrakeInput = 1;

        TimerController.Agent.canRunTimer = false;
        LevelData.Agent.mDestinationParticles.SetActive(true);

        particleSystems = LevelData.Agent.mDestinationParticles.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particleSystems)
        {
            particle.Play();
        }

        GameStatus.Agent.OnLevelComplete();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Player"))
        {
            if (other.CompareTag("DeadZone"))
                gameStatus.OnLevelFailed();

            if (other.CompareTag("StuntZone"))
            {
                GameplayUIManager.Agent.BackCamera.SetActive(false);
                FindObjectOfType<RCC_Camera>().TPSLockX = false;
                StartCoroutine(EnableTPSLockX());
            }

            //TODO:STUNTS CODE

            //if (other.CompareTag("StuntZone"))
            //{
            //    if (gameObject.GetComponent<RCC_CarControllerV3>().speed > 120)
            //    {
            //        Time.timeScale = 0.3f;

            //        // int randomNum = Random.Range(0, 4);
            //        int randomNum = 1;
            //        switch (randomNum)
            //        {
            //            case 0:
            //                transform.DOLocalRotate(new Vector3(transform.localRotation.x, transform.localRotation.y, 360), 1f).SetRelative(true).SetEase(Ease.Linear).OnComplete(oncomplete);
            //                break;
            //            case 1:
            //                transform.DOLocalRotate(new Vector3(transform.localRotation.x, transform.localRotation.y, -360), 1f).SetRelative(true).SetEase(Ease.Linear).OnComplete(oncomplete);
            //                break;
            //            case 2:
            //                transform.DOLocalRotate(new Vector3(transform.localRotation.x, 360, transform.localRotation.z), 1f).SetRelative(true).SetEase(Ease.Linear).OnComplete(oncomplete);
            //                break;
            //            case 3:
            //                transform.DOLocalRotate(new Vector3(transform.localRotation.x, -360, transform.localRotation.z), 1f).SetRelative(true).SetEase(Ease.Linear).OnComplete(oncomplete);
            //                break;
            //        }
            //    }
            //}
        }

        if (gameObject.CompareTag("AI"))
        {
            if (other.CompareTag("DeadZone"))
            {
                AIImage.transform.GetChild(0).gameObject.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
    IEnumerator EnableTPSLockX()
    {
        yield return new WaitForSeconds(4f);
        FindObjectOfType<RCC_Camera>().TPSLockX = true;
        GameplayUIManager.Agent.BackCamera.SetActive(true);


    }
    public void oncomplete()
    {
        Time.timeScale = 1f;
    }

    public void StartBlinking()
    {
        //GetComponent<Animator>().enabled = true;
        //Character.SetActive(false);
        //GetComponent<Animator>().SetBool("CanBlink", true);
        //Invoke(nameof(StopBlinking), 3);
    }

    private void StopBlinking()
    {
        GetComponent<Animator>().SetBool("CanBlink", false);
        GetComponent<Animator>().enabled = false;

        StartCoroutine(EnableCharacter());
    }
    IEnumerator EnableCharacter()
    {
        yield return new WaitForSeconds(1f);
        Character.SetActive(true);
        Character.transform.GetComponent<Animator>().Play("CarSitting");
    }

    private void Update()
    {
        if (this.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.J))
            {
                transform.DOLocalRotate(new Vector3(transform.localRotation.x, 360, transform.localRotation.z), 2.5f).SetRelative(true).SetEase(Ease.Linear).OnComplete(resetTimescale);
            }
            else if (Input.GetKey(KeyCode.K))
            {
                transform.DOLocalRotate(new Vector3(transform.localRotation.x, transform.localRotation.y, 360), 2.5f).SetRelative(true).SetEase(Ease.Linear).OnComplete(resetTimescale);
            }
        }
    }

    void resetTimescale()
    {
        Time.timeScale = 1f;
    }
    public void PlayHaptics()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (GameData.Haptics)
            {
//#if UNITY_IOS
//            MMVibrationManager.TransientHaptic(0.2f, 0.2f);
//#elif UNITY_ANDROID
//                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
//#endif
            }
        }
    }
    public void PlayHapticsOnUI()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (GameData.Haptics)
            {
//#if UNITY_IOS
//            MMVibrationManager.TransientHaptic(0.5f, 0.5f);
//#elif UNITY_ANDROID
//                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
//#endif
            }
        }
    }
}
