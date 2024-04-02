using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackArea : MonoBehaviour
{
    private SphereCollider sphereCol;
    private Skill skillManager;

    public void Awake()
    {
 
        if(!GameObject.Find("SkillManager").TryGetComponent<Skill>(out skillManager))
        {
            Debug.Log("AttackArea - Init - SkillManager");
        }
        if(!TryGetComponent<SphereCollider>(out sphereCol))
        {
            Debug.Log("AttackArea - Init - SphereCollider");
        }
        gameObject.SetActive(false);
    }

    public void Attack()
    {
        gameObject.SetActive(true);

    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Creture")
        {
            skillManager.TakeDamageOther(other);
        }
    }
}