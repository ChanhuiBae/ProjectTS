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
    private float critical_Mag;
    public float CriticalMag()
    {
        critical_Mag = Random.Range(0f, 1f);
        return critical_Mag;
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
            Debug.Log("AttackArea - Init - SkillManager");
        }
    }

    public void Init(WeaponType type)
    {
        this.type = type;
        
    }
    /*
    public void NormalAttack()
    {
        if (damage == 0)
        {
            if (type == WeaponType.Gun)
            {
                // todo : calculateDamage
                damage = 5;
                GameObject obj = pool.GetFromPool<Projectile>(0).gameObject;
                obj.transform.rotation = transform.rotation;
                Projectile projectile = obj.GetComponent<Projectile>();
                projectile.Init(damage, transform.GetChild(0).transform.position);
            }
            else
            {
                // todo : calculateDamage
                damage = 5f;
            }
            StartCoroutine(DamageZero());
        }
    }
    public void Dragon_Hammer_Attack()
    {
        if (damage == 0)
        {
            damage = 1f; // Weapon_Physics * (2.0 + 0.15 * level)
            isKnockback = true;
            StartCoroutine(Buster());
        }
    }

    private IEnumerator Buster()
    {
        effects[0].OnEffect();
        effects[1].OnEffect();
        yield return YieldInstructionCache.WaitForSeconds(1f);
        effects[0].OffEffect();
        effects[1].OffEffect();
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Creture")
        {
            skillManager.TakeDamageOther(other);
        }
    }
}
