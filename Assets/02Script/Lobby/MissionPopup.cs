using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPopup : MonoBehaviour
{
    private LobbySceneManager lobby;

    private Image mainImage;
    private Button main;
    private Button sub;

    private Sprite nonChoice;
    private Sprite choice;

    private Button outBtn;

    private GameObject missionPopup;
    private Button receive;

    private TextMeshProUGUI mission1;
    private TextMeshProUGUI planet;
    private TextMeshProUGUI misstionObject;
    private TextMeshProUGUI creature;
    private TextMeshProUGUI DNA;
    private TextMeshProUGUI EXP;
    private TextMeshProUGUI description;

    private Button back;

    private void Awake()
    {
        if(!GameObject.Find("LobbySceneManager").TryGetComponent<LobbySceneManager>(out lobby))
        {
            Debug.Log("MissionPopup - Awake - LobbySceneManager");
        }

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


        if (!TryGetComponent<Button>(out outBtn))
        {
            Debug.Log("MissionPopup - Awake - Button");
        }
        else
        {
            outBtn.onClick.AddListener(Out);
        }

        missionPopup = GameObject.Find("MissionPopup");
        if(!GameObject.Find("Receive").TryGetComponent<Button> (out receive))
        {
            Debug.Log("MissionPopup - Awake - Button");
        }
        else
        {
            receive.onClick.AddListener(OutStartPopup);
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

    private void OutStartPopup()
    {
        lobby.SetStart();
        lobby.OpenStartPopup();
        gameObject.SetActive(false);
    }
}
