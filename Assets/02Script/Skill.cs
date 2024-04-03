using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Skill : MonoBehaviour, ITakeDamage
{
    private PlayerController player;
    private PlayerAnimationController anim;
    private Weapon weapon;
    private AttackArea attackArea;
    private float current_skill_ID;
    private PoolManager effectManager;
    private PoolManager projectileManager;

    private void Awake()
    {
        GameObject obj = GameObject.Find("Player");
        if (!obj.TryGetComponent<PlayerController>(out player))
        {
            Debug.Log("Skill - Awake - PlayerController");
        }
        if (!obj.TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("Skill - Awake - PlayerAnimationController");
        }
        if(!obj.transform.Find("AttackArea").TryGetComponent<AttackArea>(out attackArea))
        {
            Debug.Log("Skill - Awake - AttackArea");
        }
        if (!GameObject.Find("EffectManager").TryGetComponent<PoolManager>(out effectManager))
        {
            Debug.Log("Skill - Awake - PoolManager");
        }
        if (!GameObject.Find("ProjectileManager").TryGetComponent<PoolManager>(out projectileManager))
        {
            Debug.Log("Skill - Awake - PoolManager");
        }
    }

    public void WeaponInit(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public void SetSkill(float value)
    {
        current_skill_ID = value;
        //to getSkill name & start corutine
    }


    public float TakeDamage(float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        return weapon.Physics * (1 - Creature_Physics_Cut) * weapon.CriticalMag()
            + (weapon.Fire * (1 - Creature_Fire_Cut)) + (weapon.Water * (1 - Creature_Water_Cut))
            + (weapon.Electric * (1 - Creature_Electric_Cut)) + (weapon.Ice * (1 - Creature_Ice_Cut))
            + (weapon.Wind * (1 - Creature_Wind_Cut));

    }

    public void TakeDamageOther(Collider other) 
    { 
        if (other.TryGetComponent<IDamage>(out IDamage creatureDamage))
        {
             creatureDamage.CalculateDamage(this);
        }
    }

    public void TakeProjectile(string name, Projectile projectile)
    {
        projectileManager.TakeToPool<Projectile>(name, projectile);
    }

    public void TakeEffect(string name, Effect effect)
    {
        effectManager.TakeToPool<Effect>(name, effect);
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
    */
    /*
    public void Dragon_Hammer_Attack()
    {
        if (damage == 0)
        {
            damage = 1f; // Weapon_Physics * (2.0 + 0.15 * skill.level)
            isKnockback = true;
            StartCoroutine(Buster());
        }
    }
    */
    /*
    private IEnumerator Buster()
    {
        effects[0].OnEffect();
        effects[1].OnEffect();
        yield return YieldInstructionCache.WaitForSeconds(1f);
        effects[0].OffEffect();
        effects[1].OffEffect();
    }
    */


}
