using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private string poolName;
    private Skill skillManager;

    private void Awake()
    {
        if(!GameObject.Find("SkillManager").TryGetComponent<Skill>(out skillManager))
        {
            Debug.Log("Effect - Awake - SkillManager");
        }
    }
    public void Init(Vector3 pos, int lifeFrame)
    {
        transform.position = pos;
        StartCoroutine(ReturnPool(lifeFrame));
    }

    private IEnumerator ReturnPool(int lifeFrame)
    {
        yield return YieldInstructionCache.WaitForSeconds(lifeFrame * 0.017f);
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
