using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private GameObject timeBackgroun;
    private TextMeshProUGUI countdown;
    private int time;
    private TextMeshProUGUI killCount;
    private SkillButton basic;
    private SkillButton skill1;
    private SkillButton skill2;
    private SkillButton skill3;
    private UltimateButton ultimate;

    private Image passive1;
    private Image passive2;
    private Image passive3;

    private int maxUltimateValue;
    private float currentUltimateValue;

    private TextMeshProUGUI level;

    private GameObject levelupPopup;
    private Choice selection1;
    private Choice selection2;
    private Choice selection3;
    private Button selectBtn;
    private int choice;
    private List<int> UsingSkills;

    private Button pause;
    private Image current;
    private Sprite pauseImage;
    private GameObject pausePopup;
    private Button play;
    private Button setting;
    private Button exit;

    private GameObject settingPopup;
    private Button settingBack;
    private Slider bgm;
    private Slider sfx;
    private Toggle damageText;
    private Toggle camShake;
    private Button setSkill1;
    private Image icon1;
    private Button setSkill2;
    private Image icon2;
    private Button setSkill3;
    private Image icon3;
    private int select;

    private GameObject exitPopup;
    private Button exitButton;
    private Button notexitButton;

    private PlayerController player;

    private void Awake()
    {
        QualitySettings.shadowDistance = 55;
        timeBackgroun = GameObject.Find("TimeBackground");
        if (!GameObject.Find("GameTime").TryGetComponent<TextMeshProUGUI>(out countdown))
        {
            Debug.Log("MenuManager - Awake - TextMeshProUGUI");
        }
        time = 900;
        
        if (!GameObject.Find("KillCount").TryGetComponent<TextMeshProUGUI>(out killCount))
        {
            Debug.Log("MenuManager - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("BasicAttack").TryGetComponent<SkillButton>(out basic))
        {
            Debug.Log("PlayerController - Awake - Button");
        }

        if (!GameObject.Find("Skill1B").TryGetComponent<SkillButton>(out skill1))
        {
            Debug.Log("MenuManager - Awake - SkillButton");
        }
        if (!GameObject.Find("Skill2B").TryGetComponent<SkillButton>(out skill2))
        {
            Debug.Log("MenuManager - Awake - SkillButton");
        }
        if (!GameObject.Find("Skill3B").TryGetComponent<SkillButton>(out skill3))
        {
            Debug.Log("MenuManager - Awake - SkillButton");
        }
        if (!GameObject.Find("UltimateB").TryGetComponent<UltimateButton>(out ultimate))
        {
            Debug.Log("MenuManager - Awake - UltimateButton");
        }
        if (!GameObject.Find("Passive1I").TryGetComponent<Image>(out passive1))
        {
            Debug.Log("MenuManager - Awake - Image");
        }
        if (!GameObject.Find("Passive2I").TryGetComponent<Image>(out passive2))
        {
            Debug.Log("MenuManager - Awake - Image");
        }
        if (!GameObject.Find("Passive3I").TryGetComponent<Image>(out passive3))
        {
            Debug.Log("MenuManager - Awake - Image");
        }


        if (!GameObject.Find("Level").TryGetComponent<TextMeshProUGUI>(out level))
        {
            Debug.Log("MenuManager - Awake - TextMeshProUGUI");
        }

        levelupPopup = GameObject.Find("LevelUpPopup");
        if(levelupPopup != null)
        {
            if(!levelupPopup.transform.Find("Selection1").TryGetComponent<Choice>(out selection1))
            {
                Debug.Log("MenuManager - Awake - Choice");
            }
            if (!levelupPopup.transform.Find("Selection2").TryGetComponent<Choice>(out selection2))
            {
                Debug.Log("MenuManager - Awake - Choice");
            }
            if (!levelupPopup.transform.Find("Selection3").TryGetComponent<Choice>(out selection3))
            {
                Debug.Log("MenuManager - Awake - Choice");
            }
            if(!levelupPopup.transform.Find("Select").TryGetComponent<Button>(out selectBtn))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                selectBtn.onClick.AddListener(Select);
            }
            levelupPopup.SetActive(false);
        }
        UsingSkills = new List<int>();

        if (!GameObject.Find("Pause").TryGetComponent<Button>(out pause))
        {
            Debug.Log("MenuManager - Awake - Button");
        }
        else
        {
            pause.onClick.AddListener(PressPause);
        }
        if (!GameObject.Find("Pause").TryGetComponent<Image>(out current))
        {
            Debug.Log("MenuManager - Awake - Image");
        }
        pauseImage = Resources.Load<Sprite>("Image/Pasue");
        if (pauseImage == null)
        {
            Debug.Log("MenuManager - Awake - Resources Image");
        }
        pausePopup = GameObject.Find("PausePopup");
        if (pausePopup != null)
        {
            if (!pausePopup.transform.Find("Play").TryGetComponent<Button>(out play))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                play.onClick.AddListener(OnPlay);
            }
            if (!pausePopup.transform.Find("Setting").TryGetComponent<Button>(out setting))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                setting.onClick.AddListener(OnSetting);
            }
            if (!pausePopup.transform.Find("Exit").TryGetComponent<Button>(out exit))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                exit.onClick.AddListener(OnExit);
            }
        }
        settingPopup = GameObject.Find("SettingPopup");
        if (settingPopup != null)
        {
            if (!settingPopup.transform.Find("Back").TryGetComponent<Button>(out settingBack))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                settingBack.onClick.AddListener(OnSettingBack);
            }
            if(!settingPopup.transform.Find("Bgm").TryGetComponent<Slider>(out bgm))
            {
                Debug.Log("MenuManager - Awake - Slider");
            }
            else
            {
                bgm.minValue = -40;
                bgm.maxValue = 0;
                bgm.onValueChanged.AddListener(delegate { SetVolumBGM(); });
            }
            if (!settingPopup.transform.Find("Sfx").TryGetComponent<Slider>(out sfx))
            {
                Debug.Log("MenuManager - Awake - Slider");
            }
            else
            {
                sfx.minValue = -40;
                sfx.maxValue = 0;
                sfx.onValueChanged.AddListener(delegate { SetVolumSFX(); });
            }
            if(!settingPopup.transform.Find("DamageText").TryGetComponent<Toggle>(out damageText))
            {
                Debug.Log("MenuManager - Awake - Toggle");
            }
            else
            {
                damageText.onValueChanged.AddListener(delegate { SetDamageText(); });
            }
            if (!settingPopup.transform.Find("Shake").TryGetComponent<Toggle>(out camShake))
            {
                Debug.Log("MenuManager - Awake - Toggle");
            }
            else
            {
                camShake.onValueChanged.AddListener(delegate { SetCameraShake(); });
            }
            GameObject obj = settingPopup.transform.Find("SetSkill1").gameObject;
            if (!obj.TryGetComponent<Button>(out setSkill1))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                setSkill1.onClick.AddListener(delegate { ChangeSkillPos(1); });
            }
            if(!obj.TryGetComponent<Image>(out icon1))
            {
                Debug.Log("MenuManager - Awake - Image");
            }
            obj = settingPopup.transform.Find("SetSkill2").gameObject;
            if (!obj.TryGetComponent<Button>(out setSkill2))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                setSkill2.onClick.AddListener(delegate { ChangeSkillPos(2); });
            }
            if(!obj.TryGetComponent<Image>(out icon2))
            {
                Debug.Log("MenuManager - Awake - Image");
            }
            obj = settingPopup.transform.Find("SetSkill3").gameObject;
            if (!obj.TryGetComponent<Button>(out setSkill3))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                setSkill3.onClick.AddListener(delegate { ChangeSkillPos(3); });
            }
            if(!obj.TryGetComponent<Image>(out icon3))
            {
                Debug.Log("MenuManager - Awake - Image");
            }
        }
        exitPopup = GameObject.Find("ExitPopup");
        if (exitPopup != null)
        {
            if(!exitPopup.transform.Find("Yes").TryGetComponent<Button>(out exitButton))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                exitButton.onClick.AddListener(Exit);
            }
            if(!exitPopup.transform.Find("No").TryGetComponent<Button>(out notexitButton))
            {
                Debug.Log("MenuManager - Awake - Button");
            }
            else
            {
                notexitButton.onClick.AddListener(NotExit);
            }
        }
        if(!GameObject.Find("Player").TryGetComponent<PlayerController>(out player))
        {
            Debug.Log("MenuManager - Awake - PlayerController");
        }

        select = 0;
        pausePopup.SetActive(false);
        settingPopup.SetActive(false);
        exitPopup.SetActive(false);
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
        timeBackgroun.SetActive(false);
    }

    public void InitSkillButton(int num, int skill, string name, int level)
    {
        switch(num)
        { 
            case 0:
                basic.Init(0, skill, name, level); 
                break;
            case 1:
                skill1.Init(1,skill, name, level);
                icon1.sprite = Resources.Load<Sprite>("Image/" + name);
                break;
            case 11:
                break;
            case 2:
                skill2.Init(2,skill, name, level);
                icon2.sprite = Resources.Load<Sprite>("Image/" + name);
                break;
            case 21:
                break;
            case 3:
                skill3.Init(3,skill, name, level);
                icon3.sprite = Resources.Load<Sprite>("Image/" + name);
                break;
            case 31:
                break;
            case 4:
                ultimate.InitSkill(4,skill, name);
                break;
            case 41:
                ultimate.InitConnectedSkill(41,skill, name);
                break;
        }
        settingPopup.SetActive(false);
    }

    public void SetMaxUltimate(int value)
    {
        maxUltimateValue = value;
        if (maxUltimateValue > currentUltimateValue)
        {
            currentUltimateValue = maxUltimateValue;
        }
        ultimate.SetUaltimate(currentUltimateValue / maxUltimateValue);
    }

    public void SetUaltimate(float value)
    {
        currentUltimateValue = value;
        ultimate.SetUaltimate(currentUltimateValue/maxUltimateValue);
    }

    public void AddUaltimate(float value)
    {
        currentUltimateValue += value;
        if(currentUltimateValue > maxUltimateValue)
        {
            currentUltimateValue = maxUltimateValue;
        }
        ultimate.SetUaltimate(currentUltimateValue/maxUltimateValue);
    }

    public void InitPassive(int num, string name)
    {
        Sprite icon = Resources.Load<Sprite>("Image/"+ name);
        switch(num)
        {
            case 1:
                passive1.sprite = icon;
                break;
            case 2:
                passive2.sprite = icon;
                break;
            case 3:
                passive3.sprite = icon;
                break;
        }
    }

    public void SetKillCount(int value)
    {
        killCount.text = value.ToString();
    }

    public void SetSkillList(int weaponID)
    {
        TableEntity_Weapon weapon;
        GameManager.Inst.GetWeapon(weaponID, out weapon);
        int skillID = weaponID;
        for (int i = 0; i < weapon.Skill; i++)
        {
            if(i == 3)
            {
                skillID = weaponID / 10 - 2;
            }
            if(i < 6)
            {
                UsingSkills.Add(skillID + i);
                UsingSkills.Add(0);
            }
            else
            {
                UsingSkills.Add(skillID + i + 27);
                UsingSkills.Add(0);
            }
        }
    }

    public void SetLevelUpPopup(int playerLevel)
    {
        level.text = "Lv"+playerLevel.ToString();
        levelupPopup.SetActive(true);
        choice = 0;
        int pick1 = Random.Range(2, UsingSkills.Count -5);
        int pick2;
        int pick3;
        if(pick1 % 2 != 0)
        {
            pick1++;
        }
        pick2 = Random.Range(0, pick1 - 1);
        if (pick2 % 2 != 0)
        {
            pick2--;
        }
        pick3 = Random.Range(pick1 + 2, UsingSkills.Count - 1);
        if(pick3 % 2 != 0)
        {
            pick3--;
        }
        SetSelection(selection1, pick1);
        SetSelection(selection2, pick2);
        SetSelection(selection3, pick3);
        Time.timeScale = 0;
    }

    private void SetSelection(Choice selection, int pick)
    {
        if (UsingSkills[pick+1] == 0)
        {
            if(pick < 6)
            {
                selection.SetNewPassive(UsingSkills[pick]);
            }
            else
            {
                selection.SetNewSkill(UsingSkills[pick]);
            }
        }
        else
        {
            if (pick < 6)
            {
                selection.LevelUpPassive(UsingSkills[pick], UsingSkills[pick+1]);
            }
            else
            {
                selection.LevelUpSkill(UsingSkills[pick], UsingSkills[pick + 1]);
            }
        }
    }
    public void SetChoice(int value)
    {
        choice = value;
        selection1.SetPickLine(value);
        selection2.SetPickLine(value);
        selection3.SetPickLine(value);
    }

    private void Select()
    {
        Time.timeScale = 1;
        Debug.Log(choice);
        if(choice == 0)
        {
            return;
        }
        else
        {
            for(int i = 0; i < UsingSkills.Count; i++)
            {
                if (UsingSkills[i] == choice)
                {
                    if (UsingSkills[i+1] == 0)
                    {
                        if(i < 6)
                        {
                            GameManager.Inst.SetPassive(choice % 10 + 1, choice);
                        }
                        else
                        {
                            if(choice % 100 > 30)
                            {
                                if(choice % 10 == 1)
                                {
                                    GameManager.Inst.SetSkill(4, choice);
                                }
                                else
                                {
                                    GameManager.Inst.SetSkill(41, choice);
                                }
                            }
                            else
                            {
                                GameManager.Inst.SetSkill(choice % 10, choice);
                            }
                        }
                    }
                    else
                    {
                        if(i < 6)
                        {
                            GameManager.Inst.LevelUpPassive(choice % 10 + 1, choice);
                        }
                        else
                        {
                            if (choice % 100 > 30)
                            {
                                if (choice % 10 == 1)
                                {
                                    GameManager.Inst.LevelUpSkill(4, choice, UsingSkills[i+1]);
                                }
                                else
                                {
                                    GameManager.Inst.LevelUpSkill(41, choice, UsingSkills[i + 1]);
                                }
                            }
                            else
                            {
                                GameManager.Inst.LevelUpSkill(choice % 10, choice, UsingSkills[i + 1]);
                            }
                        }
                    }
                    UsingSkills[i + 1] += 1;
                    break;
                }
            }
        }
        levelupPopup.SetActive(false);
    }

    private void PressPause()
    {
        Time.timeScale = 0f;
        pause.gameObject.SetActive(false);
        pausePopup.SetActive(true);

    }

    private void OnPlay()
    {
        Time.timeScale = 1.0f;
        current.sprite = pauseImage;
        pause.gameObject.SetActive(true);
        pausePopup.SetActive(false);
    }

    private void OnSetting()
    {
        pausePopup.SetActive(false);
        settingPopup.SetActive(true);
        SettingData data = GameManager.Inst.GetSettinggData;
        bgm.value = data.bgm;
        sfx.value = data.sfx;
        damageText.isOn = data.damageText;
        camShake.isOn = data.cameraShake;
    }

    private void OnExit()
    {
        pausePopup.SetActive(false);
        exitPopup.SetActive(true);
    }

    private void OnSettingBack()
    {
        GameManager.Inst.SaveData();
        settingPopup.SetActive(false);
        pausePopup.SetActive(true);
    }

    private void Exit()
    {
        Time.timeScale = 1f;
        exitPopup.SetActive(false);
        player.ChangeState(State.Die);
        StartCoroutine(Exiting());
    }

    private IEnumerator Exiting()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        GameManager.Inst.AsyncLoadNextScene(SceneName.LobbyScene);
    }

    private void NotExit()
    {
        pausePopup.SetActive(true);
        exitPopup.SetActive(false);
    }
    private void SetVolumBGM()
    {
        GameManager.Inst.SetVolumBGM((int)bgm.value);
    }

    private void SetVolumSFX()
    {
        GameManager.Inst.SetVolumSFX((int)sfx.value);
    }

    private void SetDamageText()
    {
        GameManager.Inst.OnDamageText = damageText.isOn;
    }

    private void SetCameraShake()
    {
        GameManager.Inst.OnCamaraShake = camShake.isOn;
    }

    private void ChangeSkillPos(int num)
    {
        switch (select)
        {
            case 0:
                select = num;
                break;
            case 1:
                Vector3 pos = icon1.transform.position;
                Vector3 button = skill1.transform.position;
                switch (num)
                {
                    case 2:
                        icon1.transform.position = icon2.transform.position;
                        icon2.transform.position = pos;
                        skill1.transform.position = skill2.transform.position;
                        skill2.transform.position = button;
                        select = 0;
                        break;
                    case 3:
                        icon1.transform.position = icon3.transform.position;
                        icon3.transform.position = pos;
                        skill1.transform.position = skill3.transform.position;
                        skill3.transform.position = button;
                        select = 0;
                        break;
                }
                break;
            case 2:
                pos = icon2.transform.position;
                button = skill2.transform.position;
                switch (num)
                {
                    case 1:
                        icon2.transform.position = icon1.transform.position;
                        icon1.transform.position = pos;
                        skill2.transform.position = skill1.transform.position;
                        skill1.transform.position = button;
                        select = 0;
                        break;
                    case 3:
                        icon2.transform.position = icon3.transform.position;
                        icon3.transform.position = pos;
                        skill2.transform.position = skill3.transform.position;
                        skill3.transform.position = button;
                        select = 0;
                        break;
                }
                break;
            case 3:
                pos = icon3.transform.position;
                button = skill3.transform.position;
                switch (num)
                {
                    case 1:
                        icon3.transform.position = icon1.transform.position;
                        icon1.transform.position = pos;
                        skill3.transform.position = skill1.transform.position;
                        skill1.transform.position = button;
                        select = 0;
                        break;
                    case 2:
                        icon3.transform.position = icon2.transform.position;
                        icon2.transform.position = pos;
                        skill3.transform.position = skill2.transform.position;
                        skill2.transform.position = button;
                        select = 0;
                        break;
                }
                break;
        }
    }

    public void CoolTime_Passive(float passive)
    {
        skill1.Passive = passive;
        skill2.Passive = passive;
        skill3.Passive = passive;
    }

}
