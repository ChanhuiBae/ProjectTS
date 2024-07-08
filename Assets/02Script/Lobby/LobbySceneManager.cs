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
        GameManager.Inst.SetHammer();
    }

    private void SetGun()
    {
        GameManager.Inst.SetGun();  
    }
}
