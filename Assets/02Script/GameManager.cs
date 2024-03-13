using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO; // input output controll

[System.Serializable]
public class PlayerData
{
    public int uidCounter;
    public string userNickname;
    public float maxHP;
    public int health;
    public int patience;
    public int Strength;
    public int intuition;
    public int adaptation;
    public float experience;
    public Inventory inventory;
    //public Weapon weapon;
}

public enum SceneName
{
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

    public void Awake()
    {
        base.Awake();
        // dataPath = Application.persistentDataPath + "/save";

        #region TableData
       // table = Resources.Load<>("");
       // for (int i = 0; i < table.ItemData.Count; i++)
       // {
       //     itemDataTable.Add(table.ItemData[i].uid, table.ItemData[i]);
       // }
        #endregion

        pData = new PlayerData();
    }

    private void OnLevelWasLoaded(int level)
    {
        StartCoroutine(InitGameManager());
    }

    private IEnumerator InitGameManager()
    {
        LoadData();
        yield return YieldInstructionCache.WaitForSeconds(0.05f);
        bool end = false;

        while (!end)
        {
            end = UpdatePlayer();
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        }

        if (menuManager == null)
        {
            GameObject.Find("MainCanvas").TryGetComponent<MenuManager>(out menuManager);
            //if (menuManager != null)
               // menuManager.InitMenuManager();
        }
        if (fadeManager == null)
        {
            GameObject.Find("MainCanvas").TryGetComponent<FadeManager>(out fadeManager);
            if (fadeManager != null)
            {
               // fadeManager.Fade_InOut(true);
               // player.ISCONTROLLER = true;
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
        if (!GameObject.Find("Player").TryGetComponent<PlayerController>(out player))
        {
            Debug.Log("GameManger - UpdateGMInfo - PlayerController");
            return false;
        }
        else
        {
           // player.InitPlayerController();
            return true;
        }
    }
    #endregion

    #region TableData

    //private Fairytale table;
    //private Dictionary<int, TableEntity_Item> itemDataTable = new Dictionary<int, TableEntity_Item>();

   // public bool GetItemData(int itemID, out TableEntity_Item data)
   // {
   //     return itemDataTable.TryGetValue(itemID, out data);
   // }

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
    public void CreateUserData(string newNickName)
    {
        pData.userNickname = newNickName;
        SaveData();
    }

    #region Player Setter
    public bool LootingItem(InventoryItemData newItem)
    {
        return false;
    }

    public void PlayerIsController(bool control)
    {

    }
    #endregion
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


    public string PlayerName
    {
        get => pData.userNickname;
    }


    public bool CheckItem(int itemID)
    {

        return false;
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
        if (SceneManager.GetActiveScene().buildIndex > 2)
           // fadeManager.Fade_InOut(false);
        SceneManager.LoadScene("LoadingScene");
    }
    #endregion
}
