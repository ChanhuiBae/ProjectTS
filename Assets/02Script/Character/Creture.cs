using HighlightPlus;
using Redcode.Pools;
using System.Collections;
using UnityEngine;


public class Creture : MonoBehaviour, IDamage, IPoolObject
{
    private Rigidbody rig;
    private CreatureAnimationController anim;
    private CretureAI ai;
    [SerializeField]
    private string poolName;
    private SpawnManager spawnManager;
    private SkillManager skillManager;
    private int ID;
    private float maxHP;
    private float currentHP;
    public float HP
    {
        get => currentHP/maxHP;
    }

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
    private int phase2HP;
    private int phase3HP;

    private int currentPhase;
    private bool IsDie;
    private Effect stun;
    private Effect hit;
    private Effect effect;

    private HighlightEffect inner;

    private int HPCount;

    private float maxGroggyHP;
    private float groggyHP;
    private float staggerMul;
    private float staggerCheck;

    private float damage;
    private Vector3 postion;
    private float count;

    private int endtime;
    public void Awake()
    {
        if (!TryGetComponent(out rig))
        {
            Debug.Log("Creture - Awake - Rigidbody");
        }
        if (!TryGetComponent<CreatureAnimationController>(out anim))
        {
            Debug.Log("Creature - Awake - AnimatorController");
        }
        if (!TryGetComponent<CretureAI>(out ai))
        {
            Debug.Log("Creture - Awake - AI");
        }
        if (!GameObject.Find("PoolManager").TryGetComponent<SpawnManager>(out spawnManager))
        {
            Debug.Log("Creture - Awake - SpawnManager");
        }
        if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("Creture - Awake - skillManager");
        }
        if (!transform.Find("Stun").gameObject.TryGetComponent<Effect>(out stun))
        {
            Debug.Log("Creature - Awake - Effect");
        }
        else
        {
            stun.gameObject.SetActive(false);
        }
        if (!transform.Find("Hit").gameObject.TryGetComponent<Effect>(out hit))
        {
            Debug.Log("Creature - Awake - Effect");
        }
        else
        {
            hit.gameObject.SetActive(false);
        }

