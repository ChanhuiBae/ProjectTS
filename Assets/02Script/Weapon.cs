using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum WeaponType
{
    Sowrd = 0,
    Hamer = 1,
    Gun = 2
}
public class Weapon : MonoBehaviour, ITakeDamage
{
    private WeaponType type;
    private float damage;
    private float Weapon_Physics;
    private float Weapon_Fire;
    private float Weapon_Water;
    private float Weapon_Electric;
    private float Weapon_Ice;
    private float Weapon_Wind;
    private PlayerController owner;
    private PoolManager pool;

    private void Awake()
    {
        if(!GameObject.Find("Player").TryGetComponent<PlayerController>(out owner))
        {
            Debug.Log("Weapon - Awake - PlayerController");
        }   
    }

    public void Init(WeaponType type)
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
    }

    public WeaponType GetType()
    {
        return type;
    }
    
    public void ResetDamage()
    {
        damage = 0;
    }

    private IEnumerator DamageZero()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        damage = 0;
    }

    public void NormalAttack()
    {
        if(damage == 0)
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

    public void CalculateDamage(float Critical_Mag, float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        damage = Weapon_Physics * (1 - Creature_Physics_Cut) * Critical_Mag
            + (Weapon_Fire * (1 - Creature_Fire_Cut)) + (Weapon_Water * (1 - Creature_Water_Cut)) 
            + (Weapon_Electric * (1 - Creature_Electric_Cut)) + (Weapon_Ice * (1 - Creature_Ice_Cut)) 
            + (Weapon_Wind * (1 - Creature_Wind_Cut));

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
                creture.GetDamage(this);
            }
        }
    }
}
