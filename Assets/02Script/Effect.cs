using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    None,
    Once,
    Multiple
}

public class Effect : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private string poolName;
    private SkillManager skillManager;
    private EffectType type;  
    private int key;
    private bool hit;
    public int Key
    {
        set { key = value; }
    }

    private void Awake()
    {
        if(!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("Effect - Awake - SkillManager");
        }
    }
    public void Init(EffectType type, Vector3 pos, float lifeTime)
    {
        this.type = type;
        transform.position = pos;
        StartCoroutine(ReturnPool(lifeTime));
    }

    public void SetScale(float size)
    {
        transform.LeanScale(Vector3.one * size,0);
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    private IEnumerator ReturnPool(float lifeTime)
    {
        yield return YieldInstructionCache.WaitForSeconds(lifeTime);
        skillManager.TakeEffect(poolName, this);
    }

    public void ReturenEffect()
    {
        StopAllCoroutines();
        skillManager.TakeEffect(poolName, this);
    }

    public void OnCreatedInPool()
    {
        //throw new System.NotImplementedException();
    }

    public void OnGettingFromPool()
    {
        //throw new System.NotImplementedException();
    }

    public void StayCount(int count)
    {
        Debug.Log(key);
        StartCoroutine(StartStayCount(count));
    }

    private IEnumerator StartStayCount(int count)
    {
        int i = 0;
        while (i < count)
        {
            if(i % 5 == 0)
            {
                hit = true;
            }
            else
            {
                hit = false;
            }
            i++;
            yield return null;
        }
        Debug.Log("End");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Creature")
        {
            if (type == EffectType.Once)
                skillManager.TakeDamageOther(AttackType.Effect, other);
            else if (type == EffectType.Multiple && hit)
            {
                skillManager.TakeDamageByKey(AttackType.Effect, key, other);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Creature" && hit)
        {
            if (type == EffectType.Multiple)
            {
                skillManager.TakeDamageByKey(AttackType.Effect, key, other);
            }
        }
    }
}
