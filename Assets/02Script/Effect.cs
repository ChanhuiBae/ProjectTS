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
    public void Init(EffectType type, Vector3 pos, float lifeFrame)
    {
        this.type = type;
        transform.position = pos;
        StartCoroutine(ReturnPool(lifeFrame));
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Creature")
        {
            if (type == EffectType.Once)
                skillManager.TakeDamageOther(AttackType.Effect, other);
            else if (type == EffectType.Multiple)
                skillManager.TakeDamageByKey(AttackType.Effect, key, other);
        }
    }
}
