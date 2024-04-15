using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackArea : MonoBehaviour
{
    private SphereCollider sphereCol;
    private SkillManager skillManager;
    private List<Collider> targets;
    private ParticleSystem view;
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
        if (!transform.GetChild(0).TryGetComponent<ParticleSystem>(out view))
        {
            Debug.Log("AttackArea - Init - ParticleSystem");
        }

        sphereCol.enabled = false;
        targets = new List<Collider>();
        view.Stop();
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


    public void AttackInAngle()
    {
        sphereCol.enabled = true;
        sphereCol.center = Vector3.zero;
        sphereCol.radius = 10;
        for (int i = 0; i < targets.Count; i++)
        {
            float inside = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(targets[i].transform.position.x, targets[i].transform.position.z));
            if(Mathf.Abs(inside) < 15) 
            {
                skillManager.TakeDamageOther("AttackArea", targets[i]);
            }
        }
        targets.Clear();
    }

    public void StartView()
    {
        view.startSize = sphereCol.radius * 2;
        view.Play();
    }

    public void StopView()
    {
        view.Stop();
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