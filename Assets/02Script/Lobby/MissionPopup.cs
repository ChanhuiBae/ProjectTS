using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPopup : MonoBehaviour
{
    private Image mainImage;
    private Button main;
    private Button sub;

    private Sprite nonChoice;
    private Sprite choice;

    private Button outBtn;

    private GameObject missionPopup;
    private Button startBtn;

    private Button back;

    private void Awake()
    {
        if (!GameObject.Find("MainMission").TryGetComponent<Image>(out mainImage))
        {
            Debug.Log("MissionPopup - Awake - Image");
        }
        if (!GameObject.Find("MainMission").TryGetComponent<Button>(out main))
        {
            Debug.Log("MissionPopup - Awake - Button");
        }
        else
        {
            main.onClick.AddListener(MainChoice);
        }
        if (!GameObject.Find("SubMission").TryGetComponent<Button>(out sub))
        {
            Debug.Log("MissionPopup - Awake - Button");
        }
        else
        {
            sub.onClick.AddListener(EnableChoice);
        }


        nonChoice = mainImage.sprite;
        choice = Resources.Load<Sprite>("Image/Choice");


        if (!transform.Find("Out").TryGetComponent<Button>(out outBtn))
        {
            Debug.Log("MissionPopup - Awake - Button");
        }
        else
        {
            outBtn.onClick.AddListener(Out);
        }

        missionPopup = GameObject.Find("MissionPopup");
        if(!missionPopup.transform.Find("Start").TryGetComponent<Button> (out startBtn))
        {
            Debug.Log("MissionPopup - Awake - Button");
        }
        else
        {
            startBtn.onClick.AddListener(GameStart);
        }
       
        if (!missionPopup.transform.Find("Back").TryGetComponent<Button>(out back))
        {
            Debug.Log("MissionPopup - Awake - Button");
        }
        else
        {
            back.onClick.AddListener(Back);
        }
    }
    private void Start()
    {
        missionPopup.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OpenMapPopup()
    {
        mainImage.sprite = nonChoice;
        missionPopup.SetActive(false);
    }

    private void MainChoice()
    {
        mainImage.sprite = choice;
        missionPopup.SetActive(true);
        OpenMissionPopup();
    }

    private void EnableChoice()
    {
        missionPopup.gameObject.SetActive(false);
        mainImage.sprite = nonChoice;
        outBtn.gameObject.SetActive(true);
    }

    private void OpenMissionPopup()
    {
        missionPopup.SetActive(true);
    }

    private void GameStart()
    {
        if(GameManager.Inst.PlayerInfo.WeaponID != 0)
        {
            GameManager.Inst.AsyncLoadNextScene(SceneName.PlayScene);
        }
    }

    private void Back()
    {
        missionPopup.gameObject.SetActive(false);
        mainImage.sprite = nonChoice;
        outBtn.gameObject.SetActive(true);
    }

    private void Out()
    {
        gameObject.SetActive(false);
    }
}
