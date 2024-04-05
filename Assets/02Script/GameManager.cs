using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

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
    public Inventory inventory;
    public int WeaponID;
    public int skill1_ID;
    public int skill2_ID;
    public int skill3_ID;
}

public enum SceneName
{
    StartScene,
    LoadingScene,
    PlayScene,
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private PlayerData pData;

    private PlayerController player;
    private FadeManager fadeManager;
    private MenuManager menuManager;
    private SoundManager soundManager;

    private TS table;
    private Dictionary<int, TableEntity_Player_Stats> playerDataTable = new Dictionary<int, TableEntity_Player_Stats>();
    private Dictionary<int, TableEntity_Skill_List> skillListTable = new Dictionary<int, TableEntity_Skill_List>();
    public bool GetSkillList(int skillID, out TableEntity_Skill_List data)
    {
        return skillListTable.TryGetValue(skillID, out data);
    }
    private Dictionary<int, TableEntity_Skill> skillDataTable = new Dictionary<int, TableEntity_Skill>();
    public bool GetSkillData(int ID, out TableEntity_Skill data)
    {
        return skillDataTable.TryGetValue(ID, out data);
    }
    private Dictionary<int, TableEntity_Skill_Hit_Frame> skillHitFrame = new Dictionary<int, TableEntity_Skill_Hit_Frame>();
    public bool GetSkillHitFrame(int skillID, TableEntity_Skill_Hit_Frame data)
    {
        return skillHitFrame.TryGetValue(skillID, out data);
    }
    private Dictionary<int, TableEntity_Weapon> weaponDataTable = new Dictionary<int, TableEntity_Weapon>();
    private Dictionary<int, TableEntity_Creature> creatureDataTable = new Dictionary<int, TableEntity_Creature>();
    public bool GetCreatureData(int key, out TableEntity_Creature data)
    {
        return creatureDataTable.TryGetValue(key, out data);
    }

    //  private List<TableEntity_Tip> Tip = new List<TableEntity_Tip>();

    private int ultimateValue;
    private int killCount;

    public void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        dataPath = Application.persistentDataPath + "/save";

        #region TableData
        table = Resources.Load<TS>("TS");
        playerDataTable.Add(0, table.Player_Stats[0]);
        for(int i = 0; i < table.Skill_List.Count; i++)
        {
            skillListTable.Add(table.Skill_List[i].ID, table.Skill_List[i]);
        }
        for(int i = 0; i < table.Skill_Info_List.Count; i++)
        {
            skillDataTable.Add(table.Skill_Info_List[i].ID, table.Skill_Info_List[i]);
        }
        for (int i = 0; i < table.Skill_Hit_Frame.Count; i++)
        {
            skillHitFrame.Add(table.Skill_Hit_Frame[i].Skill_ID, table.Skill_Hit_Frame[i]);
        }
        for (int i = 0; i < table.Weapon_List.Count; i++)
        {
            weaponDataTable.Add(table.Weapon_List[i].ID, table.Weapon_List[i]);
        }
        for (int i = 0; i < table.Creature_List.Count; i++)
        {
            creatureDataTable.Add(table.Creature_List[i].ID, table.Creature_List[i]);
        }
        #endregion

        pData = new PlayerData();
        CreateUserData();
    }

    private void OnLevelWasLoaded(int level)
    {
        StartCoroutine(InitGameManager(level));
    }

    private IEnumerator InitGameManager(int level)
    {
        LoadData();
        yield return YieldInstructionCache.WaitForSeconds(0.05f);
        
        if(level > 1)
        {
            bool end = false;
            if (menuManager == null)
            {
                GameObject.Find("Canvas").TryGetComponent<MenuManager>(out menuManager);

            }
            if (fadeManager == null)
            {
                GameObject.Find("Canvas").TryGetComponent<FadeManager>(out fadeManager);
                if (fadeManager != null)
                {
                    fadeManager.Fade_InOut(true);
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
            //GameObject.Find("SoundManager").TryGetComponent<SoundManager>(out soundManager);
            //if (soundManager != null)
            //{
            //    int activeScene = SceneManager.GetActiveScene().buildIndex;
            //}
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
            player.Init();
            menuManager.InitSkill(1, 0);
            menuManager.InitSkill(2, 202);
            menuManager.InitSkill(3, 0);
            ultimateValue = 0;
            killCount = 0;
            return true;
        }
    }
    #endregion

    #region PlayerDataGetter
    public PlayerData PlayerInfo
    {
        get => pData;
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
        playerDataTable.TryGetValue(0, out TableEntity_Player_Stats info);
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
        pData.WeaponID = 1;
        pData.skill1_ID = 201;
        pData.skill2_ID = 202;
        pData.skill3_ID = 203;
        SaveData();
    }

    public void PlayerIsController(bool value)
    {
        player.CONTROLL = value;
    }

    public void ChargeUaltimate(int value)
    {
        ultimateValue += value;
        menuManager.SetUaltimate(ultimateValue);
    }

    public void AddKillCount()
    {
        killCount++;
        menuManager.SetKillCount(killCount);
    }

    #endregion

    #region Save&Load
    private string dataPath;
    public void SaveData()
    {
        string data = JsonUtility.ToJson(pData);
        File.WriteAllText(dataPath, data);
    }

    public bool LoadData()
    {
        if (File.Exists(dataPath))
        {
            string data = File.ReadAllText(dataPath);
            pData = JsonUtility.FromJson<PlayerData>(data);
            return true;
        }

        return false;
    }

    public void DeleteData()
    {
        File.Delete(dataPath);
    }

    public bool CheckData()
    {
        if (File.Exists(dataPath))
        {
            return LoadData();
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
        //if (SceneManager.GetActiveScene().buildIndex > 2)
            // fadeManager.Fade_InOut(false);
        SceneManager.LoadScene("LoadingScene");
    }
    #endregion 
}