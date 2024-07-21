using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venomidra : MonoBehaviour
{
    private BossGage hp;
    private Creture creature;
    private FollowCamera cam;

    private void Start()
    {
        if (!GameObject.Find("BossGage").TryGetComponent<BossGage>(out hp))
        {
            Debug.Log("Venomidra - Awake - BossGage");
        }
        if (!TryGetComponent<Creture>(out creature))
        {
            Debug.Log("Venomidra - Awake - Creature");
        }
        if(!Camera.main.TryGetComponent<FollowCamera>(out cam))
        {
            Debug.Log("Venomidra - Awake - FollowCamera");
        }
        else
        {
            cam.IsMove = false;
        }

        hp.SetBossHP(1);
    }

    private void Update()
    {
        hp.SetBossHP(creature.HP);
        if(creature.HP <= 0)
        {
            hp.gameObject.SetActive(false);
            return;
        }
    }

    public void SpawnSound()
    {
        GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_BossSpawn);
    }

    public void Scream()
    {
        GameManager.Inst.soundManager.PlaySFX(SFX_Type.SfX_BossScream);
    }
}
