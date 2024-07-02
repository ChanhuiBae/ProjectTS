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
    private int spawn;


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

        spawn = Random.Range(1, 8);

        StartCoroutine(SpawnLogic());
        StartCoroutine(SpawnGuvnor());
    }

    private void Update()
    {
        outline.center = player.transform.position;
    }

    private void Spawn(int num)
    {
        Creture creature;

        creature = pool.GetFromPool<Creture>(num);
        if(creature == null)
        {
            Debug.Log("Can't get Creature from Pool");
            return;
        }

        switch (spawn)
        {
            case 1:
                creature.Init(spawn1 + player.transform.position, 1000, CretureType.Normal);
                break;
            case 2:
                creature.Init(spawn2 + player.transform.position, 1000, CretureType.Normal);
                break;
            case 3:
                creature.Init(spawn3 + player.transform.position, 1000, CretureType.Normal);
                break;
            case 4:
                creature.Init(spawn4 + player.transform.position, 1000, CretureType.Normal);
                break;
            case 5:
                creature.Init(spawn5 + player.transform.position, 1000, CretureType.Normal);
                break;
            case 6:
                creature.Init(spawn6 + player.transform.position, 1000, CretureType.Normal);
                break;
            case 7:
                creature.Init(spawn7 + player.transform.position, 1000, CretureType.Normal);
                break;
            case 8:
                creature.Init(spawn8 + player.transform.position, 1000, CretureType.Normal);
                break;
        }
        spawn += 3;
        if (spawn > 8)
        {
            spawn -= 8;
        }
        spawn = Random.Range(spawn, spawn + 2);
    }

    private IEnumerator SpawnLogic()
    {
        for (int i = 0; i < 264; i++)
        {
            Spawn(3);
            yield return YieldInstructionCache.WaitForSeconds(0.17f);
        }
        //00분 44초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 264; i++)
        {
            Spawn(3);
            if (i % 18 == 0)
            {
                Spawn(6);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.17f);
        }
        //01분 29초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for(int i = 0; i < 174; i += 3)
        {
            Spawn(3);
            if(i % 2 == 0)
            {
                Spawn(6);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
        //01분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 354; i++)
        {
            if (i % 1.5f == 0)
            {
                Spawn(3);
            }
            if (i % 3 == 0)
            {
                Spawn(4);
            }
            if (i % 30 == 0)
            {
                Spawn(6);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.17f);
        }
        //02분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 1180; i++)
        {
            Spawn(3);
            yield return YieldInstructionCache.WaitForSeconds(0.05f);
        }
        //03분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 1770; i++)
        {
            if(i % 4 == 0)
            {
                Spawn(3);
            }
            if(i % 75 == 0)
            {
                Spawn(6);
            }
            yield return null;
            yield return null;
        }
        //04분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        int swarm = Random.Range(0, 1);
        swarm = swarm == 0 ? 9: 11;
        Spawn(swarm);
        for (int i = 0; i < 264; i++)
        {
            Spawn(4);
            yield return YieldInstructionCache.WaitForSeconds(0.17f);
        }
        //5분 44초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 264; i++)
        {
            Spawn(4);
            if(i % 18 == 0)
            {
                Spawn(7);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.17f);
        }
        //6분 29초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 69.6; i++)
        {
            Spawn(4);
            if(i % 2 == 0)
            {
                Spawn(7);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.415f);
        }
        //6분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 3540; i++)
        {
            if (i % 13 == 0)
            {
                Spawn(4);
            }
            if (i % 25 == 0)
            {
                Spawn(5);
            }
            if(i % 280 == 0)
            {
                Spawn(7);
            }
            yield return null;
        }
        //7분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 1770; i++)
        {
            Spawn(4);
            yield return null;
            yield return null;
        }
        //8분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 3540; i++)
        {
            if(i % 7 == 0)
            {
                Spawn(4);
            }
            if(i % 135 == 0)
            {
                Spawn(7);
            }
            yield return null;
        }
        //9분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        swarm = Random.Range(0, 1);
        swarm = swarm == 0 ? 10 : 12;
        Spawn(swarm);
        for (int i = 0; i < 264; i++)
        {
            Spawn(5);
            yield return YieldInstructionCache.WaitForSeconds(0.17f);
        }
        //10분 44초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 264; i++)
        {
            Spawn(5);
            if (i % 18 == 0)
            {
                Spawn(8);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.17f);
        }
        // 11분 29초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 87; i++)
        {
            Spawn(5);
            if (i % 2 == 0)
            {
                Spawn(8);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.33f);
        }
        // 11분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 1770; i++)
        {
            Spawn(5);
            yield return null;
            yield return null;
        }
        // 12분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 1770; i++)
        {
            Spawn(5);
            if(i % 30 == 0)
            {
                Spawn(8);
            }
            yield return null;
            yield return null;
        }
        // 13분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 590; i++)
        {
            Spawn(3);
            Spawn(4);
            Spawn(5);
            if (i % 10 == 0)
            {
                Spawn(6);
                Spawn(7);
                Spawn(8);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        // 14분 59초
    }

    private IEnumerator SpawnGuvnor()
    {
        yield return YieldInstructionCache.WaitForSeconds(900);
        Spawn(13);
    }

    public void SpawnHPItem(Vector3 pos)
    {
        HPItem hp = pool.GetFromPool<HPItem>(0);
        hp.Init(1f, pos);
    }

    public void SpawnEXPItem(Vector3 pos,float dropEXP)
    {
        EXPItem exp = pool.GetFromPool<EXPItem>(1);
        exp.Init(dropEXP, pos);
    }

    public void SpawnFloatingDamage(Vector3 pos, float damage)
    {
        FloatingDamage fd = pool.GetFromPool<FloatingDamage>(2);
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
