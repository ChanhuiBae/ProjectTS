using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

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
    public Weapon weapon;
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
    //private Dictionary<int, TableEntity_Weapon> weaponDataTable = new Dictionary<int, TableEntity_Weapon>();
    //private Dictionary<int, TableEntity_DropList> dropDataTable = new Dictionary<int, TableEntity_DropList>();
    //private Dictionary<int, TableEntity_Monster> monsterData = new Dictionary<int, TableEntity_Monster>();
    //public bool GetMonsterData(int itemID, out TableEntity_Monster moster)
    // {
    //     return monsterData.TryGetValue(itemID, out moster);
    // }
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
        playerDataTable.Add(0, table.Player_Default_Stats[0]);
        /*
        for (int i = 0; i < table.WeaponData.Count; i++)
        {
            weaponDataTable.Add(table.WeaponData[i].uid, table.WeaponData[i]);
            weaponItemID.Add(table.WeaponData[i].uid);
        }
        for (int i = 0; i < table.MonsterData.Count; i++)
        {
            monsterData.Add(table.MonsterData[i].uid, table.MonsterData[i]);
        }
        for (int i = 0; i < table.DropList.Count; i++)
        {
            dropDataTable.Add(table.DropList[i].uid, table.DropList[i]);
        }
        */
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
            while (!end)
            {
                end = UpdatePlayer();
                yield return YieldInstructionCache.WaitForSeconds(0.05f);
            }
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
        pData.Critical_Chance = info.Critical_Chance;
        pData.Health = info.Health;
        pData.Endurance = info.Endurance;
        pData.Strength = info.Strength;
        pData.Dexterity = info.Dexterity;
        pData.Adaptation = info.Adaptation;
        pData.Available_Point = info.Available_Point;
        pData.Exp_Need = info.Exp_Need;
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