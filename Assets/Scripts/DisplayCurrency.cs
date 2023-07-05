using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCurrency : MonoBehaviour
{
    public static DisplayCurrency Agent;
    [SerializeField] private Text coinsText, starsText;

    private void Awake()
    {
        Agent = this;
        UpdateCoins();
        UpdateStars();
    }

    public void UpdateCoins()
    {
        coinsText.text = GameData.GetTotalCoins().ToString("n0");
    }

    public void UpdateStars()
    {
        starsText.text = GameData.GetTotalStars().ToString("n0")+ "/150";
    }
}
