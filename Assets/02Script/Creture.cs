using Redcode.Pools;
using System.Collections;
using UnityEngine;


public class Creture : MonoBehaviour, IDamage, IPoolObject
{
    private CretureAI ai;
    [SerializeField]
    private string poolName;
    private PoolManager poolManager;
    private float maxHP;
    private float currentHP;
    private CretureType type;

    public void Awake()
    {
        if(!TryGetComponent<CretureAI>(out ai))
        {
            Debug.Log("Creture - Awake - AI");
        }
        if (!GameObject.Find("PoolManager").TryGetComponent<PoolManager>(out poolManager))
        {
            Debug.Log("Creture - Awake - PoolManager");
        }
    }

    public void Init(Vector3 SpawnPos, CretureType type)
    {
        maxHP = 2;
        currentHP = maxHP;
        this.type = type;
        transform.position = SpawnPos;
        ai.InitAI(this.type);
    }

    public void GetDamage(ITakeDamage hiter)
    {
        currentHP -= hiter.TakeDamage();
        if (currentHP < 0)
        {
            ai.Die();
            poolManager.TakeToPool<Creture>(poolName, this);
        }
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
