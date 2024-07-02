using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using Unity.VisualScripting;

[System.Serializable]
public class PlayerData
{
    public int uidCounter;
    public int Level;
    public int Max_Weight;
    public float Max_HP;
    public int Critical_Chance;
    public int Health;
    public int Endurance;
    public int Strength;
    public int Dexterity;
    public int Adaptation;
    public int Available_Point;
    public float Exp_Need;
    public int ArmorID;
    public Inventory inventory;
}

public class WeaponSkillSetData
{
    public int WeaponID;
    public int basic_ID;
    public int skill1_ID;
    public int connected1_ID;
    public int skill2_ID;
    public int connected2_ID;
    public int skill3_ID;
    public int connected3_ID;
    public int ultimate_ID;
    public int connectedU_ID;
    public int passive1_ID;
    public int passive2_ID;
    public int passive3_ID;
}

[System.Serializable]
public class SettingData
{
    public int uidCounter;
    public int bgm;
    public int sfx;
    public bool damageText;
    public bool cameraShake;
}

public enum SceneName
{
    StartScene,
    LoadingScene,
    PlayScene,
    LobbyScene
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private PlayerData pData;
    private WeaponSkillSetData wsData;
    private SettingData setData;

    private PlayerController player;
    private FadeManager fadeManager;
    public MenuManager menuManager;
    private SoundManager soundManager;
    public SkillManager skillManager;
    private PatternManager patternManager;

    private TS table;
    private Dictionary<int, TableEntity_Player> playerDataTable = new Dictionary<int, TableEntity_Player>();
    private Dictionary<int, TableEntity_Skill> skillTable = new Dictionary<int, TableEntity_Skill>();
    public bool GetSkillData(int skillID, out TableEntity_Skill data)
    {
        return skillTable.TryGetValue(skillID, out data);
    }
    private Dictionary<int, TableEntitiy_Passive_Skill> passiveTable = new Dictionary<int, TableEntitiy_Passive_Skill>();
    public bool GetPassiveData(int passiveID, out TableEntity_Skill data)
    {
        return skillTable.TryGetValue(passiveID, out data);
    }
    private Dictionary<int, TableEntity_Skill_Info> skillInfoTable = new Dictionary<int, TableEntity_Skill_Info>();
    public bool GetSkillInfoData(int ID, out TableEntity_Skill_Info data)
    {
        return skillInfoTable.TryGetValue(ID, out data);
    }
    private Dictionary<int, TableEntity_Skill_Hit_Frame> skillHitFrameTable = new Dictionary<int, TableEntity_Skill_Hit_Frame>();
    public bool GetSkillHitFrame(int skillID, out TableEntity_Skill_Hit_Frame data)
    {
        return skillHitFrameTable.TryGetValue(skillID, out data);
    }
    private Dictionary<int, TableEntity_Weapon> weaponTable = new Dictionary<int, TableEntity_Weapon>();
    public bool GetWeapon(int weaponID, out TableEntity_Weapon data)
    {
        return weaponTable.TryGetValue(weaponID, out data);
    }
    public WeaponType GetWeaponType(int weaponID)
    {
        TableEntity_Weapon weapon;
        weaponTable.TryGetValue(weaponID, out weapon);
        switch(weapon.Type)
        {
            case 1:
                return WeaponType.Sowrd;
            case 2:
                return WeaponType.Hammer;
            case 3:
                return WeaponType.Gun;
        }
        return WeaponType.None;
    }

    private Dictionary<int,TableEntity_Armor> armorTable = new Dictionary<int, TableEntity_Armor>();
    public bool GetArmorData(int key, out TableEntity_Armor data)
    {
        return armorTable.TryGetValue(key, out data);
    }

    private Dictionary<int, TableEntity_Creature> creatureTable = new Dictionary<int, TableEntity_Creature>();
    public bool GetCreatureData(int key, out TableEntity_Creature data)
    {
        return creatureTable.TryGetValue(key, out data);
    }

    private Dictionary<int, TableEntity_Pattern> patternTable = new Dictionary<int, TableEntity_Pattern>();

