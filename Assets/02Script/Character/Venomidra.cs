using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venomidra : MonoBehaviour
{
    private BossGage hp;
    private Creture creature;
    private FollowCamera cam;
    private BoxCollider lHand;
    private BoxCollider rHand;
    private BoxCollider head;
    private BoxCollider body;


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
        
        if(!GameObject.Find("GuvnorHead").TryGetComponent<BoxCollider>(out head))
        {
            Debug.Log("Venomidra - Awake - BoxCollider");
        }
        if (!GameObject.Find("Bip001 Spine1").TryGetComponent<BoxCollider>(out body))
        {
            Debug.Log("Venomidra - Awake - BoxCollider");
        }
        if (!GameObject.Find("Bip001 L Hand").TryGetComponent<BoxCollider>(out lHand))
        {
            Debug.Log("Venomidra - Awake - BoxCollider");
        }
        if (!GameObject.Find("Bip001 R Hand").TryGetComponent<BoxCollider>(out rHand))
        {
            Debug.Log("Venomidra - Awake - BoxCollider");
        }

        if (!Camera.main.TryGetComponent<FollowCamera>(out cam))
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
    public void setBothHand(int value)
    {
        if (value > 0)
        {
            lHand.enabled = true;
            rHand.enabled = true;
        }
        else
        {
            lHand.enabled = false;
            rHand.enabled= false;
        }
    }

    public void setLeftHand(int value)
    {
        if(value > 0) 
        {
            lHand.enabled = true;
        }
        else
        {
            lHand.enabled =false;
        }
    }

    public void setRightHand(int value)
    {
        if (value > 0)
        {
            rHand.enabled = true;
        }
        else
        {
            rHand.enabled = false;
        }
    }

    public void setHead(int value)
    {
        if (value > 0)
        {
            head.enabled = true;
        }
        else
        {
            head.enabled = false;
        }
    }

    public void setBody(int value)
    {
        if (value > 0)
        {
            body.enabled = true;
        }
        else
        {
            body.enabled = false;
        }
    }
}
