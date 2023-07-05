using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Agent;
    public GameObject[] vehicles;
    public Transform PlayerInstantiationPos, AI1InstantiationPos, AI2InstantiationPos;
    public int currentLevelIndex, currentVehicleIndex;
    [HideInInspector] public GameObject heroVehicle, aiVehicle1, aiVehicle2;
    public List<GameObject> tempVehiclesList, aiVehiclesList;
    private LevelData levelData;
    private TimerController timerController;
    [SerializeField] private Camera rccCamera;
    [SerializeField] private RCC_Camera rccCameraRoot;
    private bool EnableAI;

    public GameObject[] AI_1_CharacterList;
    public GameObject[] AI_2_CharacterList;
    public Animator AI_1_Avatar;
    public Animator AI_2_Avatar;

    private int _AI1Characterval;
    private int _AI2Characterval;
    [HideInInspector] public float PlayerDistance, AI1Distance, AI2Distance, TargetDistance;
    private bool DestinationReached = false;
    [HideInInspector] private float PlayerPercent, AI1Percent, AI2Percent;
    [SerializeField] private GameObject DestinationObject;

    public GameObject Canvas_Rank;

    public GameObject LevelFinishCamera;
    public GameObject ExplosionParticles;

    bool tempLockedVehicle = true;
    public List<int> RandomCarlist = new();

    private void Awake()
    {
        Agent = this;

#if !UNITY_EDITOR
        RCC_Settings.Instance.mobileControllerEnabled = true;

        currentLevelIndex = GameData.GetCurrentLevel();
        currentVehicleIndex = GameData.GetCurrentVehicle();
#endif
       currentLevelIndex = GameData.GetCurrentLevel();
       // GameData.SetCurrentLevel(currentLevelIndex);

        currentVehicleIndex = GameData.GetCurrentVehicle();
        StartCoroutine(LoadAsyncScene());
    }
    AsyncOperation asyncLoad;

    private IEnumerator LoadAsyncScene()
    {
        yield return null;
        if (currentLevelIndex > 24)
        {
            asyncLoad = SceneManager.LoadSceneAsync((currentLevelIndex-25) + 3, LoadSceneMode.Additive);

        }
        else
        {
            asyncLoad = SceneManager.LoadSceneAsync(currentLevelIndex + 3, LoadSceneMode.Additive);
        }
        while (!asyncLoad.isDone)
            yield return null;

        StartGameManager();
    }

    private void StartGameManager()
    {
        tempVehiclesList = new List<GameObject>();
        aiVehiclesList = new List<GameObject>();
        levelData = LevelData.Agent;
        timerController = TimerController.Agent;

        InstantiateVehicles();
    }
    private void InstantiateVehicles()
    {
        StartCoroutine(SetAICharcaterValues());
        GameObject Vehicle1 = Instantiate(vehicles[currentVehicleIndex], PlayerInstantiationPos.transform.position, Quaternion.identity);
        Vehicle1.name = vehicles[currentVehicleIndex].name;

        Vehicle1.SetActive(true);
        Vehicle1.GetComponent<RCC_CarControllerV3>().canControl = false;
        aiVehiclesList.Add(Vehicle1);
        for (int i = 0; i < vehicles.Length; i++)
        {
            if (GameData.GetVehicleLockStatus(i) == 1)
                tempVehiclesList.Add(vehicles[i]);
            //else if(tempLockedVehicle)
            //{
            //    tempVehiclesList.Add(vehicles[i]);
            //    tempLockedVehicle = false;
            //}
        }
        Vehicle1.GetComponent<VehicleBehaviour>().isLocked = false;
        tempVehiclesList.Remove(vehicles[currentVehicleIndex]);

        int RandomIndex1 = Random.Range(0, tempVehiclesList.Count);
        GameObject Vehicle2 = Instantiate(tempVehiclesList[RandomIndex1], AI1InstantiationPos.transform.position, Quaternion.identity);
        Vehicle2.name = tempVehiclesList[RandomIndex1].name;
        tempVehiclesList.RemoveAt(RandomIndex1);
        Vehicle2.SetActive(true);
        Vehicle2.GetComponent<RCC_CarControllerV3>().canControl = false;
        aiVehiclesList.Add(Vehicle2);
        Vehicle2.GetComponent<VehicleBehaviour>().isLocked = false;

        for (int i = 0; i < vehicles.Length; i++)
        {
            if (GameData.GetVehicleLockStatus(i) == 0)
            { 
                Debug.Log("Test");
                RandomCarlist.Add(i);
            }
            else if(Preferences.UnlockEverything)
            {
                RandomCarlist.Add(5);
            }
        }
        if(RandomCarlist != null)
        {
            int ListSize = RandomCarlist.Count;
            int Randomcar = RandomCarlist[Random.Range(0, ListSize - 1)];
            GameObject Vehicle3 = Instantiate(vehicles[Randomcar], AI2InstantiationPos.transform.position, Quaternion.identity);
            Vehicle3.name = vehicles[Randomcar].name;
            //tempVehiclesList.RemoveAt(i);
            Vehicle3.SetActive(true);
            Vehicle3.GetComponent<RCC_CarControllerV3>().canControl = false;
            Vehicle3.GetComponent<VehicleBehaviour>().isLocked = true;
            aiVehiclesList.Add(Vehicle3);
            tempLockedVehicle = false;
        }

    }
    [HideInInspector] public GameObject CanvasRankP, CanvasRankAI1, CanvasRankAI2;
    public void SetupVehicles(GameObject PlayerVehicle)
    {
        heroVehicle = PlayerVehicle;
        heroVehicle.GetComponent<RCC_CarControllerV3>().canControl = false;
        GameplayUIManager.Agent.VehicleSelectionCanvas.SetActive(false);
         CanvasRankP = Instantiate(Canvas_Rank, heroVehicle.transform);
        CanvasRankP.GetComponent<PlayerRank>().PlayerB = true;
        CanvasRankP.SetActive(false);

        SetupCars(0);
        rccCamera.enabled = true;
        rccCameraRoot.GetComponent<RCC_Camera>().cameraTarget.playerVehicle = heroVehicle.GetComponent<RCC_CarControllerV3>();
        //rccCameraRoot.GetComponent<RCC_Camera>().cameraTarget.playerVehicle = GameplayUIManager.Agent.FollowCar;

        heroVehicle.GetComponent<VehicleBehaviour>().Character = heroVehicle.transform.Find("Hero").gameObject;
        aiVehiclesList.Remove(PlayerVehicle);

        aiVehicle1 = aiVehiclesList[0];
        aiVehicle1.tag = "AI";
         CanvasRankAI1=  Instantiate(Canvas_Rank, aiVehicle1.transform);
        CanvasRankAI1.GetComponent<PlayerRank>().AI1 = true;
        CanvasRankAI1.SetActive(false);

        aiVehicle1.AddComponent<RCC_AICarController>();
        aiVehicle1.GetComponent<RCC_AICarController>().navigationMode = RCC_AICarController.NavigationMode.TargetPostion;
        aiVehicle1.GetComponent<RCC_AICarController>().raycastAngle = 70f;
        aiVehicle1.GetComponent<RCC_AICarController>().raycastLength = 30f;
        aiVehicle1.GetComponent<RCC_AICarController>().waypointsContainer = levelData.aiWaypointsContainer1;
        aiVehicle1.GetComponent<RCC_CarControllerV3>().canControl = true;
        AI1CharacterSetup();

        aiVehicle2 = aiVehiclesList[1];
        aiVehicle2.tag = "AI";
         CanvasRankAI2 = Instantiate(Canvas_Rank, aiVehicle2.transform);
        CanvasRankAI2.GetComponent<PlayerRank>().AI2 = true;
        CanvasRankAI2.SetActive(false);

        aiVehicle2.AddComponent<RCC_AICarController>();
        aiVehicle2.GetComponent<RCC_AICarController>().navigationMode = RCC_AICarController.NavigationMode.TargetPostion;
        aiVehicle2.GetComponent<RCC_AICarController>().raycastAngle = 70f;
        aiVehicle2.GetComponent<RCC_AICarController>().raycastLength = 30f;
        aiVehicle2.GetComponent<RCC_AICarController>().waypointsContainer = levelData.aiWaypointsContainer2;
        aiVehicle2.GetComponent<RCC_CarControllerV3>().canControl = true;
        AI2CharacterSetup();

        GameplayUIManager.Agent.GameplayUI.SetActive(true);
        EnableAI = true;
        heroVehicle.GetComponent<RCC_CarControllerV3>().canControl = true;

        DestinationObject = FindObjectOfType<FinishTrigger>().gameObject;
        TargetDistance = Vector3.Distance(heroVehicle.transform.position, DestinationObject.transform.position);
    }

    IEnumerator SetAICharcaterValues()
    {
        while (_AI1Characterval == GameData.GetCurrentHero())
        {
            _AI1Characterval = Random.Range(0, AI_1_CharacterList.Length);
        }

        yield return new WaitForSecondsRealtime(0.1f);

        while (_AI2Characterval == GameData.GetCurrentHero() || _AI2Characterval == _AI1Characterval)
        {
            _AI2Characterval = Random.Range(0, AI_2_CharacterList.Length);
        }

    }
    public void AI1CharacterSetup()
    {
        AI_1_CharacterList[_AI1Characterval].SetActive(true);
        AI_1_CharacterList[_AI1Characterval].transform.parent.SetParent(aiVehicle1.transform, true);
        AI_1_Avatar.avatar = FindObjectOfType<VehicleSelectionManager>().heroAvatars[0];

        Transform sittingpos = aiVehicle1.transform.Find("CarSittingPos").transform;
        AI_1_CharacterList[_AI1Characterval].transform.parent.position = sittingpos.position;
        AI_1_CharacterList[_AI1Characterval].transform.parent.rotation = sittingpos.rotation;
        AI_1_CharacterList[_AI1Characterval].transform.parent.localScale = sittingpos.localScale;
        SetupCars(1);

        aiVehicle1.GetComponent<VehicleBehaviour>().Character = AI_1_CharacterList[_AI1Characterval].transform.parent.gameObject;
        aiVehicle1.GetComponent<VehicleBehaviour>().AIImage = GameplayUIManager.Agent.AI1Image.gameObject;

        aiVehicle1.GetComponent<RCC_CarControllerV3>().canControl = false;
    }

    public void AI2CharacterSetup()
    {
        AI_2_CharacterList[_AI2Characterval].SetActive(true);
        AI_2_CharacterList[_AI2Characterval].transform.parent.SetParent(aiVehicle2.transform, true);
        AI_2_Avatar.avatar = FindObjectOfType<VehicleSelectionManager>().heroAvatars[0];
        Transform sittingpos = aiVehicle2.transform.Find("CarSittingPos").transform;
        AI_2_CharacterList[_AI2Characterval].transform.parent.position = sittingpos.position;
        AI_2_CharacterList[_AI2Characterval].transform.parent.rotation = sittingpos.rotation;
        AI_2_CharacterList[_AI2Characterval].transform.parent.localScale = sittingpos.localScale;
        SetupCars(2);
        aiVehicle2.GetComponent<VehicleBehaviour>().Character = AI_2_CharacterList[_AI2Characterval].transform.parent.gameObject;
        aiVehicle2.GetComponent<VehicleBehaviour>().AIImage = GameplayUIManager.Agent.AI2Image.gameObject;
        aiVehicle2.GetComponent<RCC_CarControllerV3>().canControl = false;

    }
    public void SetupCars(int val)
    {
        switch (val)
        {
            case 0:
                for (int i = 0; i < GameplayUIManager.Agent.PlayerCarImages.Length; i++)
                {
                    if (heroVehicle.name == GameplayUIManager.Agent.PlayerCarImages[i].name)
                    {
                        GameplayUIManager.Agent.PlayerImage.sprite = GameplayUIManager.Agent.PlayerCarImages[i];
                    }
                }
                
                break;
            case 1:
                for (int i = 0; i < GameplayUIManager.Agent.PlayerCarImages.Length; i++)
                {
                    if (aiVehicle1.name == GameplayUIManager.Agent.PlayerCarImages[i].name)
                    {
                        GameplayUIManager.Agent.AI1Image.sprite = GameplayUIManager.Agent.PlayerCarImages[i];

                    }
                }
                break;
            case 2:
                for (int i = 0; i < GameplayUIManager.Agent.PlayerCarImages.Length; i++)
                {
                    if (aiVehicle2.name == GameplayUIManager.Agent.PlayerCarImages[i].name)
                    {
                        GameplayUIManager.Agent.AI2Image.sprite = GameplayUIManager.Agent.PlayerCarImages[i];

                    }
                }
                break;

        }
        

    }
    IEnumerator DelayPlayerRank()
    {
        yield return new WaitForSecondsRealtime(3f);
        CanvasRankP.SetActive(true);
        CanvasRankAI1.SetActive(true);
        CanvasRankAI2.SetActive(true);
    }
    private void Update()
    {
        if (EnableAI)
        {
            if (heroVehicle.GetComponent<RCC_CarControllerV3>().speed > 5)
            {
                EnableAI = false;
                StartAIVehicles();
               StartCoroutine(DelayPlayerRank());
            }
        }
        if (DestinationReached == false)
        {
            if (heroVehicle)
            {
                PlayerDistance = Vector3.Distance(heroVehicle.transform.position, DestinationObject.transform.position);
                PlayerPercent = (TargetDistance - PlayerDistance) / TargetDistance;
                //Debug.Log(PlayerPercent + "Player percent");
                GameplayUIManager.Agent.PlayerImage.transform.localPosition = new Vector3(PlayerPercent * 1200f, GameplayUIManager.Agent.PlayerImage.transform.localPosition.y, GameplayUIManager.Agent.PlayerImage.transform.localPosition.z);
                if (PlayerPercent > 0.95f)
                {
                    DestinationReached = true;
                    GameplayUIManager.Agent.PlayerImage.transform.localPosition = new Vector3(0.97f * 1200f, GameplayUIManager.Agent.PlayerImage.transform.localPosition.y, GameplayUIManager.Agent.PlayerImage.transform.localPosition.z);
                }
            }
            if (aiVehicle1)
            {
                AI1Distance = Vector3.Distance(aiVehicle1.transform.position, DestinationObject.transform.position);
                AI1Percent = (TargetDistance - AI1Distance) / TargetDistance;
                GameplayUIManager.Agent.AI1Image.transform.localPosition = new Vector3(AI1Percent * 1200f, GameplayUIManager.Agent.AI1Image.transform.localPosition.y, GameplayUIManager.Agent.AI1Image.transform.localPosition.z);
            }
            if (aiVehicle2)
            {
                AI2Distance = Vector3.Distance(aiVehicle2.transform.position, DestinationObject.transform.position);
                AI2Percent = (TargetDistance - AI2Distance) / TargetDistance;
                GameplayUIManager.Agent.AI2Image.transform.localPosition = new Vector3(AI2Percent * 1200f, GameplayUIManager.Agent.AI2Image.transform.localPosition.y, GameplayUIManager.Agent.AI2Image.transform.localPosition.z);
            }

        }

    }

    public void StartAIVehicles()
    {
        heroVehicle.GetComponent<RCC_CarControllerV3>().canControl = true;
        aiVehicle1.GetComponent<RCC_CarControllerV3>().canControl = true;
        aiVehicle2.GetComponent<RCC_CarControllerV3>().canControl = true;

        timerController.canRunTimer = true;
    }

    public void OnResetVehicle()
    {
        heroVehicle.GetComponent<RCC_CarControllerV3>().ManualResetCar();
    }
    public void RepairCarAd()
    {
        IronSourceAdManager.instance.ShowRewardedAd(10);
    }
    public void OnRepairVehicle()
    {
        heroVehicle.GetComponent<RCC_CarControllerV3>().Repair();
    }
}
