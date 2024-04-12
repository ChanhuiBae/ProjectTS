using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private string poolName;
    private SkillManager skillManager;

    private void Awake()
    {
        if(!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("Effect - Awake - SkillManager");
        }
    }
    public void Init(Vector3 pos, float lifeFrame)
    {
        transform.position = pos;
        StartCoroutine(ReturnPool(lifeFrame));
    }

    public void SetScale(float size)
    {
        transform.LeanScale(Vector3.one * size,0);
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
}
