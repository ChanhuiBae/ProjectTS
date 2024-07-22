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
    public void setBothHandTrue(int key)
    {
        lHand.enabled = true;
        rHand.enabled = true;
        
    }

    public void setLeftHandTrue(int key)
    {
        lHand.enabled = true;
    }

    public void setRightHandTrue(int key)
    {
        rHand.enabled = true;
    }

    public void setHeadTrue(int key)
    {
        head.enabled = true;
    }

    public void setBody(int key)
    {
        body.enabled = true;
    }
}
