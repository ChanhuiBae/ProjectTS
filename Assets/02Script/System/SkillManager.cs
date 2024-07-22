using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

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
    private PlayerAnimationController anim;
    private Weapon weapon;
    private AttackArea attackArea;
    private EffectManager effect;
    
    private Skill basic;
    private Skill skill1;
    private Skill connected1;
    private Skill skill2;
    private Skill connected2;
    private Skill skill3;
    private Skill connected3;
    private Skill ultimate;
    private Skill connectedU;

    private PassiveSkill passive1;
    private PassiveSkill passive2;
    private PassiveSkill passive3;

    private float additionalDamgae;
    public float AdditionalDamgae
    {
        set => additionalDamgae = value;
        get => additionalDamgae;
    }

    private PoolManager effectManager;
    private PoolManager projectileManager;

    private int useSkill;
    private bool isCharge;
    private Vector3 playerLook;
    private Queue<Vector3> queue;
    private CrowdControlType crowdControl;
    private Vector3 pulledPoint;
    private bool counter;

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
        if(!player.TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("SkillManager - Awake - PlayerAnimationControllerr");
        }
        if (!player.TryGetComponent<EffectManager>(out effect))
        {
            Debug.Log("SkillManager - Awake - EffectManager");
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

        if(!transform.Find("Passive1").TryGetComponent<PassiveSkill>(out passive1))
        {
            Debug.Log("SkillManager - Awake - Passive1");
        }
        if (!transform.Find("Passive2").TryGetComponent<PassiveSkill>(out passive2))
        {
            Debug.Log("SkillManager - Awake - Passive1");
        }
        if (!transform.Find("Passive3").TryGetComponent<PassiveSkill>(out passive3))
        {
            Debug.Log("SkillManager - Awake - Passive1");
        }
        useSkill = -1;
        isCharge = false;
        crowdControl = CrowdControlType.None;
        pulledPoint = player.transform.position;
        queue = new Queue<Vector3>();
        additionalDamgae = 0;
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

    public void LevelUpSkill(int num, int id)
    {
        switch (num)
        {
            case 1:
                skill1.LevelUp(id);
                break;
            case 11:
                connected1.LevelUp(id);
                break;
            case 2:
                skill2.LevelUp(id);
                break;
            case 21:
                connected2.LevelUp(id);
                break;
            case 3:
                skill3.LevelUp(id);
                break;
            case 31:
                connected3.LevelUp(id);
                break;
            case 4:
                ultimate.LevelUp(id);
                break;
            case 41:
                connectedU.LevelUp(id);
                break;
        }
    }

    public void SetPassive(int num, TableEntitiy_Passive_Skill passive)
    {
        switch (num)
        {
            case 1:
                this.passive1.Init(passive.ID, passive.Max_Level, passive.Base_Figure, passive.Increase_Value, passive.Passive_Type); 
                break;
            case 2:
                this.passive2.Init(passive.ID, passive.Max_Level, passive.Base_Figure, passive.Increase_Value, passive.Passive_Type);
                break;
            case 3:
                this.passive3.Init(passive.ID, passive.Max_Level, passive.Base_Figure, passive.Increase_Value, passive.Passive_Type);
                break;
        }
    }

    public void LevelUpPassive(int num, int id)
    {
        switch (num)
        {
            case 1:
                passive1.LevelUp(id);
                break;
            case 2:
                passive2.LevelUp(id);
                break;
            case 3:
                passive3.LevelUp(id);
                break;
        }
    }

    public int GetCurrentSkill()
    {
        return useSkill;
    }

    public void UseSkill(int num)
    {
        counter = false;
        useSkill = num;
        switch (useSkill)
        {
            case 0:
                basic.StartSkill(0);
                break;
            case 1:
                skill1.StartSkill(1);
                break;
            case 11:
                connected1.StartSkill(11);
                break;
            case 2:
                skill2.StartSkill(2);
                break;
            case 21:
                connected2.StartSkill(21);
                break;
            case 3:
                skill3.StartSkill(3);
                break;
            case 31:
                connected3.StartSkill(31);
                break;
            case 4:
                ultimate.StartSkill(4);
                break;
            case 41:
                connectedU.StartSkill(41);
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


    public void ChargeUp(int count)
    {
        effect.ChargeUp(count);
    }

    public void SetCrowdControl(CrowdControlType type)
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

    public void SetAttackAreaRadius(float radius)
    {
        attackArea.Attack(player.transform.position, radius);
    }

    public void SetAttackArea(Vector3 center, float radius)
    {
        attackArea.Attack(center, radius);
    }

    public void MoveAttackArea(Vector3 center, float radius)
    {
        attackArea.Move(center, radius);
    }

    public void ShowAttackArea(int num)
    {
        if(num < 4)
        {
            attackArea.SetArea(1.8f);
            attackArea.ShowAttackArea();
        }
        else
        {
            attackArea.SetArea(0.8f);
            attackArea.ShowAttackArea();
        }
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
        return queue.Dequeue();
    }

    public int GetVectorCount()
    {
        return queue.Count;
    }

    public void ClearVector()
    {
        queue.Clear();
    }

    public void PinPointDown(Vector3 position)
    {
        effect.PinPointDown(position);
    }

    public int GetCurrentKey()
    {
        switch (useSkill)
        {
            case 1:
                return skill1.GetKey();
            case 11:
                return connected1.GetKey();
            case 2:
                return skill2.GetKey();
            case 21:
                return connected2.GetKey();
            case 3:
                return skill3.GetKey();
            case 31:
                return connected3.GetKey();
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
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Bullet);
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
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.APHE);
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

    public void SpawnThorn(Vector3 pos, GameObject target, int key)
    {
        Projectile p1 = projectileManager.GetFromPool<Projectile>(4);
        Projectile p2 = projectileManager.GetFromPool<Projectile>(4);
        Projectile p3 = projectileManager.GetFromPool<Projectile>(4);
        p1.Init(ProjectileType.Thorn, pos, key);
        p2.Init(ProjectileType.Thorn, pos, key);
        p3.Init(ProjectileType.Thorn, pos, key);
        p1.transform.LookAt(target.transform);
        p1.AttackThorn(p1.transform.rotation);
        p2.AttackThorn(p1.transform.rotation * Quaternion.Euler(0,15,0));
        p3.AttackThorn(p1.transform.rotation * Quaternion.Euler(0, -15, 0));
    }

    public void SpawnThornUp(Vector3 pos)
    {
        Projectile p1 = projectileManager.GetFromPool<Projectile>(4);
        p1.Init(ProjectileType.Thorn, pos, 0);
        p1.MoveUp(Quaternion.Euler(-90, 0, 0));
    }

    public void StartDrop(int key)
    {
        StartCoroutine(DropThorns(key));
    }

    private IEnumerator DropThorns(int key)
    {
        yield return null;
        for(int i = 0; i < 10; i++)
        {
            Effect effect = SpawnEffect(25);
            effect.transform.LeanScale(new Vector3(0.2f, 0.2f, 0.2f),0);
            Vector3 pos = player.transform.position + new Vector3(Random.Range(-3, 3), 0.01f, Random.Range(-3,3));
            effect.Init(EffectType.None, pos, 1f);
            yield return YieldInstructionCache.WaitForSeconds(1);
            Projectile thorn = projectileManager.GetFromPool<Projectile>(4);
            pos += Vector3.up * 15f;
            thorn.Init(ProjectileType.Thorn, pos, key);
            thorn.AttackThorn(Quaternion.Euler(90, 0, 0));
        }
    }
    
    public float TakeDamage(int creatureKey, int PatternInfoKey)
    {
        return 0;
    }

    public float TakeDamage(float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        float weaponPhysics = weapon.Physics * (1 - Creature_Physics_Cut) * weapon.CriticalMag()
            + (weapon.Fire * (1 - Creature_Fire_Cut)) + (weapon.Water * (1 - Creature_Water_Cut))
            + (weapon.Electric * (1 - Creature_Electric_Cut)) + (weapon.Ice * (1 - Creature_Ice_Cut))
            + (weapon.Wind * (1 - Creature_Wind_Cut))
            + (weapon.Physics * additionalDamgae);
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
                skillPhysics = ultimate.GetDamageA() + (ultimate.GetDamageB() * ultimate.GetLevel());
                break;
            case 41:
                skillPhysics = connectedU.GetDamageA() + (connectedU.GetDamageB() * connectedU.GetLevel());
                break;
            default:
                skillPhysics = basic.GetDamageA() + basic.GetDamageB();
                break;
        }
        return weaponPhysics * skillPhysics;
    }

    public float TakeDamage(int key, float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        float weaponPhysics = weapon.Physics * (1 - Creature_Physics_Cut) * weapon.CriticalMag()
            + (weapon.Fire * (1 - Creature_Fire_Cut)) + (weapon.Water * (1 - Creature_Water_Cut))
            + (weapon.Electric * (1 - Creature_Electric_Cut)) + (weapon.Ice * (1 - Creature_Ice_Cut))
            + (weapon.Wind * (1 - Creature_Wind_Cut))
            + (weapon.Physics * additionalDamgae);
        TableEntity_Skill_Info skill;
        GameManager.Inst.GetSkillInfoData(key, out skill);
        float skillPhysics = 0;
        if (skill != null)
        {
            skillPhysics = skill.Damage_A + skill.Damage_B * skill.Skill_Level;
        }
        return weaponPhysics * skillPhysics;
    }

    public float TakeDamage(Armor armor)
    {
        return 0;
    }

    public void SetPulledPoint(Vector3 point)
    {
        pulledPoint = point;
    }

    public void SetBoxes()
    {
        effect.SetBoxArea();
    }

    public void MoveBoxes(Vector3 look)
    {
        player.transform.LookAt(look + transform.position);
        effect.MoveBoxes(look);
    }

    public void DropMissile()
    {
        effect.DropMissile();
    }

    public void AttackAnim()
    {
        anim.Attack(true);
    }

    public void SetInvincible(float time)
    {
        player.IsInvincibility(time);
    }

    public void CounterCheck()
    {
        StartCoroutine(CounterChecking());
    }

    private IEnumerator CounterChecking()
    {
        anim.IsCombo(false);
        counter = false;
        for(int i = 0; i < 10; i++)
        {
            if (counter)
            {
                anim.IsCombo(true);
            }
            yield return null;
        }
        if(!counter)
        {
            player.SetIdle();
        }
    }

    public void TakeDamageOther(AttackType attack, Collider other)
    {
        if (useSkill > -1)
        {
            if (other.TryGetComponent<IDamage>(out IDamage creatureDamage))
            {
                if(creatureDamage.CalculateDamage(attack, this))
                {
                    counter = true;
                    TableEntity_Skill_Info skill;
                    switch (useSkill)
                    {
                        case 1:
                            GameManager.Inst.GetSkillInfoData(skill1.GetKey(), out skill);
                            break;
                        case 2:
                            GameManager.Inst.GetSkillInfoData(skill2.GetKey(), out skill);
                            break;
                        case 3:
                            GameManager.Inst.GetSkillInfoData(skill3.GetKey(), out skill);
                            break;
                        case 4:
                            GameManager.Inst.GetSkillInfoData(ultimate.GetKey(), out skill);
                            break;
                        case 41:
                            GameManager.Inst.GetSkillInfoData(connectedU.GetKey(), out skill);
                            break;
                        default:
                            GameManager.Inst.GetSkillInfoData(basic.GetKey(), out skill);
                            break;
                    }
                    if (skill != null)
                    {
                        switch (crowdControl)
                        {
                            case CrowdControlType.Airback:
                                creatureDamage.Airback(skill.Airborne_Time, skill.Knockback_Distance);
                                break;
                            case CrowdControlType.Pulled:
                                creatureDamage.Pulled(pulledPoint);
                                break;
                        }
                        if (skill.Stun_Time > 0)
                        {
                            creatureDamage.Stun(skill.Stun_Time);
                        }
                        if (skill.Stagger_Time > 0)
                        {
                            creatureDamage.Stagger(skill.Stagger_Time);
                        }
                        if (skill.Airborne_Time > 0)
                        {
                            creatureDamage.Airborne(skill.Airborne_Time);
                        }
                        if (skill.Knockback_Distance > 0)
                        {
                            creatureDamage.Knockback(skill.Knockback_Distance);
                        }
                    }
                }
            }
        }
    }

    public void TakeDamageByKey(AttackType attack, int key, Collider other)
    {
       
        if (other.TryGetComponent<IDamage>(out IDamage creatureDamage))
        {
            if(creatureDamage.CalculateDamage(attack, key, this))
            {
                if (creatureDamage.IsAttack())
                {
                    counter = true;
                }
                TableEntity_Skill_Info skill;
                GameManager.Inst.GetSkillInfoData(key, out skill);
                if (skill != null)
                {
                    switch (crowdControl)
                    {
                        case CrowdControlType.Stun:
                            creatureDamage.Stun(skill.Stun_Time);
                            break;
                        case CrowdControlType.Airborne:
                            creatureDamage.Airborne(skill.Airborne_Time);
                            break;
                        case CrowdControlType.Knockback:
                            creatureDamage.Knockback(skill.Knockback_Distance);
                            break;
                        case CrowdControlType.Airback:
                            creatureDamage.Airback(skill.Airborne_Time, skill.Knockback_Distance);
                            break;
                        case CrowdControlType.Pulled:
                            creatureDamage.Pulled(pulledPoint);
                            break;
                        case CrowdControlType.Stagger:
                            creatureDamage.Stagger(skill.Stagger_Time);
                            break;
                    }
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

    public void ATK_Passive(float passive) 
    {
        weapon.ATK_Passive = passive;
    }

    public void Speed_Passive(float passive)
    {
        weapon.Speed_Passive = passive;
    }


}