    public bool GetPatternData(int key, out TableEntity_Pattern data)
    {
        return patternTable.TryGetValue(key, out data);
    }

    private Dictionary<int, TableEntity_Pattern_Info> patternInfoTable = new Dictionary<int, TableEntity_Pattern_Info>();

    public bool GetPatternInfoData(int key, out TableEntity_Pattern_Info data)
    {
        return patternInfoTable.TryGetValue(key, out data);
    }

    private Dictionary<int, TableEntity_Pattern_Hit_Frame> patternHitFrameTable = new Dictionary<int, TableEntity_Pattern_Hit_Frame>();

    public bool GetPatternHitFrameData(int key, out TableEntity_Pattern_Hit_Frame data)
    {
        return patternHitFrameTable.TryGetValue(key, out data);
    }
    //  private List<TableEntity_Tip> Tip = new List<TableEntity_Tip>();

    private int killCount;

    public void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        dataPath = Application.persistentDataPath;

        #region TableData
        table = Resources.Load<TS>("TS");
        playerDataTable.Add(0, table.Player_Stats[0]);
        for(int i = 0; i < table.Skill_List.Count; i++)
        {
            skillTable.Add(table.Skill_List[i].ID, table.Skill_List[i]);
        }
        for(int i = 0; i < table.Skill_Info_List.Count; i++)
        {
            skillInfoTable.Add(table.Skill_Info_List[i].ID, table.Skill_Info_List[i]);
        }
        for (int i = 0; i < table.Skill_Hit_Frame.Count; i++)
        {
            skillHitFrameTable.Add(table.Skill_Hit_Frame[i].ID, table.Skill_Hit_Frame[i]);
        }
        for (int i = 0; i < table.Weapon_List.Count; i++)
        {
            weaponTable.Add(table.Weapon_List[i].ID, table.Weapon_List[i]);
        }
        for(int i = 0; i < table.Armor_List.Count; i++)
        {
            armorTable.Add(table.Armor_List[i].ID, table.Armor_List[i]);
        }
        for (int i = 0; i < table.Creature_List.Count; i++)
        {
            creatureTable.Add(table.Creature_List[i].ID, table.Creature_List[i]);
        }
        for (int i = 0; i < table.Pattern_List.Count; i++)
        {
            patternTable.Add(table.Pattern_List[i].ID, table.Pattern_List[i]);
        }
        for(int i = 0; i < table.Pattern_Info_List.Count; i++) 
        {
            patternInfoTable.Add(table.Pattern_Info_List[i].ID, table.Pattern_Info_List[i]);
        }
        for(int i = 0; i < table.Pattern_Hit_Frame.Count; i++)
        {
            patternHitFrameTable.Add(table.Pattern_Hit_Frame[i].ID, table.Pattern_Hit_Frame[i]);
        }
        for(int i = 0; i < table.Passive_Skill.Count; i++)
        {
            passiveTable.Add(table.Passive_Skill[i].ID, table.Passive_Skill[i]);
        }
        #endregion

        pData = new PlayerData();
        wsData = new WeaponSkillSetData();
        SetSword();
        setData = new SettingData();
        if (CheckData())
        {
            LoadData();
        }
        else
        {
            CreateUserData();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        StartCoroutine(InitGameManager(level));
    }

    private IEnumerator InitGameManager(int level)
    {
        LoadData();
        yield return YieldInstructionCache.WaitForSeconds(0.05f);
        if (level > 2)
        {
            if (skillManager == null)
            {
                GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager);
            }
            if(patternManager == null)
            {
                GameObject.Find("PatternManager").TryGetComponent<PatternManager>(out patternManager);
                if(skillManager != null)
                {
                    List<int> creatureIDList = new List<int>();
                    for(int i = 0; creatureTable.Count < 0; i++)
                    {
                        creatureIDList.Add(creatureTable[i].ID);
                    }
                    patternManager.Init(creatureIDList);
                }
            }
            if (menuManager == null)
            {
                GameObject.Find("Canvas").TryGetComponent<MenuManager>(out menuManager);

            }
        }
        if (level > 1)
        {
            bool end = false;
            if (fadeManager == null)
            {
                GameObject.Find("Canvas").TryGetComponent<FadeManager>(out fadeManager);
                if (fadeManager != null)
                {
                    fadeManager.FadeOut(0.25f);
                }
            }
            while (!end)
            {
                end = UpdatePlayer();
                yield return YieldInstructionCache.WaitForSeconds(0.05f);
            }
        }

