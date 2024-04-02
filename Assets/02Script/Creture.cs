using Redcode.Pools;
using System.Collections;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;


public class Creture : MonoBehaviour, IDamage, IPoolObject
{
    private Rigidbody rig;
    private CretureAI ai;
    [SerializeField]
    private string poolName;
    private PoolManager poolManager;
    private float maxHP;
    private float currentHP;
    private CretureType type;
    private float Creature_Physics_Cut;
    public float PhysicsCut
    {
        get => Creature_Physics_Cut;
    }
    private float Creature_Fire_Cut;
    public float FireCut
    {
        get => Creature_Fire_Cut;
    }
    private float Creature_Water_Cut;
    public float WaterCut
    {
        get => Creature_Water_Cut;
    }
    private float Creature_Electric_Cut;
    public float ElectricCut
    {
        get=> Creature_Electric_Cut;
    }
    private float Creature_Ice_Cut;
    public float IceCut
    {
        get => Creature_Ice_Cut;
    }
    private float Creature_Wind_Cut;
    public float WindCut
    {
        get => Creature_Electric_Cut;
    }

    public void Awake()
    {
        if (!TryGetComponent(out rig))
        {
            Debug.Log("Creture - Awake - Rigidbody");
        }
        if (!TryGetComponent<CretureAI>(out ai))
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
        maxHP = 4;
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
            GameManager.Inst.AddKillCount();
            GameObject obj = poolManager.GetFromPool<HPItem>(0).gameObject;
            HPItem hp = obj.GetComponent<HPItem>();
            hp.Init(1f, transform.position);
            obj = poolManager.GetFromPool<EXPItem>(1).gameObject;
            EXPItem exp = obj.GetComponent<EXPItem>();
            exp.Init(1f, transform.position);
            poolManager.TakeToPool<Creture>(poolName, this);
        }
    }

    public void Knockback()
    {
        ai.InitKnockback(true);
        rig.freezeRotation = true;
        rig.velocity = Vector3.up * 10f + transform.position;
    }


    public void OnCreatedInPool()
    {
        //throw new System.NotImplementedException();
    }

    public void OnGettingFromPool()
    {
        //throw new System.NotImplementedException();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground" && ai.State == AI_State.Knockback)
        {
            rig.freezeRotation = false;
            ai.InitKnockback(false);
        }
    }

    public void Pulled(Vector3 center)
    {
        // todo: move to center
    }
}
