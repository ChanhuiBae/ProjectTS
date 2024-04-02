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
public class Weapon : MonoBehaviour, ITakeDamage
{
    private WeaponType type;
    private float attackTime;
    public float AttackTime
    {
        set => attackTime = value;
    }
    protected float damage;
    protected float Weapon_Physics;
    protected float Weapon_Fire;
    protected float Weapon_Water;
    protected float Weapon_Electric;
    protected float Weapon_Ice;
    protected float Weapon_Wind;
    protected PlayerController owner;
    protected PoolManager pool;
    protected List<Effect> effects;

    private bool isKnockback;

    private void Awake()
    {
        if(!GameObject.Find("Player").TryGetComponent<PlayerController>(out owner))
        {
            Debug.Log("Weapon - Awake - PlayerController");
        }   
        effects = new List<Effect>();
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent<Effect>(out Effect effect))
            {
                effects.Add(effect);
                effects[i].Init(EffectType.Skill);
            }
        }
        isKnockback = false;
    }

    public void Init(WeaponType type, float attackTime)
    {
        this.type = type;
        if(this.type == WeaponType.Gun)
        {
            if (!GameObject.Find("PoolManager").TryGetComponent<PoolManager>(out pool))
            {
                Debug.Log("Weapon - Init - PoolManager");
            }
        }
        damage = 0;
        this.attackTime = attackTime;
    }

    public WeaponType GetType()
    {
        return type;
    }
    
    public void ResetDamage()
    {
        damage = 0;
        isKnockback = false;
    }

    private IEnumerator DamageZero()
    {
        yield return YieldInstructionCache.WaitForSeconds(attackTime);
        damage = 0;
    }

    public void CalculateDamage(float Critical_Mag, float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        damage = Weapon_Physics * (1 - Creature_Physics_Cut) * Critical_Mag
            + (Weapon_Fire * (1 - Creature_Fire_Cut)) + (Weapon_Water * (1 - Creature_Water_Cut)) 
            + (Weapon_Electric * (1 - Creature_Electric_Cut)) + (Weapon_Ice * (1 - Creature_Ice_Cut)) 
            + (Weapon_Wind * (1 - Creature_Wind_Cut));

    }

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

    public float TakeDamage()
    {
        return damage;
    }

    public float TakeDamage(float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        CalculateDamage(owner.GetCritical_Mag(), Creature_Physics_Cut, Creature_Fire_Cut, Creature_Water_Cut, Creature_Electric_Cut, Creature_Ice_Cut, Creature_Wind_Cut);
        return damage;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root != this.transform.root)
        {
            if(other.TryGetComponent<IDamage>(out IDamage creture))
            {
                if(isKnockback)
                    creture.Knockback();

                creture.GetDamage(this);
            }
        }
    }
}
