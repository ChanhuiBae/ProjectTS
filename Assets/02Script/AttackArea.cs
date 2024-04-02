using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackArea : MonoBehaviour, ITakeDamage
{
    private BoxCollider boxCol;
    private SphereCollider sphereCol;
    private float damage;

    public void Init()
    {
 
        if(!TryGetComponent<SphereCollider>(out sphereCol))
        {
            Debug.Log("AttackArea - Init - SphereCollider");
        }
        else
        {
            sphereCol.enabled = false;
        }

        damage = 0;
       
    }

    public void Attack(float calculateDamage)
    {
        damage = calculateDamage;
        sphereCol.enabled = true;

    }

    public void Reset()
    {
        damage = 0;
        sphereCol.enabled = false;
    }

    public float TakeDamage()
    {
        return damage;
    }

    public float TakeDamage(float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        return damage;
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
