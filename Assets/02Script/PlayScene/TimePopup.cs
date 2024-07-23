using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimePopup : MonoBehaviour
{
    private MenuManager menuManager;
    private TextMeshProUGUI countdown;
    private int time;
    public int Time
    {
        get => time;
    }

    private void Awake()
    {
        if(!transform.root.TryGetComponent<MenuManager>(out menuManager))
        {
            Debug.Log("TimePopup - Awkae - MenuManageer");
        }
        if (!GameObject.Find("GameTime").TryGetComponent<TextMeshProUGUI>(out countdown))
        {
            Debug.Log("MenuManager - Awake - TextMeshProUGUI");
        }
        time = 300;

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
        menuManager.Warning();
        gameObject.SetActive(false);
    }
}
