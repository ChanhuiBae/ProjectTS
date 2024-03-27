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
            switch (type)
            {
                case CretureType.Normal:
                    GameManager.Inst.ChargeUaltimate(1);
                    break;
                case CretureType.Noble:
                    GameManager.Inst.ChargeUaltimate(2);
                    break;
                case CretureType.Swarm_Boss:
                    GameManager.Inst.ChargeUaltimate(50);
                    break;
                case CretureType.Guvnor:
                    GameManager.Inst.ChargeUaltimate(100);
                    break;
                case CretureType.Elite:
                    GameManager.Inst.ChargeUaltimate(30);
                    break;
            }
            GameObject obj = poolManager.GetFromPool<HPItem>(1).gameObject;
            HPItem hp = obj.GetComponent<HPItem>();
            hp.Init(1f, transform.position);
            obj = poolManager.GetFromPool<EXPItem>(2).gameObject;
            EXPItem exp = obj.GetComponent<EXPItem>();
            exp.Init(1f, transform.position);
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
