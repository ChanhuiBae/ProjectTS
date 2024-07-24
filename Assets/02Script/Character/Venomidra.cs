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

    private SkinnedMeshRenderer renderer;
    private Material phase2;
    private int phase;

    private Vector3 postion;
    private bool look;

    private Transform L;
    private Transform R;

    private void Start()
    {
        postion = Vector3.zero;
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

        if(!GameObject.Find("GuvnorRenderder").TryGetComponent<SkinnedMeshRenderer>(out renderer))
        {
            Debug.Log("Venomidra - Awake - SkinnedMeshRenderer");
        }

        if (!GameObject.Find("L").TryGetComponent<Transform>(out L))
        {
            Debug.Log("");
        }
        if (!GameObject.Find("R").TryGetComponent<Transform>(out R))
        {
            Debug.Log("");
        }

        phase2 = Resources.Load<Material>("Venomidra2Phase");
        phase = 1;
        hp.SetBossHP(1);
        look = true;
        StartCoroutine(UpdatePosition());
    }

    private IEnumerator UpdatePosition()
    {
        yield return YieldInstructionCache.WaitForSeconds(2f);
        postion = transform.position;
    }

    private void Update()
    {
        if(postion != Vector3.zero)
        {
            if (look)
            {
                transform.LookAt(player.transform);
            }
            //transform.position = postion;
        }
        if(creature != null && hp != null)
        {
            hp.SetBossHP(creature.HP);
        }
        if(creature.HP <= 0)
        {
            hp.gameObject.SetActive(false);
            return;
        }
    }

    public void Setlook(int value)
    {
        if(value > 0)
        {
            look = true;
        }
        else
        {
            look = false;
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

    public void setDrop(int key)
    {
        skillManager.StartDrop(key);
    }

    public void setBreath(int key)
    {
        GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_BossBreath);
        Effect effect = skillManager.SpawnEffect(27);
        effect.Init(EffectType.Once, transform.position + new Vector3(0,2,0) + transform.forward * 8,6.5f);
        effect.Key = key;
        effect.transform.rotation = transform.rotation;
    }


    public void SetPhase2()
    {
        renderer.material = phase2;
        phase = 2;
    }

    public void setTail(int key)
    {
        if(phase == 2)
        {
            skillManager.StartTailAttack(key);
        }
    }

    public void SpawnRock(int value)
    {
        GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_Earth);
        if (value > 0)
        {
            Effect effect = skillManager.SpawnEffect(29);
            effect.Init(EffectType.None, R.position, 2f);
        }
        else
        {
            Effect effect = skillManager.SpawnEffect(29);
            effect.Init(EffectType.None, L.position, 2f);
        }
    }

    public void PlaySound(int value)
    {
        switch(value)
        {
            case 4:
                GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_BossSwing);
                break;
            case 5:
                GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_BossStagger);
                break;
            case 6:
                GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_BossGroggy);
                break;
        }
    }
}
