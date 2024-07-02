using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneManager : MonoBehaviour
{
    private GameObject weaponPopup;
    private Button hammer;
    private Button gun;

    private void Awake()
    {
        if (!GameObject.Find("HammerButton").TryGetComponent<Button>(out hammer))
        {
            Debug.Log("LobbySceneManager - Awake - Button");
        }
        if (!GameObject.Find("GunButton").TryGetComponent<Button>(out gun))
        {
            Debug.Log("LobbySceneManager - Awake - Button");
        }
        weaponPopup = GameObject.Find("Popup");
        if (weaponPopup != null)
        {
            weaponPopup.SetActive(false);
        }
        hammer.onClick.AddListener(SetHammer);
        gun.onClick.AddListener(SetGun);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(weaponPopup.activeSelf)
            {
                weaponPopup.SetActive(false);
            }
            else
            {
                weaponPopup.SetActive(true);
            }
        }
    }


    private void SetHammer()
    {
        GameManager.Inst.SetHammerTest();
    }

    private void SetGun()
    {
        GameManager.Inst.SetGunTest();  
    }
}
