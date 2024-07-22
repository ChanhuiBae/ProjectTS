using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venomidra : MonoBehaviour
{
    private BossGage hp;
    private Creture creature;
    private FollowCamera cam;

    private SkillManager skillManager;
    private PlayerController player;

    private CapsuleCollider lHand;
    private CapsuleCollider rHand;
    private BoxCollider head;

    private AttackPart headP;
    private AttackPart lhandP;
    private AttackPart rhandP;

    private Transform thorn;

    private void Start()
    {
        if(!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("Venomidra - Awake - SkillManager");
        }
        if(!GameObject.Find("Player").TryGetComponent<PlayerController>(out player))
        {
            Debug.Log("Venomidra - Awake - PlayerController");
        }

        if (!GameObject.Find("BossGage").TryGetComponent<BossGage>(out hp))
        {
            Debug.Log("Venomidra - Awake - BossGage");
        }
        if (!TryGetComponent<Creture>(out creature))
        {
            Debug.Log("Venomidra - Awake - Creature");
        }

        GameObject obj = GameObject.Find("GuvnorHead");
        if (!obj.TryGetComponent<BoxCollider>(out head))
        {
            Debug.Log("Venomidra - Awake - BoxCollider");
        }
        if(!obj.TryGetComponent<AttackPart>(out headP))
        {
            Debug.Log("Venomidra - Awake - AttackPart");
        }

        obj = GameObject.Find("Bip001 L Hand");
        if (!obj.TryGetComponent<CapsuleCollider>(out lHand))
        {
            Debug.Log("Venomidra - Awake - BoxCollider");
        }
        if (!obj.TryGetComponent<AttackPart>(out lhandP))
        {
            Debug.Log("Venomidra - Awake - AttackPart");
        }
        obj = GameObject.Find("Bip001 R Hand");
        if (!obj.TryGetComponent<CapsuleCollider>(out rHand))
        {
            Debug.Log("Venomidra - Awake - BoxCollider");
        }
        if (!obj.TryGetComponent<AttackPart>(out rhandP))
        {
            Debug.Log("Venomidra - Awake - AttackPart");
        }

        thorn = GameObject.Find("Bip001 Neck").transform;

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
    public void setBothHandTrue(int key)
    {
        lHand.enabled = true;
        rHand.enabled = true;
        lhandP.Key = key;
        rhandP.Key = key;
    }

    public void setLeftHandTrue(int key)
    {
        lHand.enabled = true;
        lhandP.Key= key;
    }

    public void setRightHandTrue(int key)
    {
        rHand.enabled = true;
        rhandP.Key = key;
    }

    public void setHeadTrue(int key)
    {
        head.enabled = true;
        headP.Key = key;
    }

    public void setBothHandFalse()
    {
        lHand.enabled = false;
        rHand.enabled = false;
    }

    public void setLeftHandFalse()
    {
        lHand.enabled = false;
    }

    public void setRightHandFalse()
    {
        rHand.enabled = false;
    }

    public void setHeadFalse()
    {
        head.enabled = false;
    }


    public void setThorn(int key)
    {
        skillManager.SpawnThorn(thorn.position, player.gameObject, key);
    }

    public void setAirThorn()
    {

    }

    public void setBreath()
    {

    }
}
