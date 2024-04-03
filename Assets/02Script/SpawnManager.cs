using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private PoolManager pool;
    private TextMeshProUGUI countdown;
    private int time;

    private void Awake()
    {
        if (!TryGetComponent<PoolManager>(out pool))
        {
            Debug.Log("SpawnManager - Awake - poolManager");
        }
        if(!GameObject.Find("Time").TryGetComponent<TextMeshProUGUI>(out countdown))
        {
            Debug.Log("SpawnMananger - Awake - TextMeshProUGUI");
        }
        time = 1200;
        StartCoroutine(Spawn());
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while(time > 0)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
            time--;
            countdown.text = (time / 60).ToString() + ":" + (time % 60).ToString();
        }
    }

    private IEnumerator Spawn()
    {
        for (int i = 0; i < 30; i++)
        {
            GameObject obj = pool.GetFromPool<Creture>(2).gameObject;
            Creture creture = obj.GetComponent<Creture>();
            creture.Init(new Vector3(Random.Range(-30f, 30f), 0f, Random.Range(-30f, 30f)), CretureType.Normal);
            yield return YieldInstructionCache.WaitForSeconds(2f);
            obj = pool.GetFromPool<Creture>(3).gameObject;
            creture = obj.GetComponent<Creture>();
            creture.Init(new Vector3(Random.Range(-30f, 30f), 0f, Random.Range(-30f, 30f)), CretureType.Noble);
            yield return YieldInstructionCache.WaitForSeconds(2f);
        }
    }
}
