using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AttackArea : MonoBehaviour
{
    private SphereCollider sphereCol;
    private SkillManager skillManager;
    private List<Collider> targets;
    private ParticleSystem view;
    private ParticleSystem full;
    private bool isListUp;

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
        if (!GameObject.Find("FullAttackAreaView").TryGetComponent<ParticleSystem>(out full))
        {
            Debug.Log("AttackArea - Init - ParticleSystem");
        }

        sphereCol.enabled = false;
        targets = new List<Collider>();
        view.Stop();
        full.Stop();
        isListUp = false;
    }

    public void Attack(Vector3 center, float radius)
    {
        sphereCol.enabled = false;
        sphereCol.enabled = true;
        sphereCol.center = center;
        sphereCol.radius = radius;
    }

    public void StopAttack()
    {
        sphereCol.enabled = false;
    }

    public void SetTargets()
    {
        isListUp = true;
        sphereCol.enabled = true;
        sphereCol.center = Vector3.zero;
        sphereCol.radius = 35;
    }


    public void AttackInAngle()
    {
        StartCoroutine(Attacking());
    }

    private IEnumerator Attacking()
    {
        yield return null;
        for (int i = 0; i < targets.Count; i++)
        {
            float inside = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(targets[i].transform.position.x - transform.position.x, targets[i].transform.position.z - transform.position.z));
            if (Mathf.Abs(inside) < 30)
            {
                skillManager.SetCrowdControl(CrowdControlType.Airback);
                skillManager.TakeDamageByKey(AttackType.AttackArea, 201022123, targets[i]);
            }
        }
        StopAttack();
        isListUp = false;
        targets.Clear();
    }

    public void Move(Vector3 center, float radius)
    {
        sphereCol.center = center;
        sphereCol.radius = radius;
    }
    public void StartView()
    {
        full.Play();
        view.transform.position = transform.root.position + sphereCol.center + new Vector3(0,0.2f,0);
        view.startSize = sphereCol.radius * 2;
        view.Play();
    }

    public void StopView()
    {
        view.Clear();
        full.Clear();
    }

    public Vector3 GetCenter()
    {
        return sphereCol.center;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Creature")
        {
            if (isListUp)
            {
                targets.Add(other);
            }
            else
            {
                skillManager.TakeDamageOther(AttackType.AttackArea, other);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isListUp && other.tag == "Creature")
        {
            targets.Add(other);
            targets.Distinct();
        }
    }
}