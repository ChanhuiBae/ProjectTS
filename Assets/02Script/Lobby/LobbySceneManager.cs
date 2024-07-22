using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Interaction_Type
{
    None,
    Armory,
    Mission
}
public class LobbySceneManager : MonoBehaviour
{ 
    private Interaction_Type interaction;
    private Button interBtn;
    private Image namePopup;
    private TextMeshProUGUI nameText;
    private ArmoryPopup armory;
    private MissionPopup map;
    private StartPopup start;
    private bool haveMission;

    private void Awake()
    {
        if (!GameObject.Find("ArmoryPopup").TryGetComponent<ArmoryPopup>(out armory))
        {
            Debug.Log("LobbySceneManager - Awake - ArmoryPopup");
        }
        if (!GameObject.Find("MapPopup").TryGetComponent<MissionPopup>(out map))
        {
            Debug.Log("LobbySceneManager - Awake - MissionPopup");
        }
        if(!GameObject.Find("StartPopup").TryGetComponent<StartPopup>(out start))
        {
            Debug.Log("LobbySceneManager - Awake - StartPopup");
        }
        if (!GameObject.Find("NamePopup").TryGetComponent<Button>(out interBtn))
        {
            Debug.Log("LobbySceneManager - Awake - Button");
        }
        else
        {
            interBtn.onClick.AddListener(InteracteType);
        }
        if(!GameObject.Find("NamePopup").TryGetComponent<Image>(out namePopup))
        {
            Debug.Log("LobbySceneManager - Awake - Image");
        }
        if(!namePopup.transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out nameText))
        {
            Debug.Log("LobbySceneManager - Awake - TextMeshProUGUI");
        }
        interaction = Interaction_Type.None;
        interBtn.gameObject.SetActive(false);
        namePopup.enabled = false;
        nameText.enabled = false;
        haveMission = false;
    }

    public void SetInteracte(Interaction_Type type, bool use)
    {
        interaction = type;
        nameText.enabled = use;
        namePopup.enabled = use;
        interBtn.gameObject.SetActive(use);
        switch (interaction)
        {
            case Interaction_Type.Armory:
                nameText.text = "무기고";
                break;
            case Interaction_Type.Mission:
                nameText.text = "우주 지도";
                break;
        }
    }

    private void InteracteType()
    {
        switch (interaction)
        {
            case Interaction_Type.Armory:
                armory.gameObject.SetActive(true);
                armory.OpenArmorPopup();
                break;
            case Interaction_Type.Mission:
                map.gameObject.SetActive(true);
                map.OpenMapPopup();
                break;
        }
        GameManager.Inst.PlayerIsController(false);
    }

    public void SetStart(bool have)
    {
        haveMission = have;
    }

    public void OpenStartPopup()
    {
        if(haveMission)
        {
            start.gameObject.SetActive(true);
            start.OpenStartPopup();
        }
    }

    public void CloseStartPopup()
    {
        start.gameObject.SetActive(false);
    }
}
