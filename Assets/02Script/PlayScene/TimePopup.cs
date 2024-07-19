using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimePopup : MonoBehaviour
{
    private TextMeshProUGUI countdown;
    private int time;
    public int Time
    {
        get => time;
    }

    private void Awake()
    {
        if (!GameObject.Find("GameTime").TryGetComponent<TextMeshProUGUI>(out countdown))
        {
            Debug.Log("MenuManager - Awake - TextMeshProUGUI");
        }
        time = 600;

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (time > 0)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
            time--;
            countdown.text = (time / 60).ToString() + ":" + (time % 60).ToString();
        }
        countdown.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
