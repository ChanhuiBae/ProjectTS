using Redcode.Pools;
using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UIElements.UxmlAttributeDescription;


public class Creture : MonoBehaviour, IDamage, IPoolObject
{
    private Rigidbody rig;
    private CretureAI ai;
    [SerializeField]
    private string poolName;
    private PoolManager poolManager;
    private int ID;
    private float maxHP;
    private float currentHP;
    private CretureType type;
    private int physics;
    private int fire;
    private int water;
    private int electric;
    private int ice;
    private int wind;
    private float physicsCut;
    private float fireCut;
    private float waterCut;
    private float electricCut;
    private float iceCut;
    private float windCut;
    private float attackSpeed;

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

    public void Init(Vector3 SpawnPos, int ID, CretureType type)
    {
        transform.position = SpawnPos;
        this.ID = ID;
        this.type = type;
        TableEntity_Creature creature;
        GameManager.Inst.GetCreatureData(this.ID, out creature);
        maxHP = creature.Max_HP;
        currentHP = maxHP;
        physics = creature.Physics;
        fire = creature.Fire;
        water = creature.Water;
        electric = creature.Electric;
        ice = creature.Ice;
        wind = creature.Wind;
        physicsCut = creature.Physics_Cut;
        fireCut = creature.Fire_Cut;
        waterCut = creature.Water_Cut;
        electricCut = creature.Electric_Cut;
        iceCut = creature.Ice_Cut;
        windCut = creature.Wind_Cut;
        attackSpeed = creature.Attack_Speed;
        ai.InitAI(this.type, creature.Move_Speed);
    }

    public void CalculateDamage(ITakeDamage hiter)
    {
        currentHP -= hiter.TakeDamage(physicsCut, fireCut,waterCut,electricCut,iceCut,windCut);
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


    public void OnCreatedInPool()
    {
        //throw new System.NotImplementedException();
    }

    public void OnGettingFromPool()
    {
        //throw new System.NotImplementedException();
    }

    public void Pulled(Vector3 center)
    {
        // todo: move to center
    }

    public void Stun(int time)
    {
        ai.StopAI(time);
        if (gameObject.activeSelf)
            StartCoroutine(StunEffect(time));
    }

    private IEnumerator StunEffect(int time)
    {
        for (float i = 0.1f; i < time; i += 0.1f)
        {
            LeanTween.move(gameObject, transform.position - transform.forward * 0.1f, 0.1f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            LeanTween.move(gameObject, transform.position + transform.forward * 0.1f, 0.1f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
    }

    public void Airborne(int time)
    {
        ai.StopAI(time);
        if(gameObject.activeSelf)
        {
            StartCoroutine(MoveAirborne(time));
        }
    }

    private IEnumerator MoveAirborne(float time)
    {
        Vector3 pos = transform.position;
        LeanTween.move(gameObject, pos + (Vector3.up * 3f), time / 2).setEase(LeanTweenType.easeOutCubic);
        yield return YieldInstructionCache.WaitForSeconds(time/2);
        LeanTween.move(gameObject, pos, time / 2).setEase(LeanTweenType.easeInSine);
    }

    public void Knockback(int distance)
    {
        ai.StopAI(distance * 0.01f);
        LeanTween.move(gameObject, transform.position + Vector3.up - transform.forward * distance, distance * 0.01f).setEase(LeanTweenType.easeOutQuart);
    }
}