        if(!TryGetComponent<HighlightEffect>(out inner))
        {
            Debug.Log("Creature - Awake - HighligtEffect");
        }
    }

    public void Init(Vector3 SpawnPos, int ID, CretureType type)
    {
        transform.position = new Vector3(SpawnPos.x, -10, SpawnPos.z);
        transform.rotation = Quaternion.Euler(0, -180, 0);
        postion = SpawnPos;
        count = -45;
        inner.innerGlow = 0;
        anim.Move(false);
        anim.SetPattern(0);
        this.ID = ID;
        this.type = type;
        if(type == CretureType.Guvnor)
        {
            HPCount = 9;
        }
        else
        {
            HPCount = 1;
        }
        TableEntity_Creature creature;
        GameManager.Inst.GetCreatureData(this.ID, out creature);
        maxHP = creature.Max_HP;
        currentHP = maxHP;
        maxGroggyHP = creature.Groggy_HP;
        groggyHP = maxGroggyHP;
        staggerMul = creature.Stagger_Mul;
        staggerCheck = 1 / staggerMul;
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
        phase2HP = creature.Phase_2_HP;
        phase3HP = creature.Phase_3_HP;
        ai.InitAI(this.type, creature.Move_Speed);
        currentPhase = 1;
        IsDie = false;
        stun.gameObject.SetActive(false);
        hit.gameObject.SetActive(false);
        if(type == CretureType.Guvnor)
        {
            ai.StopAI(1.5f);
        }
        endtime = 0;
    }
    public void Init(Vector3 SpawnPos, int ID, CretureType type, int time)
    {
        postion = SpawnPos;
        transform.position = SpawnPos;
        transform.rotation = Quaternion.Euler(0, -180, 0);
        count = -45;
        inner.innerGlow = 0;
        anim.Move(false);
        anim.SetPattern(0);
        this.ID = ID;
        this.type = type;
        if (type == CretureType.Guvnor)
        {
            HPCount = 9;
        }
        else
        {
            HPCount = 1;
        }
        TableEntity_Creature creature;
        GameManager.Inst.GetCreatureData(this.ID, out creature);
        maxHP = creature.Max_HP;
        currentHP = maxHP;
        maxGroggyHP = creature.Groggy_HP;
        groggyHP = maxGroggyHP;
        staggerMul = creature.Stagger_Mul;
        staggerCheck = 1 / staggerMul;
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
        phase2HP = creature.Phase_2_HP;
        phase3HP = creature.Phase_3_HP;
        ai.InitAI(this.type, creature.Move_Speed);
        currentPhase = 1;
        IsDie = false;
        stun.gameObject.SetActive(false);
        hit.gameObject.SetActive(false);
        if (type == CretureType.Guvnor)
        {
            ai.StopAI(1.5f);
        }
        endtime = time;
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return YieldInstructionCache.WaitForSeconds(endtime);
        ai.StopAI(10f);
        anim.Move(true);
        transform.LookAt(postion);
        transform.LeanMove(postion, 3f);
        yield return YieldInstructionCache.WaitForSeconds(3);
        spawnManager.ReturnCreature(poolName, this);
    }

    public int GetKey()
    {
        return ID;
    }

    public bool IsAttack()
    {
        if (ai.GetState() == AI_State.Attack)
        {
            return true;
        }
        return false;
    }

    public bool CalculateDamage(AttackType attack, ITakeDamage hiter)
    {
        if (!IsDie)
        {
            damage = hiter.TakeDamage(physicsCut, fireCut, waterCut, electricCut, iceCut, windCut);
            if (damage > 0)
            {
                currentHP -= damage;
                GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_Hit);
                if (type == CretureType.Guvnor)
                {
                    GameManager.Inst.menuManager.SetBossGage(currentHP / maxHP);
                }
                ChargeUltimate(damage);
                CheckPhase();
                DieCheck();
                if (type == CretureType.Guvnor)
                    CheckDropHP();
                if (!IsDie)
                {
                    SpawnHitEffect(damage);
                }
                if (GameManager.Inst.OnDamageText)
                {
                    spawnManager.SpawnFloatingDamage(transform.position, (int)damage);
                }
                return true;
            }
        }
        return false;
    }
    public bool CalculateDamage(AttackType attack, int key, ITakeDamage hiter)
    {
        if (!IsDie)
        {
            damage = hiter.TakeDamage(key, physicsCut, fireCut, waterCut, electricCut, iceCut, windCut);
            if (damage > 0)
            {
                currentHP -= damage;
                GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_Hit);
                if(type == CretureType.Guvnor)
                {
                    GameManager.Inst.menuManager.SetBossGage(currentHP / maxHP);
                }

                ChargeUltimate(damage);
                CheckPhase();
                DieCheck();
                if(type == CretureType.Guvnor)
                    CheckDropHP();
                if (!IsDie)
                {
                    SpawnHitEffect(damage);
                }
                if (GameManager.Inst.OnDamageText)
                {
                    spawnManager.SpawnFloatingDamage(transform.position, (int)damage);
                }
                return true;
            }
        }
        return false;
    }
    public bool CalulateDamage(int creatueKey, int patternKey, ITakeDamage hiter)
    {
        return false;
    }

    private void CheckPhase()
    {
        if (phase2HP != 0 && currentHP <= phase2HP && currentPhase == 1)
        {
            currentPhase = 2;
            ai.SetPhase(currentPhase);
        }
        if (phase3HP != 0 && currentPhase == 2 && currentHP <= phase3HP)
        {
            currentPhase = 3;
            ai.SetPhase(currentPhase);
        }
    }

    private void ChargeUltimate(float damage)
    {
        GameManager.Inst.AddUaltimate(damage);
    }

    private void SpawnHitEffect(float damage)
    {
        StartCoroutine(Hit());
    }

    private IEnumerator Hit()
    {
        inner.innerGlow = 0.7f;
        hit.gameObject.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        inner.innerGlow = 0f;
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        hit.gameObject.SetActive(false);
    }

    private void DieCheck()
    {
        if (currentHP < 0)
        {
            IsDie = true;
            ai.Die();

            GameManager.Inst.AddKillCount();

            effect = skillManager.SpawnEffect(18);
            effect.Init(EffectType.None, transform.position, 3f);
            DropHP();
            TableEntity_Creature exp;
            GameManager.Inst.GetCreatureData(int.Parse(poolName), out exp);
            spawnManager.SpawnEXPItem(transform.position,exp.Drop_Exp + GameManager.Inst.EXP);
            GameManager.Inst.EXP = 0;
            if(type == CretureType.Guvnor)
            {
                ai.Target.SetReward();
            }
            spawnManager.ReturnCreature(poolName, this);
        }
    }

    private void CheckDropHP()
    {
        if(currentHP*10/maxHP < HPCount)
        {
            spawnManager.SpawnHPItem(transform.position);
            HPCount--;
        }
    }

    private void DropHP()
    {
        float spawn = Random.Range(1,100);
        switch (type)
        {
            case CretureType.Normal:
                if(spawn <= 1)
                {
                    spawnManager.SpawnHPItem(transform.position);
                }
                break;
            case CretureType.Noble:
                if(spawn <= 2)
                {
                    spawnManager.SpawnHPItem(transform.position);
                }
                break;
            case CretureType.Swarm_Boss:
                spawnManager.SpawnHPItem(transform.position);
                break;
        }
        
    }

    private void FixedUpdate()
    {
        if (currentHP <= 0)
        {
            DieCheck();
        }
        if (ai.GetState() == AI_State.Idle)
        {
            spawnManager.ReturnCreature(poolName, this);
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

    private void CalulateGroggyDamage()
    {
        float groggyD = damage;
        if (GameManager.Inst.PlayerInfo.physics == Physics_Type.Strike)
        {
            groggyD += damage * 0.2f;
        }
        if (ai.GetState() == AI_State.Attack)
        {
            groggyD *= 3;
        }
        groggyHP -= groggyD;
        CheckGroggyHP();
    }

    private void CheckGroggyHP()
    {
        if(groggyHP <= 0)
        {
            StartGroggy();
            groggyHP = maxGroggyHP;
            staggerCheck = 1 / staggerMul;
        }
        else
        {
            float current = groggyHP / maxGroggyHP;
            current /= staggerMul;
            if(staggerCheck - current >= 1)
            {
                staggerCheck -= (int)current;
                StartStagger();
            }
        }
    }
    private void StartGroggy()
    {
        ai.StopAI(10f);
        StartCoroutine(Groggy());
    }

    private IEnumerator Groggy()
    {
        anim.SetGroggy(true);
        yield return new WaitForSeconds(10f);
        anim.SetGroggy(false);
    }

    private void StartStagger()
    {
        ai.StopAI(3f);
        StartCoroutine(StaggerTime(3f));
    }

    public void Stagger(float time)
    {
        if (type == CretureType.Swarm_Boss || type == CretureType.Guvnor)
        {
            CalulateGroggyDamage();
            return;
        }
        else
        {
            ai.StopAI(time);
            if (gameObject.activeSelf && !IsDie)
                StartCoroutine(StaggerTime(time));
        }
    }

    private IEnumerator StaggerTime(float time)
    {
        anim.SetStagger(true);
        yield return new WaitForSeconds(time);
        anim.SetStagger(false);
    }
    public void Stun(float time)
    {
        if (type == CretureType.Swarm_Boss || type == CretureType.Guvnor)
        {
            CalulateGroggyDamage();
            return;
        }
        else
        {
            ai.StopAI(time);
            if (gameObject.activeSelf && !IsDie)
                StartCoroutine(StunEffect(time));
        }
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
        if (type == CretureType.Swarm_Boss || type == CretureType.Guvnor)
        {
            CalulateGroggyDamage();
            return;
        }
        else
        {
            ai.StopAI(time);
            if (gameObject.activeSelf)
            {
                StartCoroutine(MoveAirborne(time));
            }
        }
    }

    private IEnumerator MoveAirborne(float time)
    {
        Vector3 pos = transform.position;
        LeanTween.move(gameObject, pos + (Vector3.up * time), time / 2).setEase(LeanTweenType.easeOutCubic);
        yield return YieldInstructionCache.WaitForSeconds(time / 2);
        LeanTween.move(gameObject, pos, time / 2).setEase(LeanTweenType.easeInSine);
    }

    public void Knockback(float distance)
    {
        if (type == CretureType.Swarm_Boss && type == CretureType.Guvnor)
        {
            CalulateGroggyDamage();
            return;
        }
        else
        {
            ai.StopAI(distance * 0.01f);
            LeanTween.move(gameObject, transform.position + Vector3.up - transform.forward * distance, distance * 0.01f).setEase(LeanTweenType.easeOutQuart);
        }
    }

    public void Airback(float time, float distance)
    {
        if (type == CretureType.Swarm_Boss || type == CretureType.Guvnor)
        {
            CalulateGroggyDamage();
            return;
        }
        else if (gameObject.activeSelf)
        {
            StartCoroutine(MoveAirback(time, distance));
        }
    }

    private IEnumerator MoveAirback(float time, float distance)
    {
        Vector3 pos = transform.position;
        LeanTween.move(gameObject, transform.position + Vector3.up - transform.forward * distance / 2 + (Vector3.up * 3f), time / 2).setEase(LeanTweenType.easeOutCubic);
        yield return YieldInstructionCache.WaitForSeconds(time / 2);
        LeanTween.move(gameObject, transform.position + Vector3.up - transform.forward * distance / 2, time / 2).setEase(LeanTweenType.easeInSine);
    }

    public void Pulled(Vector3 center)
    {
        if (type == CretureType.Swarm_Boss || type == CretureType.Guvnor)
        {
            CalulateGroggyDamage();
            return;
        }
        else
        {
            ai.StopAI(1f);
            LeanTween.move(gameObject, center, 0.5f).setEase(LeanTweenType.easeInElastic);
        }
    }

    private void Update()
    {
        if(type == CretureType.Guvnor)
        {
            if(count < 0)
            {   
                transform.position = new Vector3(postion.x, count, postion.z);
                count += 0.5f;
            }
            else
            {
                transform.position = postion;
            }
        }
        
    }
}
