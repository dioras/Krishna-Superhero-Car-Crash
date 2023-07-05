using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IntroCameraManager : MonoBehaviour
{
    private LevelData levelData;
    private GameplayUIManager gameplayUIManager;
    public Camera cam;
    public GameObject CharacterSelectionScript;
    [HideInInspector] public bool skip;

    private void Start()
    {
        levelData = LevelData.Agent;
        gameplayUIManager = GameplayUIManager.Agent;

        gameplayUIManager.IntroCameraPanel.SetActive(true);
        StartCoroutine(PlaySequenceAnim());
    }

    IEnumerator PlaySequenceAnim()
    {
        for (int i = 0; i < levelData.cameraPositions.Length; i++)
        {
            if (skip)
            {
                cam.transform.position = levelData.cameraPositions[3].position;
                cam.transform.rotation = levelData.cameraPositions[3].rotation;
                enableHeroCamera();
                break;
            }
            else
            {
                cam.transform.position = levelData.cameraPositions[i].position;
                cam.transform.rotation = levelData.cameraPositions[i].rotation;
                switch (i)
                {
                    case 0:
                        cam.gameObject.GetComponent<CameraAnimation>().MoveUp();
                        break;
                    case 1:
                        DOTween.Kill("MOVEUP");
                        cam.gameObject.GetComponent<CameraAnimation>().MoveRight();
                        break;
                    case 2:
                        DOTween.Kill("MOVERIGHT");
                        cam.gameObject.GetComponent<CameraAnimation>().MoveLeft();
                        break;
                }

                if (i == 3)
                {
                    cam.gameObject.GetComponent<CameraAnimation>().KillAnim();
                    skip = true;
                    enableHeroCamera();
                }
                else
                {
                    yield return new WaitForSeconds(4f);
                }
            }
        }
    }

    public void skipAnim()
    {
        skip = true;
    }

    void enableHeroCamera()
    {
        cam.gameObject.GetComponent<CameraAnimation>().KillAnim();
        cam.transform.DOMove(levelData.cameraPositions[4].position, 3f);
        cam.transform.DORotateQuaternion(levelData.cameraPositions[4].rotation, 3f);
        gameplayUIManager.IntroCameraPanel.SetActive(false);
        //        gameplayUIManager.characterSelectionUI.SetActive(true);
    }
}
