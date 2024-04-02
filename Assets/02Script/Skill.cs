using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Skill : MonoBehaviour,ITakeDamage
{
    private PlayerController player;
    private PlayerAnimationController anim;
    private Weapon weapon;
    private AttackArea attackArea;
    private PoolManager pool;
    private float damage;
    private List<Effect> effects;
    private float current_skill_ID;
    public SkillButton skillButton;
    public void SetSkill(float value)
    {
        current_skill_ID = value;
        //to getSkill name & start corutine
    }


    private float CalculateDamage(float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        return weapon.Physics * (1 - Creature_Physics_Cut) * weapon.CriticalMag()
            + (weapon.Fire * (1 - Creature_Fire_Cut)) + (weapon.Water * (1 - Creature_Water_Cut))
            + (weapon.Electric * (1 - Creature_Electric_Cut)) + (weapon.Ice * (1 - Creature_Ice_Cut))
            + (weapon.Wind * (1 - Creature_Wind_Cut));

    }
    public float TakeDamage()
    {
        return damage;
    }

    public void ResetDamage()
    {
        damage = 0;
    }

    public void TakeDamageOther(Collider other)
    {
        if(other.TryGetComponent<Creture>(out Creture creature))
        {
            damage = CalculateDamage(creature.PhysicsCut, creature.FireCut, creature.WaterCut, creature.ElectricCut, creature.IceCut, creature.WindCut);
        }
        if (other.TryGetComponent<IDamage>(out IDamage creatureDamage))
        {
            creatureDamage.GetDamage(this);    
        }
    }

}
