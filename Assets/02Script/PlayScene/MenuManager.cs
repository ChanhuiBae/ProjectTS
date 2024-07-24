using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private Image block;
    private TimePopup timePopup;
    private BossGage bossHP;

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
    private List<int> allSkills;
    private List<int> usingSkills;
    private List<int> maxSkills;
    private int priSkill;
    private int priPassive;
    private int listCount;
    private List<int> list;


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

    private RewardPopup rewardPopup;
    private Image warning;

    private PlayerController player;

    private void Awake()
    {
        QualitySettings.shadowDistance = 55;
        if (!GameObject.Find("BACKGROUND").TryGetComponent<Image>(out block))
        {
            Debug.Log("RewardPopup - Awake - Image");
        }

        if (!GameObject.Find("TimePopup").TryGetComponent<TimePopup>(out timePopup))
        {
            Debug.Log("MenuManager - Awake - TimePopup");
        }
        if(!GameObject.Find("BossGage").TryGetComponent<BossGage>(out bossHP))
        {
            Debug.Log("MenuManager - Awake - BossGage");
        }

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

        if (!GameObject.Find("WarningPopup").TryGetComponent<Image>(out warning))
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
        allSkills = new List<int>();
        usingSkills = new List<int>();
        maxSkills = new List<int>();
        list = new List<int>();

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

        if(!GameObject.Find("RewardPopup").TryGetComponent<RewardPopup>(out rewardPopup))
        {
            Debug.Log("MenuManager - Awake - RewardPopup");
        }

        select = 0;
        warning.gameObject.SetActive(false);
        pausePopup.SetActive(false);
        settingPopup.SetActive(false);
        exitPopup.SetActive(false);
    }

    private void Start()
    {
        block.enabled = false;
        SetBossGage(1);
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
                if(skill == 0)
                    icon1.sprite = Resources.Load<Sprite>("Image/SkillBack");
                else
                    icon1.sprite = Resources.Load<Sprite>("Image/" + name);
                break;
            case 11:
                break;
            case 2:
                skill2.Init(2,skill, name, level); 
                if (skill == 0)
                    icon2.sprite = Resources.Load<Sprite>("Image/SkillBack");
                else
                    icon2.sprite = Resources.Load<Sprite>("Image/" + name);
                break;
            case 21:
                break;
            case 3:
                skill3.Init(3,skill, name, level);
                if (skill == 0)
                    icon3.sprite = Resources.Load<Sprite>("Image/SkillBack");
                else
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
        int skillID = weaponID;

        for (int i = 0; i < 7; i++)
        {
            if(i == 3)
            {
                skillID = weaponID / 10 - 2;
            }
            if(i < 6)
            {
                allSkills.Add(skillID + i);
                usingSkills.Add(0);
                maxSkills.Add(5);
            }
            else
            {
                allSkills.Add(skillID + i + 27);
                usingSkills.Add(0);
                maxSkills.Add(3);
            }
        }
        TableEntity_Skill ultimate;
        GameManager.Inst.GetSkillData(allSkills[6], out ultimate);
        for(int i = 0; i < allSkills.Count; i++)
        {
            if (allSkills[i] == ultimate.Primal_Active)
            {
                priSkill = i;
            }
            else if (allSkills[i] == ultimate.Primal_Passive) 
            { 
                priPassive = i;
            }
        }
    }

    private int GetIndexAtAllSkills(int value)
    {
        for(int i = 0; i < allSkills.Count; i++)
        {
            if (allSkills[i] == value)
                return i;
        }
        return -1;
    }

    private int PickSkill()
    {
        int pick = Random.Range(0, list.Count);
        int i = GetIndexAtAllSkills(list[pick]);
        if(usingSkills[i] < maxSkills[i]) 
        {
            return list[pick];
        }
        else
        {
            return PickSkill();
        }
    }

    public void SetLevelUpPopup(int playerLevel)
    {
        level.text = "Lv"+playerLevel.ToString();
        if(playerLevel > 34)
        {
            return;
        }

        block.enabled = true;
        levelupPopup.SetActive(true);
        choice = 0;
        list.Clear();
        if (usingSkills[priSkill] > 0 && usingSkills[priPassive] > 0)
        {
            listCount = allSkills.Count;
        }
        else
        {
            listCount = allSkills.Count - 1;
        }
        for (int i =0; i < listCount; i++)
        {
            if (usingSkills[i] != maxSkills[i])
            {
                list.Add(allSkills[i]);
            }
        }

        int pick1 = -1;
        int pick2 = -1;
        int pick3 = -1;

        if (list.Count > 0)
        {
            pick1 = PickSkill();
            Debug.Log(pick1);
            list.Remove(pick1);
        }
        if (list.Count > 0)
        {
            pick2 = PickSkill();
            list.Remove(pick2);
        }
        if(list.Count > 0)
        {
            pick3 = PickSkill();
        }

        SetSelection(selection1, GetIndexAtAllSkills(pick1));
        SetSelection(selection2, GetIndexAtAllSkills(pick2));
        SetSelection(selection3, GetIndexAtAllSkills(pick3));
        Time.timeScale = 0;
        StartCoroutine(SetScale());
    }

    private IEnumerator SetScale()
    {
        levelupPopup.transform.localScale = Vector3.zero;
        GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_LevelUp);
        for (float i = 0; i <= 1; i += 0.05f)
        {
            yield return null;
            levelupPopup.transform.localScale = new Vector3(i, i, i);
        }
    }

    private void SetSelection(Choice selection, int pick)
    {
        if(pick == -1)
        {
            selection.NoChoice();
            return;
        }
        if (usingSkills[pick] == 0)
        {
            if(pick < 3)
            {
                selection.SetNewPassive(allSkills[pick]);
            }
            else
            {
                selection.SetNewSkill(allSkills[pick]);
            }
        }
        else
        {
            if (pick < 3)
            {
                selection.LevelUpPassive(allSkills[pick], usingSkills[pick]);
            }
            else
            {
                selection.LevelUpSkill(allSkills[pick], usingSkills[pick]);
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
        if(choice == 0)
        {
            return;
        }
        else
        {
            block.enabled = false;
            Time.timeScale = 1;
            for (int i = 0; i < allSkills.Count; i++)
            {
                if (allSkills[i] == choice)
                {
                    if (usingSkills[i] == 0)
                    {
                        if(i < 3)
                        {
                            GameManager.Inst.SetPassive(choice % 10 + 1, choice);
                        }
                        else
                        {
                            if(choice % 100 > 30)
                            {
                                GameManager.Inst.SetSkill(4, choice);
                                TableEntity_Skill skill;
                                GameManager.Inst.GetSkillData(choice, out skill);
                                if(skill.Linked_Skill != 0)
                                {
                                    GameManager.Inst.SetSkill(41, choice + 1);
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
                        if(i < 3)
                        {
                            GameManager.Inst.LevelUpPassive(choice % 10 + 1, choice);
                        }
                        else
                        {
                            if (choice % 100 > 30)
                            {
                                GameManager.Inst.LevelUpSkill(4, choice, usingSkills[i]);
                                TableEntity_Skill skill;
                                GameManager.Inst.GetSkillData(choice,out skill);
                                if(skill.Linked_Skill != 0)
                                {
                                    GameManager.Inst.LevelUpSkill(41, choice + 1, usingSkills[i]);
                                }
                            }
                            else
                            {
                                GameManager.Inst.LevelUpSkill(choice % 10, choice, usingSkills[i]);
                            }
                        }
                    }
                    usingSkills[i] += 1;
                    break;
                }
            }
        }
        levelupPopup.SetActive(false);
    }

    private void PressPause()
    {
        block.enabled = true;
        Time.timeScale = 0f;
        pause.gameObject.SetActive(false);
        pausePopup.SetActive(true);

    }

    private void OnPlay()
    {
        block.enabled = false;
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
    }

    private void NotExit()
    {
        pausePopup.SetActive(true);
        exitPopup.SetActive(false);
    }

    public void SetReward(bool boss)
    {
        rewardPopup.gameObject.SetActive(true);
        rewardPopup.SetRewardPopup(timePopup.Time, boss);
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

    public void Warning()
    {
        warning.gameObject.SetActive(true);
        StartCoroutine(StartWarning());
    }

    private IEnumerator StartWarning()
    {
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Warring);
        for(int i = 0; i < 3; i++)
        {
            StartCoroutine(RedWarning());
            yield return YieldInstructionCache.WaitForSeconds(1.42f);
        }
        warning.gameObject.SetActive(false);    
        bossHP.gameObject.SetActive(true);
        bossHP.SetBossHP(1);
    }

    private IEnumerator RedWarning()
    {
        Color color = warning.color;

        for (float i = 0; i < 0.4f; i += 0.01f)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.01f);
            color.a = i;
            warning.color = color;
        }
        for (float i = 0.4f; i > 0; i-= 0.01f)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.01f);
            color.a = i;
            warning.color = color;
        }
    }

    public void SetBossGage(float value)
    {
        bossHP.SetBossHP(value);
    }

}
