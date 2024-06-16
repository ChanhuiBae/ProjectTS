using Redcode.Pools;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private PoolManager pool;
    private PoolManager effects;
    private GameObject player;
    private SphereCollider outline;
    private float radius;
    private Vector3 spawn1;
    private Vector3 spawn2;
    private Vector3 spawn3;
    private Vector3 spawn4;
    private Vector3 spawn5;
    private Vector3 spawn6;
    private Vector3 spawn7;
    private Vector3 spawn8;
    private int count;


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
        if(!TryGetComponent<SphereCollider>(out outline))
        {
            Debug.Log("SpawnManager - Awake - SphereCollider");
        }
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.Log("SpawnManager - Awake - GameObject");
        }

        count = 1;
        radius = Screen.width / 2;
        outline.radius = radius + 10f;
        spawn1 = new Vector3(0, 0, radius);
        float pos = Mathf.Pow(radius, 2) / Mathf.Sqrt(2);
        spawn2 = new Vector3(- pos, 0, pos);
        spawn3 = new Vector3(-radius, 0, 0);
        spawn4 = new Vector3(-pos, 0, -pos);
        spawn5 = new Vector3(0,0,-radius);
        spawn6 = new Vector3(pos,0,-pos);
        spawn7 = new Vector3(radius, 0 ,0);
        spawn8 = new Vector3(pos, 0, pos);

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return YieldInstructionCache.WaitForSeconds(2f);
        GameObject obj;
        Creture creture;
        int spawn = Random.Range(1, 8);

        for (int i = 0; i < 30; i++)
        {
            switch (spawn)
            {
                case 1:
                    
                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
            }
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
