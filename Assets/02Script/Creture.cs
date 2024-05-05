using Redcode.Pools;
using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class Creture : MonoBehaviour, IDamage, IPoolObject
{
    private Rigidbody rig;
    private CretureAI ai;
    [SerializeField]
    private string poolName;
    private SpawnManager spawnManager;
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
    private bool IsDie;
    private Effect stun;
    private Effect effect;

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
        if (!GameObject.Find("PoolManager").TryGetComponent<SpawnManager>(out spawnManager))
        {
            Debug.Log("Creture - Awake - SpawnManager");
        }
        if(!transform.Find("Stun").gameObject.TryGetComponent<Effect>(out stun))
        {
            Debug.Log("Creature - Awake - Effect");
        }
        else
        {
            stun.gameObject.SetActive(false);
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
        IsDie = false;
        stun.gameObject.SetActive(false);
    }

    public void CalculateDamage(AttackType attack, ITakeDamage hiter)
    {
        if(!IsDie)
        {
            float damage = hiter.TakeDamage(physicsCut, fireCut, waterCut, electricCut, iceCut, windCut);
            currentHP -= damage;
            Debug.Log("Damage: " + damage);
            switch (type)
            {
                case CretureType.Normal:
                    GameManager.Inst.AddUaltimate(damage * 0.01f);
                    break;
                case CretureType.Noble:
                    GameManager.Inst.AddUaltimate(damage * 0.02f);
                    break;
                case CretureType.Swarm_Boss:
                    GameManager.Inst.AddUaltimate(damage * 0.5f);
                    break;
                case CretureType.Guvnor:
                    GameManager.Inst.AddUaltimate(damage);
                    break;
                case CretureType.Elite:
                    GameManager.Inst.AddUaltimate(damage * 0.3f);
                    break;
            }
            if (currentHP < 0)
            {
                IsDie = true;
                ai.Die();

                GameManager.Inst.AddKillCount();
                spawnManager.SpawnHPItem(transform.position);
                spawnManager.SpawnEXPItem(transform.position);
                spawnManager.ReturnCreature(poolName, this);
            }
            if (damage < 200 && damage > 0)
            {
                effect = spawnManager.SpawnEffect(0);
                effect.Init(EffectType.None, transform.position + Vector3.up, 1);
            }
            else if (damage > 200)
            {
                effect = spawnManager.SpawnEffect(1);
                effect.Init(EffectType.None, transform.position + Vector3.up, 1);
            }
        }
    }
    public void CalculateDamageByKey(AttackType attack, int key, ITakeDamage hiter)
    {
        if (!IsDie)
        {
            float damage = hiter.TakeDamageByKey(key, physicsCut, fireCut, waterCut, electricCut, iceCut, windCut);
            currentHP -= damage;
            Debug.Log("Damage: " + damage);
            
            switch (type)
            {
                case CretureType.Normal:
                    GameManager.Inst.AddUaltimate(damage * 0.01f);
                    break;
                case CretureType.Noble:
                    GameManager.Inst.AddUaltimate(damage * 0.02f);
                    break;
                case CretureType.Swarm_Boss:
                    GameManager.Inst.AddUaltimate(damage * 0.5f);
                    break;
                case CretureType.Guvnor:
                    GameManager.Inst.AddUaltimate(damage);
                    break;
                case CretureType.Elite:
                    GameManager.Inst.AddUaltimate(damage * 0.3f);
                    break;
            }
            if (currentHP < 0)
            {
                IsDie = true;
                ai.Die();
                GameManager.Inst.AddKillCount();
                spawnManager.SpawnHPItem(transform.position);
                spawnManager.SpawnEXPItem(transform.position);
                spawnManager.ReturnCreature(poolName, this);
            }
            if (damage < 200 && damage > 0)
            {
                effect = spawnManager.SpawnEffect(0);
                effect.Init(EffectType.None, transform.position + Vector3.up, 1);
            }
            else if (damage > 200)
            {
                effect = spawnManager.SpawnEffect(1);
                effect.Init(EffectType.None, transform.position + Vector3.up, 1);
            }
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

    public void Stagger(float time)
    {
        ai.StopAI(time);
        if (gameObject.activeSelf && !IsDie)
            StartCoroutine(StargerTime(time));
    }

    private IEnumerator StargerTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
    public void Stun(float time)
    {
        ai.StopAI(time);
        if (gameObject.activeSelf && !IsDie)
            StartCoroutine(StunEffect(time));
    }

    private IEnumerator StunEffect(float time)
    {
        stun.gameObject.SetActive(true);
        for (float i = 0.1f; i < time; i += 0.1f)
        {
            LeanTween.move(gameObject, transform.position - transform.forward * 0.1f, 0.1f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            LeanTween.move(gameObject, transform.position + transform.forward * 0.1f, 0.1f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        stun.gameObject.SetActive(false);
    }

    public void Airborne(float time)
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

    public void Knockback(float distance)
    {
        ai.StopAI(distance * 0.01f);
        LeanTween.move(gameObject, transform.position + Vector3.up - transform.forward * distance, distance * 0.01f).setEase(LeanTweenType.easeOutQuart);
    }

    public void Airback(float time, float distance)
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(MoveAirback(time, distance));
        }
    }

    private IEnumerator MoveAirback(float time, float distance)
    {
        Vector3 pos = transform.position;
        LeanTween.move(gameObject, transform.position + Vector3.up - transform.forward * distance/2 + (Vector3.up * 3f), time / 2).setEase(LeanTweenType.easeOutCubic);
        yield return YieldInstructionCache.WaitForSeconds(time / 2);
        LeanTween.move(gameObject, transform.position + Vector3.up - transform.forward * distance / 2, time / 2).setEase(LeanTweenType.easeInSine);
    }

    public void Pulled(Vector3 center)
    {
        ai.StopAI(1f);
        LeanTween.move(gameObject, center, 0.1f).setEase(LeanTweenType.easeInElastic);
    }
}
