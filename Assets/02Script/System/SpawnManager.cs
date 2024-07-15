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
    }

    private void Start()
    {
        //StartCoroutine(SpawnLogic());
       // StartCoroutine(SpawnBoss());
    }

    private void Update()
    {
        outline.center = player.transform.position;
    }

    private void Spawn(int num, int id, CretureType type)
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
                creature.Init(spawn1 + player.transform.position, id, type);
                break;
            case 2:
                creature.Init(spawn2 + player.transform.position, id, type);
                break;
            case 3:
                creature.Init(spawn3 + player.transform.position, id, type);
                break;
            case 4:
                creature.Init(spawn4 + player.transform.position, id, type); 
                break;
            case 5:
                creature.Init(spawn5 + player.transform.position, id, type);
                break;
            case 6:
                creature.Init(spawn6 + player.transform.position, id, type);
                break;
            case 7:
                creature.Init(spawn7 + player.transform.position, id, type);
                break;
            case 8:
                creature.Init(spawn8 + player.transform.position, id, type);
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
        yield return null; 

        for (int i = 0; i < 132; i++)
        {
            Spawn(3, 1000, CretureType.Normal);
            yield return YieldInstructionCache.WaitForSeconds(0.33f);
        }
        //00분 44초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 132; i++)
        {
            Spawn(3, 1000, CretureType.Normal);
            if (i % 13 == 0)
            {
                Spawn(5, 2000, CretureType.Noble);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.33f);
        }
        //01분 29초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for(int i = 0; i < 29; i++)
        {
            Spawn(3, 1000, CretureType.Normal);
            if (i % 2 == 0)
            {
                Spawn(5, 2000, CretureType.Noble);
            }
            yield return YieldInstructionCache.WaitForSeconds(1f);
        }
        //01분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 118; i++)
        {
            Spawn(3, 1000, CretureType.Normal);
            if (i % 2 == 0)
            {
                Spawn(4, 1001, CretureType.Normal);
            }
            if (i % 20 == 0)
            {
                Spawn(5, 2000, CretureType.Noble);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
        //02분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 590; i++)
        {
            Spawn(3, 1000, CretureType.Normal);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        //03분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 885; i++)
        {
            if(i % 4 == 0)
            {
                Spawn(3, 1000, CretureType.Normal);
            }
            if(i % 75 == 0)
            {
                Spawn(5, 2000, CretureType.Noble);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.066f);
        }
        //04분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 132; i++)
        {
            Spawn(4, 1001, CretureType.Normal);
            yield return YieldInstructionCache.WaitForSeconds(0.33f);
        }
        //5분 44초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 132; i++)
        {
            Spawn(4, 1001, CretureType.Normal);
            if (i % 13 == 0)
            {
                Spawn(6, 2001, CretureType.Noble);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.33f);
        }
        //6분 29초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 174; i++)
        {
            if (i % 5 == 0)
            {
                Spawn(4, 1001, CretureType.Normal);
            }
            if (i % 10 == 0)
            {
                Spawn(6, 2001, CretureType.Noble);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.17f);
        }
        //6분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 1770; i++)
        {
            if (i % 13 == 0)
            {
                Spawn(4, 1001, CretureType.Normal);
            }
            if(i % 280 == 0)
            {
                Spawn(6, 2001, CretureType.Noble);
            }
            yield return null;
            yield return null;
        }
        //7분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 885; i++)
        {
            Spawn(4, 1001, CretureType.Normal);
            yield return YieldInstructionCache.WaitForSeconds(0.066f);
        }
        //8분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 1680; i++)
        {
            if(i % 7 == 0)
            {
                Spawn(4, 1001, CretureType.Normal);
            }
            if(i % 135 == 0)
            {
                Spawn(6, 2001, CretureType.Noble);
            }
            yield return null;
            yield return null;
        }
        //9분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 132; i++)
        {
            Spawn(5, 1002, CretureType.Normal);
            yield return YieldInstructionCache.WaitForSeconds(0.33f);
        }
        //10분 44초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 132; i++)
        {
            Spawn(5, 1002, CretureType.Normal);
            if (i % 13 == 0)
            {
                Spawn(6, 2002, CretureType.Noble);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.33f);
        }
        // 11분 29초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i <43.5; i++)
        {
            Spawn(5, 1002, CretureType.Normal);
            if (i % 2 == 0)
            {
                Spawn(6, 2002, CretureType.Noble);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.66f);
        }
        // 11분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 885; i++)
        {
            Spawn(5, 1002, CretureType.Normal);
            yield return YieldInstructionCache.WaitForSeconds(0.066f);
        }
        // 12분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 885; i++)
        {
            Spawn(5, 1002, CretureType.Normal);
            if (i % 30 == 0)
            {
                Spawn(6, 2002, CretureType.Noble);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.066f);
        }
        // 13분 59초
        yield return YieldInstructionCache.WaitForSeconds(1f);
        for (int i = 0; i < 295; i++)
        {
            Spawn(3, 1000, CretureType.Normal);
            Spawn(4, 1001, CretureType.Normal);
            if (i % 10 == 0)
            {
                Spawn(5, 2000, CretureType.Noble);
                Spawn(6, 2001, CretureType.Noble);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
        // 14분 59초
 
    }

    private IEnumerator SpawnBoss()
    {
        //yield return YieldInstructionCache.WaitForSeconds(300);
        Spawn(7, 3000, CretureType.Swarm_Boss);
        yield return YieldInstructionCache.WaitForSeconds(300);
        Spawn(8,4000, CretureType.Guvnor);
    }

    public void SpawnHPItem(Vector3 pos)
    {
        HPItem hp = pool.GetFromPool<HPItem>(0);
        hp.Init(GameManager.Inst.PlayerInfo.Max_HP * 0.3f, pos);
    }

    public void SpawnEXPItem(Vector3 pos,float dropEXP)
    {
        EXPItem exp = pool.GetFromPool<EXPItem>(1);
        exp.Init(dropEXP, pos);
    }

    public void SpawnFloatingDamage(Vector3 pos, int damage)
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
        if(other.tag == "Exp")
        {
            EXPItem item = other.transform.GetComponent<EXPItem>();
            GameManager.Inst.EXP = item.EXP;
            item.ReturnItem();
        }
        else if(other.tag == "Hp")
        {
            HPItem item = other.transform.GetComponent<HPItem>();
            item.ReturnItem();
        }
        else if(other.tag == "Creature")
        {
            Debug.Log(other.name);
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
}
