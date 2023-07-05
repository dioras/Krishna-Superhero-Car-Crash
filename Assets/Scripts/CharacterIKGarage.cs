using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIKGarage : MonoBehaviour
{
    [SerializeField] private GameObject CharacterParent;
    [SerializeField] private GameObject[] CharacterList;
    [SerializeField] private Avatar[] HeroAvatars;
    [SerializeField] private Animator Avatar;
    [SerializeField] private Transform MainParent;
    private int Characterval;
    public static CharacterIKGarage instance;
    private void Awake()
    {
        if(instance == null)
        instance = this;
    }
    public void UnParentHeroFromCar()
    {
        CharacterParent.transform.SetParent(MainParent);
    }
    public void SetupCharacter(Transform cartransform, int CharacterIndex)
    {
        for(int i =0; i < CharacterList.Length; i++)
        {
            if (CharacterList[i].activeInHierarchy)
            CharacterList[i].SetActive(false);
        }
        Characterval = CharacterIndex;
        CharacterList[Characterval].SetActive(true);
        if (cartransform != null)
        {
            Transform sittingpos = cartransform.Find("CarSittingPos").transform;
            CharacterList[Characterval].transform.parent.SetParent(cartransform, true);
            CharacterList[Characterval].transform.parent.position = sittingpos.position;
            CharacterList[Characterval].transform.parent.rotation = sittingpos.rotation;
            CharacterList[Characterval].transform.parent.localScale = sittingpos.localScale;
        }

        Avatar.avatar = HeroAvatars[Characterval];
        Avatar.enabled = true;

        
    }
}
