using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private PoolManager pool;
    private GameObject player;

    private void Awake()
    {
        if (!TryGetComponent<PoolManager>(out pool))
        {
            Debug.Log("SpawnManager - Awake - poolManager");
        }
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.Log("SpawnManager - Awake - GameObject");
        }
        
        StartCoroutine(Spawn());

    }

    private IEnumerator Spawn()
    {
        yield return YieldInstructionCache.WaitForSeconds(2f);
        for (int i = 0; i < 30; i++)
        {
            GameObject obj = pool.GetFromPool<Creture>(2).gameObject;
            Creture creture = obj.GetComponent<Creture>();
            creture.Init(new Vector3(Random.Range(-10f, 10f) + player.transform.position.x , 0f, Random.Range(-10f, 10f) + player.transform.position.z), 1000, CretureType.Normal);
            yield return YieldInstructionCache.WaitForSeconds(2f);
            obj = pool.GetFromPool<Creture>(3).gameObject;
            creture = obj.GetComponent<Creture>();
            creture.Init(new Vector3(Random.Range(-10f, 10f) + player.transform.position.x, 0f, Random.Range(-10f, 10f) + player.transform.position.z), 2000,CretureType.Noble);
            yield return YieldInstructionCache.WaitForSeconds(2f);
        }
    }
}
