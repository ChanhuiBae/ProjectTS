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
        gameObject.SetActive(false);
    }

    public void ActiveFalse()
    {
        gameObject.SetActive(false);
    }

    public void Attack(Vector3 center, float radius)
    {
        gameObject.SetActive(true);
        sphereCol.center = center;
        sphereCol.radius = radius;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Creture")
        {
            skillManager.TakeDamageOther(other);
        }
    }
}