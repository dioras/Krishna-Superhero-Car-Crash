using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSelectionManager : MonoBehaviour
{
    [SerializeField] private Animator heroAnimator;
    [SerializeField] public Avatar[] heroAvatars;
    [SerializeField] private GameObject[] heroObjects;

    private void Start()
    {
        for (int i = 0; i < heroObjects.Length; i++)
            heroObjects[i].SetActive(false);
        StartCoroutine(ActiveteHeroWithDelay());
    }

    IEnumerator ActiveteHeroWithDelay()
    {
        yield return new WaitForSecondsRealtime(3.5f);
        heroAnimator.gameObject.SetActive(true);

        heroObjects[GameData.GetCurrentHero()].SetActive(true);
        heroAnimator.avatar = heroAvatars[GameData.GetCurrentHero()];
    }
}
