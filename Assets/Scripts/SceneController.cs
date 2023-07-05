using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadSceneWithDelay());
    }


    IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(1);
    }

}
