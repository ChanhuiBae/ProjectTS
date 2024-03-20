using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private Button pause;
    private bool isPause;

    private void Awake()
    {
        if(!GameObject.Find("Pause").TryGetComponent<Button>(out pause))
        {
            Debug.Log("MenuManager - Awake - Button");
        }
        else
        {
            isPause = false;
            pause.onClick.AddListener(Pause);
        }
    }

    private void Pause()
    {
        isPause = !isPause;
        if(isPause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
