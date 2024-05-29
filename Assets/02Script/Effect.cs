using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static UnityEditor.PlayerSettings;

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

    public void InitNotTime(EffectType type, Vector3 pos)
    {
        this.type = type;
        transform.position = pos;
    }

    public void InitFollow(EffectType type, GameObject target, float lifeTime)
    {
        this.type = type;
        StartCoroutine(FollowTarget(target, lifeTime));
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

    public void MoveForward()
    {
        transform.LeanMove(transform.forward *30, 2f);
    }

    private IEnumerator ReturnPool(float lifeTime)
    {
        yield return YieldInstructionCache.WaitForSeconds(lifeTime);
        skillManager.TakeEffect(poolName, this);
    }
    private IEnumerator FollowTarget(GameObject target, float lifeTime)
    {
        float i = 0;
        while (i < lifeTime)
        {
            yield return null;
            i += 0.017f;
            transform.position = target.transform.position;
        }
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

    public void Powerwave(float time, float distance)
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, time);
        LeanTween.move(gameObject, transform.position + transform.forward * distance, time);
    }

    public void StayCount(int count, float attackSpeed)
    {
        StartCoroutine(StartStayCount(count, attackSpeed));
    }

    private IEnumerator StartStayCount(int MaxCount, float attackSpeed)
    {
        int i = 0;
        int count;

        if (attackSpeed < 1.5)
            count = 10;
        else if(attackSpeed < 2.0)
            count = 9;
        else if(attackSpeed < 2.5)
            count = 8;
        else if (attackSpeed < 3.0)
            count = 7;
        else if (attackSpeed < 3.5)
            count = 6;
        else
            count = 5;

        while (i < MaxCount)
        {
            if(i % count == 0)
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
