using Redcode.Pools;
using System.Collections.Generic;
using UnityEngine;

public enum CrowdControl
{
    None,
    Stagger,
    Stun,
    Knockback,
    Airborne,
    Airback,
    Pulled
}

public enum AttackType
{
    Weapon,
    Projectile,
    AttackArea,
    Effect
}

public class SkillManager : MonoBehaviour, ITakeDamage
{
    private PlayerController player;
    private Weapon weapon;
    private AttackArea attackArea;
    
    private Skill basic;
    private Skill skill1;
    private Skill connected1;
    private Skill skill2;
    private Skill connected2;
    private Skill skill3;
    private Skill connected3;
    private Skill ultimate;
    private Skill connectedU;

    private PoolManager effectManager;
    private PoolManager projectileManager;

    private int useSkill;
    private bool isCharge;
    private Vector3 playerLook;
    private Queue<Vector3> queue;
    private CrowdControl crowdControl;
    private Vector3 pulledPoint;
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

        if (!transform.Find("Basic").TryGetComponent<Skill>(out basic))
        {
            Debug.Log("SkillManager - Awake - SkillBasic");
        }
        if (!transform.Find("Skill1").TryGetComponent<Skill>(out skill1))
        {
            Debug.Log("SkillManager - Awake - Skill1");
        }
        if (!skill1.transform.GetChild(0).TryGetComponent<Skill>(out connected1))
        {
            Debug.Log("SkillManager - Awake - ConnectedSkill");
        }
        if (!transform.Find("Skill2").TryGetComponent<Skill>(out skill2))
        {
            Debug.Log("SkillManager - Awake - Skill2");
        }
        if (!skill2.transform.GetChild(0).TryGetComponent<Skill>(out connected2))
        {
            Debug.Log("SkillManager - Awake - ConnectedSkill");
        }
        if (!transform.Find("Skill3").TryGetComponent<Skill>(out skill3))
        {
            Debug.Log("SkillManager - Awake - Skill3");
        }
        if (!skill3.transform.GetChild(0).TryGetComponent<Skill>(out connected3))
        {
            Debug.Log("SkillManager - Awake - ConnectedSkill");
        }
        if (!transform.Find("Ultimate").TryGetComponent<Skill>(out ultimate))
        {
            Debug.Log("SkillManager - Awake - Ultimate");
        }
        if (!ultimate.transform.GetChild(0).TryGetComponent<Skill>(out connectedU))
        {
            Debug.Log("SkillManager - Awake - ConnectedSkill");
        }
        useSkill = -1;
        isCharge = false;
        crowdControl = CrowdControl.None;
        pulledPoint = player.transform.position;
        queue = new Queue<Vector3>();
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
            case 11:
                this.connected1.init(ID, Weapon_ID, Category_ID, Skill_Level_Max, Charge_Max, Hit_Max);
                break;
            case 2:
                this.skill2.init(ID, Weapon_ID, Category_ID, Skill_Level_Max, Charge_Max, Hit_Max);
                break;
            case 21:
                this.connected2.init(ID, Weapon_ID, Category_ID, Skill_Level_Max, Charge_Max, Hit_Max);
                break;
            case 3:
                this.skill3.init(ID, Weapon_ID, Category_ID, Skill_Level_Max, Charge_Max, Hit_Max);
                break;
            case 31:
                this.connected3.init(ID, Weapon_ID, Category_ID, Skill_Level_Max, Charge_Max, Hit_Max);
                break;
            case 4:
                this.ultimate.init(ID, Weapon_ID, Category_ID, Skill_Level_Max, Charge_Max, Hit_Max);
                break;
            case 41:
                this.connectedU.init(ID, Weapon_ID, Category_ID, Skill_Level_Max, Charge_Max, Hit_Max);
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
            case 4:
                ultimate.StartSkill();
                break;
            case 41:
                connectedU.StartSkill();
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

    public void SpawnUniqeEffect(int num, EffectType type,Vector3 pos, float lifeTime)
    {
        GameObject obj = effectManager.GetFromPool<Effect>(num).gameObject;
        Effect effect = obj.GetComponent<Effect>();
        effect.Init(type, pos, lifeTime);
    }

