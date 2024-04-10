using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackArea : MonoBehaviour
{
    private SphereCollider sphereCol;
    private SkillManager skillManager;

    public void Awake()
    {
 
        if(!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("AttackArea - Init - SkillManager");
        }
        if(!TryGetComponent<SphereCollider>(out sphereCol))
        {
            Debug.Log("AttackArea - Init - SphereCollider");
        }
        sphereCol.enabled = false;
    }

    public void Attack(Vector3 center, float radius)
    {
        sphereCol.enabled = true;
        sphereCol.center = center;
        sphereCol.radius = radius;
    }

    public void StopAttack()
    {
        sphereCol.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Creture")
        {
            skillManager.TakeDamageOther(other);
        }

    }
}