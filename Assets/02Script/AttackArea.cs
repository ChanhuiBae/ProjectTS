using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackArea : MonoBehaviour
{
    private SphereCollider sphereCol;
    private SkillManager skillManager;
    private List<Collider> targets;
    private bool IsListUp;

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
        targets = new List<Collider>();
        IsListUp = false;
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


    public void AttackInAngle(float angle)
    {
        
        for (int i = 0; i < targets.Count; i++)
        {
            float inside = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(targets[i].transform.position.x, targets[i].transform.position.z));
            if(Mathf.Abs(inside) < angle/2) 
            {
                skillManager.TakeDamageOther("AttackArea", targets[i]);
            }
        }
        targets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Creature")
        {
            if(IsListUp)
            {
                targets.Add(other);
            }
            else
            {
                skillManager.TakeDamageOther("AttackArea", other);
            }
        }
    }
}