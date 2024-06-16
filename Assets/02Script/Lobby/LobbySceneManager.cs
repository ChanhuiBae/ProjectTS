using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneManager : MonoBehaviour
{
    private Button sword;
    private Button hammer;
    private Button gun;

    private void Awake()
    {
        if(!GameObject.Find("SwordButton").TryGetComponent<Button>(out sword))
        {
            Debug.Log("LobbySceneManager - Awake - Button");
        }
        if (!GameObject.Find("HammerButton").TryGetComponent<Button>(out hammer))
        {
            Debug.Log("LobbySceneManager - Awake - Button");
        }
        if (!GameObject.Find("GunButton").TryGetComponent<Button>(out gun))
        {
            Debug.Log("LobbySceneManager - Awake - Button");
        }

        sword.onClick.AddListener(SetSword);
        hammer.onClick.AddListener(SetHammer);
        gun.onClick.AddListener(SetGun);
    }

    private void SetSword()
    {
        GameManager.Inst.SetSword();
    }

    private void SetHammer()
    {
        GameManager.Inst.SetHammer();
    }

    private void SetGun()
    {
        GameManager.Inst.SetGun();  
    }
}