    public void SetAttackArea(Vector3 center, float radius)
    {
        attackArea.Attack(center, radius);
    }

    public void MoveAttackArea(Vector3 center, float radius)
    {
        attackArea.Move(center, radius);
    }

    public void ShowAttackArea()
    {
        attackArea.StartView();
    }

    public void StopAttackArea()
    {
        attackArea.StopAttack();
        attackArea.StopView();
    }

    public void SetLook(Vector3 look)
    {
        playerLook = look;
    }

    public Vector3 GetLook()
    {
        return playerLook;
    }

    public void PushVector(Vector3 look)
    {
        queue.Enqueue(look);
    }

    public Vector3 PopVector()
    {
        if(queue.Count == 0)
            return Vector3.zero;
        return queue.Dequeue();
    }

    public int GetVectorCount()
    {
        return queue.Count;
    }

    public int GetCurrentKey()
    {
        switch (useSkill)
        {
            case 1:
                return skill1.GetKey();
            case 2:
                return skill2.GetKey();
            case 3:
                return skill3.GetKey();
            case 4:
                return ultimate.GetKey();
            case 41:
                return connectedU.GetKey();
            default:
                return basic.GetKey();
        }
    }

    public void SpawnBasicProjectile(Vector3 pos)
    {
        GameObject obj = projectileManager.GetFromPool<Projectile>(0).gameObject;
        Projectile projectile = obj.GetComponent<Projectile>();
        projectile.Init(ProjectileType.Bullet, pos, basic.GetKey());
        projectile.AttackBullet(player.transform.rotation);
    }
    
    public void SpawnGrenade(Vector3 pos, Quaternion rotation)
    {
        GameObject obj = projectileManager.GetFromPool<Projectile>(1).gameObject;
        Projectile projectile = obj.GetComponent<Projectile>();
        switch (useSkill)
        {
            case 1:
                projectile.Init(ProjectileType.Grenade, pos, skill1.GetKey());
                break;
            case 2:
                projectile.Init(ProjectileType.Grenade, pos, skill2.GetKey());
                break;
            case 3:
                projectile.Init(ProjectileType.Grenade, pos, skill3.GetKey());
                break;
        }
        projectile.AttackGrenade(rotation);
    }
    public void SpawnAPHEProjectile(Vector3 pos)
    {
        GameObject obj = projectileManager.GetFromPool<Projectile>(2).gameObject;
        Projectile projectile = obj.GetComponent<Projectile>();
        switch(useSkill) 
        { 
            case 1:
                projectile.Init(ProjectileType.Laser, pos, skill1.GetKey());
                break;
            case 2:
                projectile.Init(ProjectileType.Laser, pos, skill2.GetKey());
                break;
            case 3:
                projectile.Init(ProjectileType.Laser, pos, skill3.GetKey());
                break;
        }
        projectile.AttackLaser(player.transform.rotation);
    }

    public void SpawnSlash(Vector3 pos)
    {
        GameObject obj = projectileManager.GetFromPool<Projectile>(3).gameObject;
        Projectile projectile = obj.GetComponent<Projectile>();
        switch (useSkill)
        {
            case 1:
                projectile.Init(ProjectileType.Laser, pos, skill1.GetKey());
                break;
            case 2:
                projectile.Init(ProjectileType.Laser, pos, skill2.GetKey());
                break;
            case 3:
                projectile.Init(ProjectileType.Laser, pos, skill3.GetKey());
                break;
        }
        projectile.AttackSlash(player.transform.rotation);
    }


