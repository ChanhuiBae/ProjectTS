using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public enum AI_State
{
    Idle,
    Chase,
    Attack,
    Defense,
    Evasion,
    Stop,
    Die
}

public enum Phase
{
    Die,
    One,
    Two, 
    Three
}
public enum CretureType
{
    Normal,
    Noble,
    Swarm_Boss,
    Guvnor,
    Elite
}

public class CretureAI : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private CreatureAnimationController anim;
    private List<Pattern> patterns;
    private Dictionary<Pattern, bool> currentPatterns;
    private bool usePattern;
    public bool UsePattern
    {
        set => usePattern = value;
    }
    private AI_State currentState;
    public AI_State State
    {
        get => currentState;
    }
    private float speed;

    private Phase phase;
    private CretureType type;
    private Creture creature;
    private GameObject attackTarget;
    private PlayerController target;
    public PlayerController Target
    {
        get => target;
    }

    private Vector3 homePos;
    private Vector3 movePos;

    private float attackDistance;

    private bool isInit;


    // MosterBase 초기화 시 호출
    public void InitAI(CretureType type, float speed)
    {
        if (!TryGetComponent<NavMeshAgent>(out navAgent))
            Debug.Log("CreatureAI - Awake - NavMeshAgent");
        if (!TryGetComponent<CreatureAnimationController>(out anim))
            Debug.Log("CreatureAI - Awake - CreatureAnimationController");
        if (!TryGetComponent<Creture>(out creature))
            Debug.Log("CretureAI - Awake - Creture");
        Transform obj = transform.Find("Patterns");
        patterns = new List<Pattern>();
        foreach (Transform transform in obj)
        {
            patterns.Add(transform.GetComponent<Pattern>());
        }
        foreach (Pattern pattern in patterns)
        {
            pattern.Init(creature.GetKey());
        }
        this.type = type;
        isInit = true;
        attackTarget = null;
        homePos = transform.position;
        if(type == CretureType.Guvnor)
        {
            attackDistance = 500f;
        }
        else if(type == CretureType.Swarm_Boss)
        {
            attackDistance = 20f;
        }
        else
        {
            attackDistance = 3f;
        }
        attackTarget = GameObject.Find("Player");
        if (attackTarget == null || !attackTarget.TryGetComponent<PlayerController>(out target))
        {
            Debug.Log("CretureAI - Init - PlayerController");
        }
        else
        {
            SetTarget(attackTarget);
        }

        navAgent.enabled = true;
        this.speed = speed;
        navAgent.speed = speed;
        usePattern = false;
        SetPhase(1);
        Spawn();
    }
    protected void ChangeAIState(AI_State newState)
    {
        if (usePattern)
        {
            StartCoroutine(WaitState(newState));
        }
        else
        {
            StopAllCoroutines();
            currentState = newState;

            anim.Move(true);
            anim.SetPattern(0);
            StartCoroutine(currentState.ToString());
        }
    } 

    private IEnumerator WaitState(AI_State newState)
    {
        while(usePattern)
        {
            yield return null;
        }
        ChangeAIState(newState);
    }

    public AI_State GetState()
    {
        return currentState;
    }

    protected void SetMoveTarget(Vector3 targetPos)
    {
        navAgent.SetDestination(targetPos);
    }

    protected float GetDistanceToTarget()
    {
        if (attackTarget)
            return (attackTarget.transform.position - transform.position).sqrMagnitude;
        return -1;
    }


    public void SetTarget(GameObject newTarget)
    {
        if (currentState == AI_State.Idle)
        {
            attackTarget = newTarget;
            try
            {
                attackTarget.TryGetComponent<PlayerController>(out target);
            }
            catch (NullReferenceException e)
            {
                Debug.Log("A traget don't have CharacterBase.");
            }
            ChangeAIState(AI_State.Chase);
        }
    }


    public void Spawn()
    {
        ChangeAIState(AI_State.Chase); // first state
    }


    public void LookTarget()
    {
        if(attackTarget != null)
        {
            transform.LookAt(target.transform.position);   
        }
    }
    
    protected IEnumerator Chase()
    {
        anim.Move(true);
        navAgent.speed = speed;
        yield return null;
        while (attackTarget != null)
        {
            if (GetDistanceToTarget() <= (float)attackDistance)
            {
                navAgent.SetDestination(transform.position);
                ChangeAIState(AI_State.Attack);
            }
            else
            {
                SetMoveTarget(attackTarget.transform.position); // 이동 목표점을 갱신
            }
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
    }

    protected IEnumerator Attack()
    {
        anim.Move(false);
        navAgent.speed = 0;
        yield return null;
        while (true)
        {
            yield return null;
            if(type != CretureType.Guvnor && GetDistanceToTarget() > attackDistance)
            {
                ChangeAIState(AI_State.Chase);
            }
            if(phase == Phase.One)
            {
                if (!usePattern)
                {
                    int use = UnityEngine.Random.Range(0, currentPatterns.Count);
                    int i = 0;
                    foreach (KeyValuePair<Pattern, bool> items in currentPatterns)
                    {
                        if(i == use)
                        {
                            if (items.Value == true)
                            {
                                anim.SetPattern(items.Key.GetPatternKey());
                                items.Key.StartPattern();
                                SetPatternDisable(items.Key);
                                usePattern = true;
                                break;
                            }
                        }
                        i++;
                    }
                }
            }
            else if(phase == Phase.Two)
            {
                if (!usePattern)
                {
                    int use = UnityEngine.Random.Range(0, currentPatterns.Count);
                    int i = 0;
                    foreach (KeyValuePair<Pattern, bool> items in currentPatterns)
                    {
                        if (i == use)
                        {
                            if (items.Value == true)
                            {
                                anim.SetPattern(items.Key.GetPatternKey());
                                items.Key.StartPattern();
                                SetPatternDisable(items.Key);
                                usePattern = true;
                                break;
                            }
                        }
                        i++;
                    }
                }
            }
            else if(phase == Phase.Three) 
            {
                if(!usePattern)
                {
                    int use = UnityEngine.Random.Range(0, currentPatterns.Count);
                    int i = 0;
                    foreach (KeyValuePair<Pattern, bool> items in currentPatterns)
                    {
                        if (i == use)
                        {
                            if (items.Value == true)
                            {
                                anim.SetPattern(items.Key.GetPatternKey());
                                items.Key.StartPattern();
                                SetPatternDisable(items.Key);
                                usePattern = true;
                                break;
                            }
                        }
                        i++;
                    }
                }
            }
        }
    }

    protected IEnumerator Defense()
    {
        yield return null;
        ChangeAIState(AI_State.Chase);
    }

    protected IEnumerator Evasion()
    {
        yield return null;
        ChangeAIState(AI_State.Attack);
    }

    protected IEnumerable Idle()
    {
        anim.Move(false);
        yield return null;
        navAgent.speed = 0f;
    }

    public void Die()
    {
        StopAllCoroutines();
        currentState = AI_State.Die;
    }

    public void SetIdle()
    {
        currentState = AI_State.Idle;
        attackTarget = null;
        phase = Phase.Die;
    }
    public void StopAI(float time)
    {
        if (navAgent == null)
            return;
        navAgent.enabled = false;
        StopAllCoroutines();
        currentState = AI_State.Stop;
        if (gameObject.activeSelf)
        {
            StartCoroutine(Stop(time));
        }
    }

    private IEnumerator Stop(float time)
    {
        yield return YieldInstructionCache.WaitForSeconds(time);
        RestartAI();
    }

    public void RestartAI()
    {
        navAgent.enabled = true;
        ChangeAIState(AI_State.Chase);
    }

    public void SetPhase(int phase)
    {
        switch(phase)
        {
            case 1:
                this.phase = Phase.One;

                currentPatterns = new Dictionary<Pattern, bool>();
                for (int i = 0; i < patterns.Count; i++)
                { 
                    if (patterns[i].IsUsePhase(1))
                    {
                        currentPatterns.Add(patterns[i], true);
                    }
                }
                break;
            case 2:
                this.phase = Phase.Two;

                currentPatterns.Clear();
                for (int i = 0; i < patterns.Count; i++)
                {
                    if (patterns[i].IsUsePhase(2))
                    {
                        currentPatterns.Add(patterns[i], true);
                    }
                }
                usePattern = false;
                StopAllCoroutines();
                StartCoroutine(Attack());
                break;
            case 3:
                this.phase = Phase.Three;

                currentPatterns.Clear();
                for (int i = 0; i < patterns.Count; i++)
                {
                    if (patterns[i].IsUsePhase(3))
                    {
                        currentPatterns.Add(patterns[i], true);
                    }
                }
                usePattern = false;
                StopAllCoroutines();
                StartCoroutine(Attack());
                break;
        }
    }

    public void SetPatternEnable(Pattern pattern)
    {
        currentPatterns[pattern] = true;
    }

    public void SetPatternDisable(Pattern pattern)
    {
        currentPatterns[pattern] = false;
    }
}
