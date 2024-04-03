using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sowrd = 0,
    Hammer = 1,
    Gun = 2
}
public class Weapon : MonoBehaviour
{
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
    private float attack_Speed;
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
    private float weapon_Fire;
    public float Fire
    {
        get => weapon_Fire;
    }
    private float weapon_Water;
    public float Water
    {
        get => weapon_Water;
    }
    private float weapon_Electric;
    public float Electric
    {
        get => weapon_Electric;
    }
    private float weapon_Ice;
    public float Ice
    {
        get => weapon_Ice;
    }
    private float weapon_Wind;
    public float Wind
    {
        get => weapon_Wind;
    }
    private BoxCollider col;
    private Skill skillManager;

    private void Awake()
    {
        if(!TryGetComponent<BoxCollider>(out col))
        {
            Debug.Log("Weapon - Awake - BoxCollider");
        }
        if (!GameObject.Find("SkillManager").TryGetComponent<Skill>(out skillManager))
        {
            Debug.Log("Weapon - Awake - SkillManager");
        }
    }

    public void Init(WeaponType type)
    {
        this.type = type;
        skillManager.WeaponInit(this);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Creture")
        {
            skillManager.TakeDamageOther(other);
        }
    }
}
