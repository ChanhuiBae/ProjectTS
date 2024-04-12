using Redcode.Pools;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private PoolManager pool;
    private PoolManager effects;
    private GameObject player;

    private void Awake()
    {
        if (!TryGetComponent<PoolManager>(out pool))
        {
            Debug.Log("SpawnManager - Awake - poolManager");
        }
        if (!GameObject.Find("EffectManager").TryGetComponent<PoolManager>(out effects))
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

    public void SpawnHPItem(Vector3 pos)
    {
        GameObject obj = pool.GetFromPool<HPItem>(0).gameObject;
        HPItem hp = obj.GetComponent<HPItem>();
        hp.Init(1f, pos);

    }

    public void SpawnEXPItem(Vector3 pos)
    {
        GameObject obj = pool.GetFromPool<EXPItem>(1).gameObject;
        EXPItem exp = obj.GetComponent<EXPItem>();
        exp.Init(1f, pos);
    }

    public void ReturnCreature(string name, Creture creture)
    {
        pool.TakeToPool<Creture>(name, creture);
    }

    public void SpawnEffect(int num, Vector3 pos, int lifeFrame)
    {
        GameObject obj = effects.GetFromPool<Effect>(num).gameObject;
        Effect effect = obj.GetComponent<Effect>();
        effect.Init(pos, lifeFrame);
    }
}
