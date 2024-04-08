using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillManager : MonoBehaviour, ITakeDamage
{
    private PlayerController player;
    private PlayerAnimationController anim;
    private Weapon weapon;
    private AttackArea attackArea;
    private Skill basic;
    private Skill skill1;
    private Skill skill2;
    private Skill skill3;
    private PoolManager effectManager;
    private PoolManager projectileManager;
    private int useSkill;

    private void Awake()
    {
        GameObject obj = GameObject.Find("Player");
        if (!obj.TryGetComponent<PlayerController>(out player))
        {
            Debug.Log("SkillManager - Awake - PlayerController");
        }
        if (!obj.TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("SkillManager - Awake - PlayerAnimationController");
        }
        if (!obj.transform.Find("AttackArea").TryGetComponent<AttackArea>(out attackArea))
        {
            Debug.Log("SkillManager - Awake - AttackArea");
        }
        if (!GameObject.Find("EffectManager").TryGetComponent<PoolManager>(out effectManager))
        {
            Debug.Log("SkillManager - Awake - PoolManager");
        }
        if (!GameObject.Find("ProjectileManager").TryGetComponent<PoolManager>(out projectileManager))
        {
            Debug.Log("SkillManager - Awake - PoolManager");
        }

        if(!transform.Find("Basic").TryGetComponent<Skill>(out basic))
        {
            Debug.Log("SkillManager - Awake - SkillBasic");
        }
        if (!transform.Find("Skill1").TryGetComponent<Skill>(out skill1))
        {
            Debug.Log("SkillManager - Awake - Skill1");
        }
        if (!transform.Find("Skill2").TryGetComponent<Skill>(out skill2))
        {
            Debug.Log("SkillManager - Awake - Skill2");
        }
        if (!transform.Find("Skill3").TryGetComponent<Skill>(out skill3))
        {
            Debug.Log("SkillManager - Awake - Skill3");
        }
        useSkill = -1;
    }

    public void WeaponInit(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public void SetSkill(int num, int ID, string Weapon_ID, int Category_ID, int Skill_Level_Max, int Charge_Max, int Hit_Max)
    {
        switch (num)
        {
            case 0:
                this.basic.init(ID, Weapon_ID, Category_ID, Skill_Level_Max, Charge_Max, Hit_Max);
                break;
            case 1:
                this.skill1.init(ID, Weapon_ID, Category_ID, Skill_Level_Max, Charge_Max, Hit_Max);
                break;
            case 2: 
                this.skill2.init(ID, Weapon_ID, Category_ID, Skill_Level_Max, Charge_Max, Hit_Max);    
                break;
            case 3:
                this.skill3.init(ID, Weapon_ID, Category_ID, Skill_Level_Max, Charge_Max, Hit_Max);
                break;
        }
    }

    public void UseSkill(int num)
    {
        useSkill = num;
        switch (useSkill)
        {
            case 0:
                basic.StartSkill();
                break;
            case 1:
                skill1.StartSkill();
                break;
            case 2: 
                skill2.StartSkill();
                 break;
            case 3:
                skill3.StartSkill();
                break;
        }
    }

    public float TakeDamage(float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        float weaponPhysics = weapon.Physics * (1 - Creature_Physics_Cut) * weapon.CriticalMag()
            + (weapon.Fire * (1 - Creature_Fire_Cut)) + (weapon.Water * (1 - Creature_Water_Cut))
            + (weapon.Electric * (1 - Creature_Electric_Cut)) + (weapon.Ice * (1 - Creature_Ice_Cut))
            + (weapon.Wind * (1 - Creature_Wind_Cut));
        float skillPhysics = basic.GetDamageA() + (basic.GetDamageB() * basic.GetLevel()); ;
        switch (useSkill)
        {
            case 1:
                skillPhysics = skill1.GetDamageA() + (skill1.GetDamageB() * skill1.GetLevel());
                break;
            case 2:
                skillPhysics = skill2.GetDamageA() + (skill2.GetDamageB() * skill2.GetLevel());
                break;
            case 3:
                skillPhysics = skill3.GetDamageA() + (skill3.GetDamageB() * skill3.GetLevel());
                break;
        }
        Debug.Log(weaponPhysics * skillPhysics);
        return weaponPhysics * skillPhysics;
    }

    public void TakeDamageOther(Collider other)
    {
        if (useSkill > -1)
        {
            if (other.TryGetComponent<IDamage>(out IDamage creatureDamage))
            {
                creatureDamage.CalculateDamage(this);
                //creatureDamage.Stun(2);
                //creatureDamage.Airborne(1);
                //creatureDamage.Knockback(10);
            }
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
