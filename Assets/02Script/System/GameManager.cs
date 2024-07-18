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
    public int WeaponID;
    public int basic_ID;
    public Physics_Type physics;
}

public class WeaponSkillSetData
{
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
    public FadeManager fadeManager;
    public MenuManager menuManager;
    public SoundManager soundManager;
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
    public bool GetPassiveData(int passiveID, out TableEntitiy_Passive_Skill data)
    {
        return passiveTable.TryGetValue(passiveID, out data);
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

    private TableEntity_Weapon noWeapon = new TableEntity_Weapon();
    public TableEntity_Weapon GetNoWeapon ()
    {
        return noWeapon;
    }

    private Dictionary<int, TableEntity_Weapon> hammerTable = new Dictionary<int, TableEntity_Weapon>();
    public bool GetHammer(int weaponID, out TableEntity_Weapon data)
    {
        return hammerTable.TryGetValue(weaponID, out data);
    }
    public int GetHammerCount()
    {
        return hammerTable.Count;
    }

    private Dictionary<int, TableEntity_Weapon> rifleTable = new Dictionary<int, TableEntity_Weapon>();
    public bool GetRifle(int weaponID, out TableEntity_Weapon data)
    {
        return rifleTable.TryGetValue(weaponID, out data);
    }
    public int GetRifleCount()
    {
        return rifleTable.Count;
    }

    private Dictionary<int, TableEntity_Weapon> swordTable = new Dictionary<int, TableEntity_Weapon>();
    public bool GetSword(int weaponID, out TableEntity_Weapon data)
    {
        return swordTable.TryGetValue(weaponID, out data);
    }
    public int GetSwordCount()
    {
        return swordTable.Count;
    }

    private TableEntitiy_Mission mission = new TableEntitiy_Mission();

    public TableEntitiy_Mission GetMission()
    {
        return mission;
    }

    private TableEntity_Reward reward = new TableEntity_Reward();

    public TableEntity_Reward GetReward()
    {
        return reward;
    }

    public WeaponType GetWeaponType(int weaponID)
    {
        if (weaponID >= 3000)
        {
            return WeaponType.Gun;
        }
        else if(weaponID >= 2000)
        {
            return WeaponType.Hammer;
        }
        else if(weaponID >= 1000)
        {
            return WeaponType.Sowrd;
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
   

    private int killCount;
    private float exp;
    public float EXP
    {
        set => exp = value;
        get => exp;
    }

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
            if(table.Weapon_List[i].ID >= 3000)
            {
                rifleTable.Add(table.Weapon_List[i].ID, table.Weapon_List[i]);
            }
            else if (table.Weapon_List[i].ID >= 2000)
            {
                hammerTable.Add(table.Weapon_List[i].ID, table.Weapon_List[i]);
            }
            else if (table.Weapon_List[i].ID >= 1000)
            {
                swordTable.Add(table.Weapon_List[i].ID, table.Weapon_List[i]);
            }
            else
            {
                noWeapon = table.Weapon_List[i];
            }
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
        mission = table.Mission[0];
        reward = table.Reward[0];
        #endregion

        pData = new PlayerData();
        wsData = new WeaponSkillSetData();
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
            player.Init(GetWeaponType(pData.WeaponID));
            if(SceneManager.GetActiveScene().buildIndex > 2)
            {
                menuManager.SetSkillList(pData.WeaponID);
                if (pData.basic_ID != 0)
                {
                    TableEntity_Skill skill;
                    GetSkillData(pData.basic_ID, out skill);
                    menuManager.InitSkillButton(0, pData.basic_ID, skill.Skill_Name_Eng, 1);
                    skillManager.SetSkill(0, pData.basic_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
                }
                menuManager.InitSkillButton(1, wsData.skill1_ID, "None", 1);
                menuManager.InitSkillButton(11, wsData.connected1_ID, "None", 1);
                menuManager.InitSkillButton(2, wsData.skill2_ID, "None", 1);
                menuManager.InitSkillButton(21, wsData.connected2_ID, "None", 1);
                menuManager.InitSkillButton(3, wsData.skill3_ID, "None", 1);
                menuManager.InitSkillButton(31, wsData.connected1_ID, "None", 1);
                menuManager.InitSkillButton(4, wsData.ultimate_ID, "None", 1);
                menuManager.InitSkillButton(41, wsData.connectedU_ID, "None", 1);
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
        pData.WeaponID = 2000;
        pData.basic_ID = 200;
        pData.physics = Physics_Type.Strike;
        wsData.skill1_ID = 0;
        wsData.connected1_ID = 0;
        wsData.skill2_ID = 0;
        wsData.connected2_ID = 0;
        wsData.skill3_ID = 0;
        wsData.connected3_ID = 0;
        wsData.ultimate_ID = 0;
        wsData.connectedU_ID = 0;
        wsData.passive1_ID = 0;
        wsData.passive2_ID = 0;
        wsData.passive3_ID = 0;
        setData.bgm = 0;
        setData.sfx = 0;
        setData.damageText = true;
        setData.cameraShake = true;
        SaveData();
    }

    public void SetHammer()
    {
        pData.WeaponID = 2000;
        pData.basic_ID = 200;
        pData.physics = Physics_Type.Strike;
        wsData.skill1_ID = 0;
        wsData.connected1_ID = 0;
        wsData.skill2_ID = 0;
        wsData.connected2_ID = 0;
        wsData.skill3_ID = 0;
        wsData.connected3_ID = 0;
        wsData.ultimate_ID = 0;
        wsData.connectedU_ID = 0;
        wsData.passive1_ID = 0;
        wsData.passive2_ID = 0;
        wsData.passive3_ID = 0;
        SaveData();
    }

    public void SetGun()
    {
        pData.WeaponID = 3000;
        pData.basic_ID = 300;
        pData.physics = Physics_Type.Thrust;
        wsData.skill1_ID = 0;
        wsData.connected1_ID = 0;
        wsData.skill2_ID = 0;
        wsData.connected2_ID = 0;
        wsData.skill3_ID = 0;
        wsData.connected3_ID = 0;
        wsData.ultimate_ID = 0;
        wsData.connectedU_ID = 0;
        wsData.passive1_ID = 0;
        wsData.passive2_ID = 0;
        wsData.passive3_ID = 0;
        SaveData();
    }

    public void SetSkill(int num, int id)
    {
        TableEntity_Skill skill;
        switch (num)
        {
            case 1:
                wsData.skill1_ID = id;
                GetSkillData(wsData.skill1_ID, out skill);
                menuManager.InitSkillButton(1, wsData.skill1_ID, skill.Skill_Name_Eng, 1);
                skillManager.SetSkill(1, wsData.skill1_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
                break;
            case 11:
                wsData.connected1_ID = id;
                GetSkillData(wsData.connected1_ID, out skill);
                menuManager.InitSkillButton(11, wsData.connected1_ID, skill.Skill_Name_Eng, 1);
                skillManager.SetSkill(11, wsData.connected1_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
                break;
            case 2:
                wsData.skill2_ID = id;
                GetSkillData(wsData.skill2_ID, out skill);
                menuManager.InitSkillButton(2, wsData.skill2_ID, skill.Skill_Name_Eng, 1);
                skillManager.SetSkill(2, wsData.skill2_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
                break;
            case 21:
                wsData.connected2_ID = id;
                GetSkillData(wsData.connected2_ID, out skill);
                menuManager.InitSkillButton(21, wsData.connected2_ID, skill.Skill_Name_Eng, 1);
                skillManager.SetSkill(21, wsData.connected2_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
                break;
            case 3:
                wsData.skill3_ID = id;
                GetSkillData(wsData.skill3_ID, out skill);
                menuManager.InitSkillButton(3, wsData.skill3_ID, skill.Skill_Name_Eng, 1);
                skillManager.SetSkill(3, wsData.skill3_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
                break;
            case 31:
                wsData.connected3_ID = id;
                GetSkillData(wsData.connected3_ID, out skill);
                menuManager.InitSkillButton(31, wsData.connected3_ID, skill.Skill_Name_Eng, 1);
                skillManager.SetSkill(31, wsData.connected3_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
                break;
            case 4:
                wsData.ultimate_ID = id;
                GetSkillData(wsData.ultimate_ID, out skill);
                menuManager.InitSkillButton(4, wsData.ultimate_ID, skill.Skill_Name_Eng, 1);
                skillManager.SetSkill(4, wsData.ultimate_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
                break;
            case 41:
                wsData.connectedU_ID = id;
                GetSkillData(wsData.connectedU_ID, out skill);
                menuManager.InitSkillButton(41, wsData.connectedU_ID, skill.Skill_Name_Eng, 1);
                skillManager.SetSkill(41, wsData.connectedU_ID, skill.Weapon_ID, skill.Category_ID, skill.Skill_Level_Max, skill.Charge_Max, skill.Hit_Max);
                break;
        }
    }

    public void SetPassive(int num, int id)
    {
        TableEntitiy_Passive_Skill passive;
        switch (num)
        {
            case 1:
                wsData.passive1_ID = id;
                GetPassiveData(wsData.passive1_ID, out passive);
                menuManager.InitPassive(1, passive.Name_Eng);
                skillManager.SetPassive(1, passive);
                break;
            case 2:
                wsData.passive2_ID = id;
                GetPassiveData(wsData.passive2_ID, out passive);
                menuManager.InitPassive(2, passive.Name_Eng);
                skillManager.SetPassive(2, passive);
                break;
            case 3:
                wsData.passive3_ID = id;
                GetPassiveData(wsData.passive3_ID, out passive);
                menuManager.InitPassive(3, passive.Name_Eng);
                skillManager.SetPassive(3, passive);
                break;
        }

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

    public void LevelUpSkill(int num, int skill_ID, int level)
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
                if (skill_ID == wsData.skill2_ID)
                    check = true;
                break;
            case 21:
                if (skill_ID == wsData.connected2_ID)
                    check = true;
                break;
            case 3:
                if (skill_ID == wsData.skill3_ID)
                    check = true;
                break;
            case 31:
                if (skill_ID == wsData.connected3_ID)
                    check = true;
                break;
            case 4:
                if (skill_ID == wsData.ultimate_ID)
                    check = true;
                break;
            case 41:
                if (skill_ID == wsData.connectedU_ID)
                    check = true;
                break;
        }
        if (check)
        {
            TableEntity_Skill skill;
            GameManager.Inst.GetSkillData(skill_ID, out skill);
            menuManager.InitSkillButton(num, skill_ID, skill.Skill_Name_Eng, level);
            skillManager.LevelUpSkill(num, skill_ID);
        }
    }

    public void LevelUpPassive(int num, int level)
    {
        skillManager.LevelUpPassive(num, level);
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