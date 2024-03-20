using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class CretureSpawner : MonoBehaviour
{
    private PoolManager pool;

    private void Awake()
    {
        if(!TryGetComponent<PoolManager>(out pool))
        {
            Debug.Log("CretureSpawner - Awake - poolManager");
        }
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for(int i = 0; i < 30; i++)
        {
            GameObject obj = pool.GetFromPool<Creture>(0).gameObject;
            Creture creture = obj.GetComponent<Creture>();
            creture.Init(new Vector3(Random.Range(-30f, 30f), 0f, Random.Range(-30f,30f)),CretureType.Normal);
            yield return YieldInstructionCache.WaitForSeconds(5f);
        }
    }
}
