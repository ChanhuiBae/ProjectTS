using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartPopup : MonoBehaviour
{
    private LobbySceneManager lobby;
    private TextMeshProUGUI mission;
    private Image weaponIcon;
    private Button start;
    private Button cancel;

    private void Awake()
    {
        if(!GameObject.Find("LobbySceneManager").TryGetComponent<LobbySceneManager>(out lobby))
        {
            Debug.Log("StartPopup - Awake - LobbySceneManager");
        }

        if(!transform.Find("Start").TryGetComponent<Button>(out start))
        {
            Debug.Log("StartPopup - Awake - Button");
        }
        else
        {
            start.onClick.AddListener(MissionStart);
        }

        if (!transform.Find("Cancel").TryGetComponent<Button>(out cancel))
        {
            Debug.Log("StartPopup - Awake - Button");
        }
        else
        {
            cancel.onClick.AddListener(CloseStartPopup);
        }

        if (!transform.Find("MissionName").TryGetComponent<TextMeshProUGUI>(out mission))
        {
            Debug.Log("StartPopup - Awake - TextMeshProUGUI");
        }

        if (!transform.Find("WeaponIcon").TryGetComponent<Image>(out weaponIcon))
        {
            Debug.Log("StartPopup - Awake - Image");
        }
        
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenStartPopup()
    {
        if(GameManager.Inst.PlayerInfo.WeaponID >= 3000)
        {
            weaponIcon.sprite = Resources.Load<Sprite>("Image/PR_Basic");
        }
        else
        {
            weaponIcon.sprite = Resources.Load<Sprite>("Image/BH_Basic");
        }
    }

    private void CloseStartPopup()
    {
        lobby.SetStart(false);
        gameObject.SetActive(false);
    }

    private void MissionStart()
    {
        GameManager.Inst.AsyncLoadNextScene(SceneName.PlayScene);
    }
}
