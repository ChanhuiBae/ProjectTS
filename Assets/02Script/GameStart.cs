using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    private Button startButton;

    private void Awake()
    {
        if(!TryGetComponent<Button>(out startButton))
        {
            Debug.Log("GameStart - Awake - Button");
        }
        else
        {
            startButton.onClick.AddListener(GoToPlay);
        }
    }

    private void GoToPlay()
    {
        GameManager.Inst.AsyncLoadNextScene(SceneName.PlayScene);
    }
}
