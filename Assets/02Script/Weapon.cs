using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WeaponType
{
    None,
    Sowrd,
    Hammer,
    Gun,
}
public enum Physics_Type
{
    Slash,
    Strike,
    Thrust
}
public class Weapon : MonoBehaviour
{
    private int id;
    private WeaponType type;
    public WeaponType Type
    {
        get => type;
    }
    private float attackTime;
    public float AttackTime
    {
        set => attackTime = value;
    }
    private float weight;
    private Physics_Type physicsType;
    private float attack_Speed;
    public float Attack_Speed
    {
        get => attack_Speed;
        set => attack_Speed = value;
    }
    private bool IsSlash;
    private bool IsStrike;
    private bool IsExplosion;
    private float stagger_Time;
    public float StaggerTime
    {
        get => stagger_Time;
    }
    private float cretical_Chance;

    private float critical_Mag;
    public float CriticalMag()
    {
        IsCritical();
        if(isCritical)
        {
            return critical_Mag;
        }
        else
        {
            return 1f;
        }
    }
    private bool isCritical;
    private void IsCritical()
    {
        float value = Random.Range(0f, 100f);
        if(value <= cretical_Chance)
        {
            isCritical = true;
        }
        else
        {
            isCritical = false;
        }
    }

    private float weapon_Physics;
    public float Physics
    {
        get => weapon_Physics;
    }
    private float fire;
    public float Fire
    {
        get => fire;
    }
    private float water;
    public float Water
    {
        get => water;
    }
    private float electric;
    public float Electric
    {
        get => electric;
    }
    private float ice;
    public float Ice
    {
        get => ice;
    }
    private float wind;
    public float Wind
    {
        get => wind;
    }
    private BoxCollider col;
    private SkillManager skillManager;
    private ParticleSystem trail;

    private void Awake()
    {
        if(!TryGetComponent<BoxCollider>(out col))
        {
            Debug.Log("Weapon - Awake - BoxCollider");
        }
        if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("Weapon - Awake - SkillManager");
        }

        if (!transform.GetChild(0).TryGetComponent<ParticleSystem>(out trail))
        {
            Debug.Log("Weapon - Awake - ParticleSystem");
        }
        else
        {
            trail.Stop();
        }
    }

    public void Init(WeaponType type)
    {
        id = GameManager.Inst.PlayerInfo.WeaponID;
        this.type = type;
        TableEntity_Weapon data;
        GameManager.Inst.GetWeapon(id, out data);
        weight = data.Weight;
        switch (data.Physics_Type)
        {
            case "Slash":
                physicsType = Physics_Type.Slash;
                break;
            case "Strike":
                physicsType = Physics_Type.Strike;
                break;
            case "Thrust":
                physicsType = Physics_Type.Thrust;
                break;
        }
        IsSlash = data.Is_Slash;
        IsStrike = data.Is_Strike;
        IsExplosion = data.Is_Explosion;
        weapon_Physics = data.Physics;
        fire = data.Fire;
        water = data.Water;
        electric = data.Electric;
        ice = data.Ice;
        wind = data.Wind;
        cretical_Chance = data.Critical_Chance;
        critical_Mag = data.Critical_Mag;
        attack_Speed = data.Attack_Speed;
        if (SceneManager.GetActiveScene().buildIndex > 2)
            skillManager.WeaponInit(this);
    }
    public void OnTrail()
    {
        trail.Play();
    }

    public void OffTrail()
    {
        trail.Stop();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Creature")
        {
            skillManager.TakeDamageOther(AttackType.Weapon,other);
        }
    }
}
