using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private Image ultimateFill;
    private TextMeshProUGUI ultimateText;
    private TextMeshProUGUI killCount;

    private void Awake()
    {
        if (!GameObject.Find("UltimateFill").TryGetComponent<Image>(out ultimateFill))
        {
            Debug.Log("PlayerController - Awake - Image");
        }
        if (!GameObject.Find("UltimateValue").TryGetComponent<TextMeshProUGUI>(out ultimateText))
        {
            Debug.Log("PlayerController - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("KillCount").TryGetComponent<TextMeshProUGUI>(out killCount))
        {
            Debug.Log("PlayerController - Awake - TextMeshProUGUI");
        }
        ultimateFill.fillAmount = 0;
        ultimateText.text = "0%";
    }

    public void SetUaltimate(int value)
    {
        ultimateFill.fillAmount = (float)value / 100f;
        ultimateText.text = value.ToString() + "%";
    }

    public void SetKillCount(int value)
    {
        killCount.text = value.ToString();
    }
}
