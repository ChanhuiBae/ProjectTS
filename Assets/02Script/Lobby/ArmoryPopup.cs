using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class ArmoryPopup : MonoBehaviour
{
    private LobbySceneManager lobby;

    private Image weaponImage;
    private Button weapon;
    private Sprite nonChoice;
    private Sprite choice;

    private Button outBtn;

    private GameObject weaponPopup;

    private Image hammerImage;
    private Image rifleImage;

    private Button hammer;
    private Button rifle;

    private Image BH_line;
    private Image PR_line;
    private Sprite thinOutline;
    private Sprite choiceOutline;

    private TextMeshProUGUI weaponName;
    private TextMeshProUGUI attack;
    private TextMeshProUGUI critical;
    private TextMeshProUGUI fire;
    private TextMeshProUGUI water;
    private TextMeshProUGUI thunder;
    private TextMeshProUGUI ice;
    private TextMeshProUGUI wind;

    private Button back;

    private void Awake()
    {
        if(!GameObject.Find("LobbySceneManager").TryGetComponent<LobbySceneManager>(out lobby))
        {
            Debug.Log("ArmoryPopup - Awake - LobbySceneManaer");
        }

        if(!GameObject.Find("WeaponCategory").TryGetComponent<Image>(out weaponImage))
        {
            Debug.Log("ArmoryPopup - Awake - Image");
        }
        if(!GameObject.Find("WeaponCategory").TryGetComponent<Button>(out weapon))
        {
            Debug.Log("ArmoryPopup - Awake - Button");
        }
        else
        {
            weapon.onClick.AddListener(WeaponChoice);
        }


        nonChoice = weaponImage.sprite;
        choice = Resources.Load<Sprite>("Image/Choice");

                
        if (!TryGetComponent<Button>(out outBtn))
        {
            Debug.Log("ArmoryPopup - Awake - Button");
        }
        else
        {
           outBtn.onClick.AddListener(Out);
        }

        weaponPopup = GameObject.Find("WeaponPopup");


        thinOutline = Resources.Load<Sprite>("Image/thinOutline");
        choiceOutline = Resources.Load<Sprite>("Image/Outline");


        if (!GameObject.Find("WeaponIcon1").TryGetComponent<Button>(out hammer))
        {
            Debug.Log("ArmoryPopup - Awake - Button");
        }
        else
        {
            hammer.onClick.AddListener(SelectHammer);
        }
        if(!hammer.transform.Find("outline").TryGetComponent<Image>(out BH_line))
        {
            Debug.Log("ArmoryPopup - Awake - Image");
        }
        if (!GameObject.Find("WeaponIcon2").TryGetComponent<Button>(out rifle))
        {
            Debug.Log("ArmoryPopup - Awake - Button");
        }
        else
        {
            rifle.onClick.AddListener(SelectRifle);
        }
        if (!rifle.transform.Find("outline").TryGetComponent<Image>(out PR_line))
        {
            Debug.Log("ArmoryPopup - Awake - Image");
        }
        if (!GameObject.Find("SelectIcon").TryGetComponent<Image>(out hammerImage))
        {
            Debug.Log("ArmoryPopup - Awake - Image");
        }
        if (!GameObject.Find("UnSelectedIcon").TryGetComponent<Image>(out rifleImage))
        {
            Debug.Log("ArmoryPopup - Awake - Image");
        }


        if (!GameObject.Find("Name").TryGetComponent<TextMeshProUGUI>(out weaponName))
        {
            Debug.Log("ArmoryPopup - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("AttackValue").TryGetComponent<TextMeshProUGUI>(out attack))
        {
            Debug.Log("ArmoryPopup - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("CriticalValue").TryGetComponent<TextMeshProUGUI>(out critical))
        {
            Debug.Log("ArmoryPopup - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("Fire").TryGetComponent<TextMeshProUGUI>(out fire))
        {
            Debug.Log("ArmoryPopup - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("Water").TryGetComponent<TextMeshProUGUI>(out water))
        {
            Debug.Log("ArmoryPopup - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("Thunder").TryGetComponent<TextMeshProUGUI>(out thunder))
        {
            Debug.Log("ArmoryPopup - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("Ice").TryGetComponent<TextMeshProUGUI>(out ice))
        {
            Debug.Log("ArmoryPopup - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("Wind").TryGetComponent<TextMeshProUGUI>(out wind))
        {
            Debug.Log("ArmoryPopup - Awake - TextMeshProUGUI");
        }
        if (!weaponPopup.transform.Find("Back").TryGetComponent<Button>(out back))
        {
            Debug.Log("ArmoryPopup - Awake - Button");
        }
        else
        {
            back.onClick.AddListener(Back);
        }
    }

    private void Start()
    {
        weaponPopup.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OpenArmorPopup()
    {
        GameManager.Inst.PlayerIsController(false);
        lobby.CloseStartPopup();
        weaponImage.sprite = nonChoice;
        weaponPopup.SetActive(false);
    }

    private void WeaponChoice()
    {
        weaponImage.sprite = choice;
        weaponPopup.SetActive(true);
        OpenWeaponPopup();
    }

    private void OpenWeaponPopup()
    {
        outBtn.enabled = false;
        int data = GameManager.Inst.PlayerInfo.WeaponID;
        if(data >= 3000)
        {
            rifleImage.enabled = true;
            hammerImage.enabled = false;

            TableEntity_Weapon weapon;
            GameManager.Inst.GetRifle(data, out weapon);
            weaponName.text = weapon.Name.ToString();
            attack.text = weapon.Physics_Type + "\n" + weapon.Physics;
            critical.text = weapon.Critical_Chance + "%\n" + weapon.Critical_Mag + "%";
            fire.text = weapon.Fire.ToString();
            water.text = weapon.Water.ToString();
            thunder.text = weapon.Electric.ToString();
            ice.text = weapon.Ice.ToString();
            wind.text = weapon.Wind.ToString();
            BH_line.sprite = thinOutline; 
            PR_line.sprite = choiceOutline;
        }
        else if(data >= 2000)
        {
            rifleImage.enabled = false;
            hammerImage.enabled = true;

            TableEntity_Weapon weapon;
            GameManager.Inst.GetHammer(data, out weapon);
            weaponName.text = weapon.Name.ToString();
            attack.text = weapon.Physics_Type + "\n" + weapon.Physics;
            critical.text = weapon.Critical_Chance + "%\n" + weapon.Critical_Mag +"%";
            fire.text = weapon.Fire.ToString();
            water.text = weapon.Water.ToString();
            thunder.text = weapon.Electric.ToString();
            ice.text = weapon.Ice.ToString();
            wind.text = weapon.Wind.ToString();
            BH_line.sprite = choiceOutline;
            PR_line.sprite = thinOutline;
        }
    }

    private void SelectHammer()
    {
        GameManager.Inst.SetHammer();
        rifleImage.enabled = false;
        hammerImage.enabled = true;

        TableEntity_Weapon weapon;
        int data = GameManager.Inst.PlayerInfo.WeaponID;
        GameManager.Inst.GetHammer(data, out weapon);
        weaponName.text = weapon.Name.ToString();
        attack.text = weapon.Physics_Type + "\n" + weapon.Physics;
        critical.text = weapon.Critical_Chance + "%\n" + weapon.Critical_Mag + "%";
        fire.text = weapon.Fire.ToString();
        water.text = weapon.Water.ToString();
        thunder.text = weapon.Electric.ToString();
        ice.text = weapon.Ice.ToString();
        wind.text = weapon.Wind.ToString();
        BH_line.sprite = choiceOutline;
        PR_line.sprite = thinOutline;

    }

    private void SelectRifle()
    {
        GameManager.Inst.SetGun();
        rifleImage.enabled = true;
        hammerImage.enabled = false;

        TableEntity_Weapon weapon;
        int data = GameManager.Inst.PlayerInfo.WeaponID;
        GameManager.Inst.GetRifle(data, out weapon);
        weaponName.text = weapon.Name.ToString();
        attack.text = weapon.Physics_Type + "\n" + weapon.Physics;
        critical.text = weapon.Critical_Chance + "%\n" + weapon.Critical_Mag + "%";
        fire.text = weapon.Fire.ToString();
        water.text = weapon.Water.ToString();
        thunder.text = weapon.Electric.ToString();
        ice.text = weapon.Ice.ToString();
        wind.text = weapon.Wind.ToString();
        BH_line.sprite = thinOutline;
        PR_line.sprite = choiceOutline;
    }

    private void Back()
    {
        weaponPopup.gameObject.SetActive(false);
        weaponImage.sprite = nonChoice;
        outBtn.enabled = true;
    }

    private void Out()
    {
        GameManager.Inst.PlayerIsController(true);
        lobby.OpenStartPopup();
        gameObject.SetActive(false);
    }
}