    public float TakeDamage(float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        float weaponPhysics = weapon.Physics * (1 - Creature_Physics_Cut) * weapon.CriticalMag()
            + (weapon.Fire * (1 - Creature_Fire_Cut)) + (weapon.Water * (1 - Creature_Water_Cut))
            + (weapon.Electric * (1 - Creature_Electric_Cut)) + (weapon.Ice * (1 - Creature_Ice_Cut))
            + (weapon.Wind * (1 - Creature_Wind_Cut));
        float skillPhysics;
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
            case 4:
                skillPhysics = ultimate.GetDamageA() + ultimate.GetDamageB() + ultimate.GetLevel();
                break;
            case 41:
                skillPhysics = connectedU.GetDamageA() + connectedU.GetDamageB() + connectedU.GetLevel();
                break;
            default:
                skillPhysics = basic.GetDamageA() + (basic.GetDamageB() * basic.GetLevel());
                break;
        }
        return weaponPhysics * skillPhysics;
    }

    public float TakeDamageByKey(int key, float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        float weaponPhysics = weapon.Physics * (1 - Creature_Physics_Cut) * weapon.CriticalMag()
            + (weapon.Fire * (1 - Creature_Fire_Cut)) + (weapon.Water * (1 - Creature_Water_Cut))
            + (weapon.Electric * (1 - Creature_Electric_Cut)) + (weapon.Ice * (1 - Creature_Ice_Cut))
            + (weapon.Wind * (1 - Creature_Wind_Cut));
        TableEntity_Skill skill;
        GameManager.Inst.GetSkillData(key, out skill);
        float skillPhysics = 0;
        if (skill != null)
        {
            skillPhysics = skill.Damage_A + skill.Damage_B * skill.Skill_Level;
        }
        return skillPhysics * skillPhysics;
    }

    public void SetPulledPoint(Vector3 point)
    {
        pulledPoint = point;
    }

    public void TakeDamageOther(AttackType attack, Collider other)
    {
        if (useSkill > -1)
        {
            if (other.TryGetComponent<IDamage>(out IDamage creatureDamage))
            {
                creatureDamage.CalculateDamage(attack, this);
                TableEntity_Skill skill;
                switch (useSkill)
                {
                    case 1:
                        GameManager.Inst.GetSkillData(skill1.GetKey(), out skill);
                        break;
                    case 2:
                        GameManager.Inst.GetSkillData(skill2.GetKey(), out skill);
                        break;
                    case 3:
                        GameManager.Inst.GetSkillData(skill3.GetKey(), out skill);
                        break;
                    case 4:
                        GameManager.Inst.GetSkillData(ultimate.GetKey(), out skill);
                        break;
                    case 41:
                        GameManager.Inst.GetSkillData(connectedU.GetKey(), out skill);
                        break;
                    default:
                        GameManager.Inst.GetSkillData(basic.GetKey(), out skill);
                        break;
                }
                if(skill != null)
                {
                    switch (crowdControl)
                    {
                        case CrowdControl.Stun:
                            creatureDamage.Stun(skill.Stun_Time);
                            break;
                        case CrowdControl.Airborne:
                            creatureDamage.Airborne(skill.Airborne_Time);
                            break;
                        case CrowdControl.Knockback:
                            creatureDamage.Knockback(skill.Knockback_Distance);
                            break;
                        case CrowdControl.Airback:
                            creatureDamage.Airback(skill.Airborne_Time, skill.Knockback_Distance);
                            break;
                        case CrowdControl.Pulled:
                            creatureDamage.Pulled(pulledPoint);
                            break;
                    }
                }
            }
        }
    }

    public void TakeDamageByKey(AttackType attack, int key, Collider other)
    {
       
        if (other.TryGetComponent<IDamage>(out IDamage creatureDamage))
        {
            creatureDamage.CalculateDamageByKey(attack, key, this);
            TableEntity_Skill skill;
            GameManager.Inst.GetSkillData(key, out skill);
            if (skill != null)
            {
                switch (crowdControl)
                {
                    case CrowdControl.Stun:
                        creatureDamage.Stun(skill.Stun_Time);
                        break;
                    case CrowdControl.Airborne:
                        creatureDamage.Airborne(skill.Airborne_Time);
                        break;
                    case CrowdControl.Knockback:
                        creatureDamage.Knockback(skill.Knockback_Distance);
                        break;
                    case CrowdControl.Airback:
                        creatureDamage.Airback(skill.Airborne_Time, skill.Knockback_Distance);
                        break;
                    case CrowdControl.Pulled:
                        creatureDamage.Pulled(pulledPoint);
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
