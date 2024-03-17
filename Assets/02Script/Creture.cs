using Redcode.Pools;
using UnityEngine;

public class Creture : MonoBehaviour, IDamage, IPoolObject
{
    private CretureAI ai;
    [SerializeField]
    private string poolName;
    private PoolManager poolManager;

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

    public void Init(Vector3 SpawnPos)
    {
        transform.position = SpawnPos;
        ai.InitAI(CretureType.normal);
    }

    public void GetDamage(ITakeDamage hiter)
    {
        hiter.TakeDamage();
        poolManager.TakeToPool<Creture>(poolName, this);
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
