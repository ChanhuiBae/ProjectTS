using Redcode.Pools;
using System.Collections;
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
        radius = Screen.width / 50;
        outline.radius = radius;
        spawn1 = new Vector3(0, 0, radius);
        float pos = radius / Mathf.Sqrt(2);
        spawn2 = new Vector3(- pos, 0, pos);
        spawn3 = new Vector3(-radius, 0, 0);
        spawn4 = new Vector3(-pos, 0, -pos);
        spawn5 = new Vector3(0,0,-radius);
        spawn6 = new Vector3(pos,0,-pos);
        spawn7 = new Vector3(radius, 0 ,0);
        spawn8 = new Vector3(pos, 0, pos);

        StartCoroutine(Spawn());
    }

    private void Update()
    {
        outline.center = player.transform.position;
    }

    private IEnumerator Spawn()
    {
        yield return YieldInstructionCache.WaitForSeconds(2f);
        GameObject obj;
        Creture creture;
        int spawn = Random.Range(1, 8);
        for (int i = 0; i < 30; i++)
        {
            
            obj = pool.GetFromPool<Creture>(2).gameObject;
            creture = obj.GetComponent<Creture>();
            switch (spawn)
            {
                case 1:
                    creture.Init(spawn1 + player.transform.position, 1000, CretureType.Normal);
                    break;
                case 2:
                    creture.Init(spawn2 + player.transform.position, 1000, CretureType.Normal);
                    break;
                case 3:
                    creture.Init(spawn3 + player.transform.position, 1000, CretureType.Normal);
                    break;
                case 4:
                    creture.Init(spawn4 + player.transform.position, 1000, CretureType.Normal);
                    break;
                case 5:
                    creture.Init(spawn5 + player.transform.position, 1000, CretureType.Normal);
                    break;
                case 6:
                    creture.Init(spawn6 + player.transform.position, 1000, CretureType.Normal);
                    break;
                case 7:
                    creture.Init(spawn7 + player.transform.position, 1000, CretureType.Normal);
                    break;
                case 8:
                    creture.Init(spawn8 + player.transform.position, 1000, CretureType.Normal);
                    break;
            }
            yield return YieldInstructionCache.WaitForSeconds(1f);
            spawn += 3;
            if(spawn > 8)
            {
                spawn -= 8;
            }
            spawn = Random.Range(spawn, spawn + 2);
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

    private void OnTriggerExit(Collider other)
    {
        switch (count)
        {
            case 1:
                other.transform.position = spawn1 + player.transform.position;
                break;
            case 2:
                other.transform.position = spawn2 + player.transform.position;
                break;
            case 3:
                other.transform.position = spawn3 + player.transform.position;
                break;
            case 4:
                other.transform.position = spawn4 + player.transform.position;
                break;
            case 5:
                other.transform.position = spawn5 + player.transform.position;
                break;
            case 6:
                other.transform.position = spawn6 + player.transform.position;
                break;
            case 7:
                other.transform.position = spawn7 + player.transform.position;
                break;
            case 8:
                other.transform.position = spawn8 + player.transform.position;
                break;
        }
        count++;
        if (count > 8)
        {
            count = 1;
        }
    }
}