        if (soundManager == null)
        {
            GameObject.Find("SoundManager").TryGetComponent<SoundManager>(out soundManager);
            if (soundManager != null)
            {
                int activeScene = SceneManager.GetActiveScene().buildIndex;
            }
        }
    }

    #region UpdateGMInfo
    public bool UpdatePlayer()
    {
        GameObject obj = GameObject.Find("Player");
        if (obj != null && !obj.TryGetComponent<PlayerController>(out player))
        {
            Debug.Log("GameManger - UpdateGMInfo - PlayerController");
            return false;
        }
        else
        {
            player.Init(GetWeaponType(wsData.WeaponID));
            if(SceneManager.GetActiveScene().buildIndex > 2)
            {
                if (wsData.basic_ID != 0)
                {
                    TableEntity_Skill skill;
                    GameManager.Inst.GetSkillData(wsData.basic_ID, out skill);
                    menuManager.InitSkillButton(0, wsData.basic_ID, skill.Skill_Name_Eng, 1);
                    skillManager.SetSkill(0, wsData.basic_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
                }
                if (wsData.skill1_ID != 0)
                {
                    TableEntity_Skill skill;
                    GameManager.Inst.GetSkillData(wsData.skill1_ID, out skill);
                    menuManager.InitSkillButton(1, wsData.skill1_ID, skill.Skill_Name_Eng, 1);
                    skillManager.SetSkill(1, wsData.skill1_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);

                }
                else
                {
                    menuManager.InitSkillButton(1, wsData.skill1_ID, "None", 1);
                }
                if (wsData.connected1_ID != 0)
                {
                    TableEntity_Skill skill;
                    GameManager.Inst.GetSkillData(wsData.connected1_ID, out skill);
                    menuManager.InitSkillButton(11, wsData.connected1_ID, skill.Skill_Name_Eng, 1);
                    skillManager.SetSkill(11, wsData.connected1_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);

                }
                else
                {
                    menuManager.InitSkillButton(11, wsData.connected1_ID, "None", 1);
                }
                if (wsData.skill2_ID != 0)
                {
                    TableEntity_Skill skill;
                    GameManager.Inst.GetSkillData(wsData.skill2_ID, out skill);
                    menuManager.InitSkillButton(2, wsData.skill2_ID, skill.Skill_Name_Eng, 1);
                    skillManager.SetSkill(2, wsData.skill2_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);

                }
                else
                {
                    menuManager.InitSkillButton(2, wsData.skill2_ID, "None", 1);
                }
                if (wsData.connected2_ID != 0)
                {
                    TableEntity_Skill skill;
                    GameManager.Inst.GetSkillData(wsData.connected2_ID, out skill);
                    menuManager.InitSkillButton(21, wsData.connected2_ID, skill.Skill_Name_Eng, 1);
                    skillManager.SetSkill(21, wsData.connected2_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);

                }
                else
                {
                    menuManager.InitSkillButton(21, wsData.connected2_ID, "None", 1);
                }
                if (wsData.skill3_ID != 0)
                {
                    TableEntity_Skill skill;
                    GameManager.Inst.GetSkillData(wsData.skill3_ID, out skill);
                    menuManager.InitSkillButton(3, wsData.skill3_ID, skill.Skill_Name_Eng, 1);
                    skillManager.SetSkill(3, wsData.skill3_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);

                }
                else
                {
                    menuManager.InitSkillButton(3, wsData.skill3_ID, "None", 1);
                }
                if (wsData.connected3_ID != 0)
                {
                    TableEntity_Skill skill;
                    GameManager.Inst.GetSkillData(wsData.connected3_ID, out skill);
                    menuManager.InitSkillButton(31, wsData.connected3_ID, skill.Skill_Name_Eng, 1);
                    skillManager.SetSkill(31,wsData.connected3_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);

                }
                else
                {
                    menuManager.InitSkillButton(31, wsData.connected1_ID, "None", 1);
                }
                if (wsData.ultimate_ID != 0)
                {
                    TableEntity_Skill skill;
                    GameManager.Inst.GetSkillData(wsData.ultimate_ID, out skill);
                    menuManager.InitSkillButton(4, wsData.ultimate_ID, skill.Skill_Name_Eng, 1);
                    skillManager.SetSkill(4, wsData.ultimate_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);

                }
                else
                {
                    menuManager.InitSkillButton(4, wsData.ultimate_ID, "None", 1);
                }
                if (wsData.connectedU_ID != 0)
                {
                    TableEntity_Skill skill;
                    GameManager.Inst.GetSkillData(wsData.connectedU_ID, out skill);
                    menuManager.InitSkillButton(41, wsData.connectedU_ID, skill.Skill_Name_Eng, 1);
                    skillManager.SetSkill(41, wsData.connectedU_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
                }
                else
                {
                    menuManager.InitSkillButton(41, wsData.connectedU_ID, "None", 1);
                }
                killCount = 0;
            }
            return true;
        }
    }
    #endregion



    #region PlayerDataGetter
    public PlayerData PlayerInfo
    {
        get => pData;
    }

    public WeaponSkillSetData WeaponSkillData
    {
        get => wsData;
    }

    public SettingData GetSettinggData
    {
        get => setData;
    }

    public bool OnDamageText
    {
        get => setData.damageText;
        set => setData.damageText = value;
    }

    public bool OnCamaraShake
    {
        get => setData.cameraShake;
        set => setData.cameraShake = value;
    }

    public Inventory INVENTORY
    {
        get
        {
            return pData.inventory;
        }
    }

    public int PlayerUIDMaker
    {
        get
        {
            return pData.uidCounter++;
        }
    }
    #endregion

    #region Setter
    public void CreateUserData()
    {
        pData.uidCounter = 0;
        playerDataTable.TryGetValue(0, out TableEntity_Player info);
        pData.Level = info.Level;
        pData.Max_Weight = info.Max_Weight;
        pData.Max_HP = info.Max_Hp;
        pData.Health = info.Health;
        pData.Endurance = info.Endurance;
        pData.Strength = info.Strength;
        pData.Dexterity = info.Dexterity;
        pData.Adaptation = info.Adaptation;
        pData.Available_Point = info.Available_Point;
        pData.Exp_Need = info.Exp_Need;
        pData.ArmorID = 1;
        wsData.WeaponID = 1000;
        wsData.basic_ID = 100;
        wsData.skill1_ID = 101;
        wsData.connected1_ID = 0;
        wsData.skill2_ID = 102;
        wsData.connected2_ID = 0;
        wsData.skill3_ID = 103;
        wsData.connected3_ID = 0;
        wsData.ultimate_ID = 131;
        wsData.connectedU_ID = 0;
        setData.bgm = 0;
        setData.sfx = 0;
        setData.damageText = true;
        setData.cameraShake = true;
        SaveData();
    }

    public void SetSword()
    {
        wsData.WeaponID = 1000;
        wsData.basic_ID = 100;
        wsData.skill1_ID = 101;
        wsData.connected1_ID = 0;
        wsData.skill2_ID = 102;
        wsData.connected2_ID = 0;
        wsData.skill3_ID = 103;
        wsData.connected3_ID = 0;
        wsData.ultimate_ID = 131;
        wsData.connectedU_ID = 0;
    }

    public void SetHammer()
    {
        wsData.WeaponID = 2000;
        wsData.basic_ID = 200;
        wsData.skill1_ID = 201;
        wsData.connected1_ID = 0;
        wsData.skill2_ID = 202;
        wsData.connected2_ID = 0;
        wsData.skill3_ID = 203;
        wsData.connected3_ID = 0;
        wsData.ultimate_ID = 231;
        wsData.connectedU_ID = 232;
    }

    public void SetGun()
    {
        wsData.WeaponID = 3000;
        wsData.basic_ID = 300;
        wsData.skill1_ID = 301;
        wsData.connected1_ID = 0;
        wsData.skill2_ID = 302;
        wsData.connected2_ID = 0;
        wsData.skill3_ID = 303;
        wsData.connected3_ID = 0;
        wsData.ultimate_ID = 331;
        wsData.connectedU_ID = 0;
    }

    public void SetVolumBGM(int value)
    {
        setData.bgm = value;
        soundManager.SetVolumBGM(value);
    }

    public void SetVolumSFX(int value)
    {
        setData.sfx = value;
        soundManager.SetVolumSFX(value);
    }

    public void PlayerIsController(bool value)
    {
        player.CONTROLL = value;
    }

    public void SetMaxUltimate(int value)
    {
        menuManager.SetMaxUltimate(value);
    }

    public void ResetUltimate()
    {
        menuManager.SetUaltimate(0f);
    }
    public void AddUaltimate(float value)
    {
        menuManager.AddUaltimate(value);
    }

    public void AddKillCount()
    {
        killCount++;
        menuManager.SetKillCount(killCount);
    }

    public void UpdateSkill(int num, int skill_ID, int level)
    {
        bool check = false;
        switch (num)
        {
            case 1:
                if (skill_ID == wsData.skill1_ID)
                    check = true;
                break;
            case 11:
                if (skill_ID == wsData.connected1_ID)
                    check = true;
                break;
            case 2:
                if(skill_ID == wsData.skill2_ID)
                    check = true; 
                break;
            case 21:
                if (skill_ID == wsData.connected2_ID)
                    check = true;
                break;
            case 3:
                if(skill_ID == wsData.skill3_ID)
                    check = true;
                break;
            case 31:
                if (skill_ID == wsData.connected3_ID)
                    check = true;
                break;
            case 4:
                if(skill_ID == wsData.ultimate_ID)
                    check = true;
                break;
            case 41:
                if(skill_ID == wsData.connectedU_ID)
                    check = true;
                break;
        }
        if (check)
        {
            TableEntity_Skill skill;
            GameManager.Inst.GetSkillData(skill_ID, out skill);
            menuManager.InitSkillButton(num, skill_ID, skill.Skill_Name_Eng, level);
            skillManager.SetSkill(num, wsData.skill1_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
        }
    }

    #endregion

    #region Save&Load
    private string dataPath;
    public void SaveData()
    {
        string path = dataPath + "/player";
        string data = JsonUtility.ToJson(pData);
        File.WriteAllText(path, data);
        path = dataPath + "/setting";
        data = JsonUtility.ToJson(setData);
        File.WriteAllText(path, data);
    }

    public bool LoadData()
    {
        if (File.Exists(dataPath + "/player"))
        {
            string data = File.ReadAllText(dataPath + "/player");
            pData = JsonUtility.FromJson<PlayerData>(data);

            if (File.Exists(dataPath + "/setting"))
            {
                data = File.ReadAllText(dataPath + "/setting");
                setData = JsonUtility.FromJson<SettingData>(data);
                return true;
            }
        }

        return false;
    }

    public void DeleteData()
    {
        File.Delete(dataPath);
    }

    public bool CheckData()
    {
        if (File.Exists(dataPath + "/player") && File.Exists(dataPath + "/setting"))
        {
            return true;
        }
        return false;
    }
    #endregion

    #region updateUserData
    public void CreateUserData(int playerUid)
    {
        SaveData();
    }
    #endregion

    #region LoadingLogic
    private SceneName nextScene;
    public SceneName NextScene
    {
        get => nextScene;
    }

    public void AsyncLoadNextScene(SceneName scene)
    {
        SaveData();
        nextScene = scene;
        SceneManager.LoadScene("LoadingScene");
    }
    #endregion 
}