using Redcode.Pools;
using UnityEngine;

public enum CrowdControl
{
    None,
    Stun,
    Knockback,
    Airborne,
    Pulled
}

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
    private Effect chargeEffect;
    private PoolManager projectileManager;
    private int useSkill;
    private bool isCharge;
    private CrowdControl crowdControl;
    public bool IsCharge
    {
        get => isCharge;
        set => isCharge = value;
    }
    
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
        isCharge = false;
        crowdControl = CrowdControl.None;
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

    public void StopCharge()
    {
        switch (useSkill)
        {
            case 1:
                skill1.StopCharge();
                break;
            case 2:
                skill2.StopCharge();
                break;
            case 3:
                skill3.StopCharge();
                break;
        }
    }

    public void StartAnimator()
    {
        player.StartAnimator();
    }

    public void ChargeUp()
    {
        player.ChargeUp();
    }

    public void SetCrowdControl(CrowdControl type)
    {
        crowdControl = type;
    }
    public Effect SpawnEffect(int num)
    {
        GameObject obj = effectManager.GetFromPool<Effect>(num).gameObject;
        return obj.GetComponent<Effect>();
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
        return weaponPhysics * skillPhysics;
    }

    public void TakeDamageOther(string name, Collider other)
    {
        if (useSkill > -1)
        {
            if (other.TryGetComponent<IDamage>(out IDamage creatureDamage))
            {
                creatureDamage.CalculateDamage(this);
                switch (crowdControl)
                {
                    case CrowdControl.Stun:
                        creatureDamage.Stun(1);
                        break;
                    case CrowdControl.Airborne:
                        creatureDamage.Airborne(1);
                        break;
                    case CrowdControl.Knockback:
                        creatureDamage.Knockback(5);
                        break;
                    case CrowdControl.Pulled:
                        Debug.Log(name + " pull");
                        creatureDamage.Pulled(player.transform.position);
                        break;
                }
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
}
