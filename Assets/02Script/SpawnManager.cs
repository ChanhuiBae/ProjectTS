using Redcode.Pools;
using System.Collections;
using System.Text.RegularExpressions;
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
        GameObject obj = pool.GetFromPool<Creture>(4).gameObject;
        Creture creture = obj.GetComponent<Creture>();
        creture.Init(new Vector3(Random.Range(-10f, 10f) + player.transform.position.x, 0f, Random.Range(-10f, 10f) + player.transform.position.z), 1000, CretureType.Normal);
        yield return YieldInstructionCache.WaitForSeconds(1f);

        for (int i = 0; i < 30; i++)
        {
            obj = pool.GetFromPool<Creture>(2).gameObject;
            creture = obj.GetComponent<Creture>();
            creture.Init(new Vector3(Random.Range(-10f, 10f) + player.transform.position.x , 0f, Random.Range(-10f, 10f) + player.transform.position.z), 1000, CretureType.Normal);
            yield return YieldInstructionCache.WaitForSeconds(1f);
            obj = pool.GetFromPool<Creture>(3).gameObject;
            creture = obj.GetComponent<Creture>();
            creture.Init(new Vector3(Random.Range(-10f, 10f) + player.transform.position.x, 0f, Random.Range(-10f, 10f) + player.transform.position.z), 2000,CretureType.Noble);
            yield return YieldInstructionCache.WaitForSeconds(1f);
        }
    }

    public void SpawnHPItem(Vector3 pos)
    {
        HPItem hp = pool.GetFromPool<HPItem>(0);
        hp.Init(1f, pos);
    }

    public void SpawnEXPItem(Vector3 pos)
    {
        EXPItem exp = pool.GetFromPool<EXPItem>(1);
        exp.Init(1f, pos);
    }

    public void SpawnFloatingDamage(Vector3 pos, float damage)
    {
        FloatingDamage fd = pool.GetFromPool<FloatingDamage>(5);
        fd.Init(pos, damage);
    }

    public void ReturnCreature(string name, Creture creture)
    {
        pool.TakeToPool<Creture>(name, creture);
    }

    public Effect SpawnEffect(int num)
    {
        GameObject obj = effects.GetFromPool<Effect>(num).gameObject;
        return obj.GetComponent<Effect>();
    }
}
