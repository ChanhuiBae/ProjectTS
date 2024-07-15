using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class ArmoryPopup : MonoBehaviour
{
    private LobbySceneManager lobby;

    private Image weaponImage;
    private Button weapon;
    private Button armor;
    private Button create;
    private Button enforce;
    private Sprite nonChoice;
    private Sprite choice;

    private Button outBtn;

    private GameObject weaponPopup;

    private Button hammer;
    private Button rifle;
    private Image hammerImage;
    private Image rifleImage;
    private Sprite dontUse;
    private Sprite use;

    private Button weaponBtn;
    private List<Image> weaponsIcon;
    private List<Image> outlines;
    private Sprite thinOutline;
    private Sprite choiceOutline;

    private Image icon;
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
        if (!GameObject.Find("Armor").TryGetComponent<Button>(out armor))
        {
            Debug.Log("ArmoryPopup - Awake - Button");
        }
        else
        {
            armor.onClick.AddListener(EnableChoice);
        }
        if (!GameObject.Find("Create").TryGetComponent< Button>(out create))
        {
            Debug.Log("ArmoryPopup - Awake - Button");
        }
        else
        {
            armor.onClick.AddListener(EnableChoice);
        }
        if (!GameObject.Find("Enforce").TryGetComponent<Button>(out enforce))
        {
            Debug.Log("ArmoryPopup - Awake - Button");
        }
        else
        {
            armor.onClick.AddListener(EnableChoice);
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

        weaponsIcon = new List<Image>();
        outlines = new List<Image>();
        GameObject content  = GameObject.Find("Content");
        if (content != null)
        {
            foreach (Transform t in content.transform)
            {
                outlines.Add(t.Find("outline").GetComponent<Image>());
                weaponsIcon.Add(t.Find("Weapon").GetComponent<Image>());
            }
        }
        thinOutline = Resources.Load<Sprite>("Image/thinOutline");
        choiceOutline = Resources.Load<Sprite>("Image/Outline");
        if (!content.transform.GetChild(0).TryGetComponent<Button>(out weaponBtn))
        {
            Debug.Log("ArmoryPopup - Awake - Button");
        }
        else
        {
            weaponBtn.onClick.AddListener(ClickWeaponIcon);
        }


        if (!GameObject.Find("Hammer").TryGetComponent<Button>(out hammer))
        {
            Debug.Log("ArmoryPopup - Awake - Button");
        }
        else
        {
            hammer.onClick.AddListener(SelectHammer);
        }
        if (!GameObject.Find("Rifle").TryGetComponent<Button>(out rifle))
        {
            Debug.Log("ArmoryPopup - Awake - Button");
        }
        else
        {
            rifle.onClick.AddListener(SelectRifle);
        }
        if (!GameObject.Find("Hammer").TryGetComponent<Image>(out hammerImage))
        {
            Debug.Log("ArmoryPopup - Awake - Image");
        }
        if (!GameObject.Find("Rifle").TryGetComponent<Image>(out rifleImage))
        {
            Debug.Log("ArmoryPopup - Awake - Image");
        }
        use = hammerImage.sprite;
        dontUse = rifleImage.sprite;


        if (!GameObject.Find("SelectIcon").TryGetComponent<Image>(out icon))
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

    private void EnableChoice()
    {
        weaponPopup.gameObject.SetActive(false);
        weaponImage.sprite = nonChoice;
        outBtn.gameObject.SetActive(true);
    }

    private void OpenWeaponPopup()
    {
        outBtn.enabled = false;
        int data = GameManager.Inst.PlayerInfo.WeaponID;
        if(data >= 3000)
        {
            rifleImage.sprite = use;
            hammerImage.sprite = dontUse;
            weaponsIcon[0].sprite = Resources.Load<Sprite>("Image/PR_Basic_Image");

            icon.sprite = weaponsIcon[0].sprite;
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
            outlines[0].sprite = choiceOutline;
        }
        else if(data >= 2000)
        {
            rifleImage.sprite = dontUse;
            hammerImage.sprite = use;
            weaponsIcon[0].sprite = Resources.Load<Sprite>("Image/BH_Basic_Image");
            icon.sprite = weaponsIcon[0].sprite;
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
            outlines[0].sprite = choiceOutline;
        }
        else
        {
            rifleImage.sprite = dontUse;
            hammerImage.sprite = use;
            outlines[0].sprite = thinOutline;
        }
    }

    private void SelectHammer()
    {
        rifleImage.sprite = dontUse;
        hammerImage.sprite = use;
        weaponsIcon[0].sprite = Resources.Load<Sprite>("Image/BH_Basic_Image");
        int data = GameManager.Inst.PlayerInfo.WeaponID;
        if (data >= 3000)
        {
            outlines[0].sprite = thinOutline;
        }
        else if (data >= 2000)
        {
            outlines[0].sprite = choiceOutline;
        }
    }

    private void SelectRifle()
    {
        rifleImage.sprite = use;
        hammerImage.sprite = dontUse;
        weaponsIcon[0].sprite = Resources.Load<Sprite>("Image/PR_Basic_Image");
        int data = GameManager.Inst.PlayerInfo.WeaponID;
        if (data>= 3000)
        {
            outlines[0].sprite = choiceOutline;
        }
        else if (data >= 2000)
        {
            outlines[0].sprite = thinOutline;
        }
    }

    public void ClickWeaponIcon()
    {
        outlines[0].sprite = choiceOutline;
        if (weaponsIcon[0].sprite == Resources.Load<Sprite>("Image/PR_Basic_Image"))
        {
            GameManager.Inst.SetGun();
        }
        else
        {
            GameManager.Inst.SetHammer();
        }
        OpenWeaponPopup();
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
