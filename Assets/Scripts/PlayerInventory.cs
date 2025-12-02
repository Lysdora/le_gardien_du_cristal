using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public TextMeshProUGUI gemmeCountText;
    int gemmeCount = 0;

    public int GetGemmeCount()
    {
        return gemmeCount;
    }

    public void RemoveGemme(int amount)
    {
        gemmeCount -= amount;
        if (gemmeCount < 0)
            gemmeCount = 0;
        Debug.Log("Gemme retirée ! Total : " + gemmeCount);
        gemmeCountText.text = gemmeCount.ToString();
    }

    public void AddGemme()
    {
        gemmeCount++;
        Debug.Log("Gemme ajoutée ! Total : " + gemmeCount);
        gemmeCountText.text = gemmeCount.ToString();
    }

}
