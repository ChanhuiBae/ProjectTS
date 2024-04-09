using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

public enum AI_State
{
    Idle,
    Roaming,
    Chase,
    Attack,
    Defense,
    Evasion,
    Stop,
    Die
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

    protected NavMeshAgent navAgent;
    protected AI_State currentState;
    public AI_State State
    {
        get => currentState;
    }
    protected CretureType type;
    protected Creture creture;
    protected GameObject attackTarget;
    protected PlayerController target;

    protected Vector3 homePos;
    protected Vector3 movePos;

    protected bool haveOneHand;
    protected bool haveShield;
    protected bool haveRanged;
    protected float attackDistance;

    protected bool isInit;

    // MosterBase 초기화 시 호출
    public void InitAI(CretureType type, float speed)
    {
        if (!TryGetComponent<NavMeshAgent>(out navAgent))
            Debug.Log("CretureAI - Init - NavMeshAgent");
        if (!TryGetComponent<Creture>(out creture))
            Debug.Log("CretureAI - Init - Creture");
        this.type = type;
        isInit = true;
        attackTarget = null;
        homePos = transform.position;
        attackDistance = 10f;
        attackTarget = GameObject.Find("Player");
        if (attackTarget == null || !attackTarget.TryGetComponent<PlayerController>(out target))
        {
            Debug.Log("CretureAI - Init - PlayerController");
        }
        navAgent.speed = speed;
        navAgent.enabled = true;
        Spawn();
    }
    protected void ChangeAIState(AI_State newState)
    {
        if (isInit)
        {
            StopCoroutine(currentState.ToString());
            currentState = newState;
            StartCoroutine(currentState.ToString());
        }
    }

    protected void SetMoveTarget(Vector3 targetPos)
    {
        // debug message 남길 수 있도록 이동 로직을 하나의 평선으로 통일
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
        if (currentState == AI_State.Idle || currentState == AI_State.Roaming)
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
        ChangeAIState(AI_State.Roaming); // first state
    }

    protected IEnumerator Roaming()
    {
        navAgent.speed = 5f;
        yield return null;
        while (true)
        {
            movePos.x = UnityEngine.Random.Range(-10f, 10f);
            movePos.y = transform.position.y;
            movePos.z = UnityEngine.Random.Range(-10f, 10f);
            Vector3 target = homePos + movePos;
            SetMoveTarget(target);
            //IBase.Walk();
            while ((target - transform.position).sqrMagnitude > 0.3)
            {
                yield return YieldInstructionCache.WaitForSeconds(1f);
            }
            //IBase.StopMove();
            yield return YieldInstructionCache.WaitForSeconds(UnityEngine.Random.Range(7f, 10f));
        }
    }

    protected IEnumerator Chase()
    {
        navAgent.speed = 5f;
        //IBase.Run();
        yield return null;
        while (attackTarget != null)
        {
            if (GetDistanceToTarget() <= (float)attackDistance)
            {
                navAgent.SetDestination(transform.position);
                //IBase.StopMove();
                ChangeAIState(AI_State.Attack);
            }
            else
            {
                SetMoveTarget(attackTarget.transform.position); // 이동 목표점을 갱신
            }
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
    }

    protected IEnumerator ReturnHome()
    {
        yield return null;
        SetMoveTarget(homePos);
        //IBase.Run();

        while (true)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
            if (navAgent.remainingDistance < 1f) // 목표까지 남은 거리
                ChangeAIState(AI_State.Roaming);
        }
    }

    protected IEnumerator Attack()
    {
        yield return null;
        while (true)
        {
            yield return null;
            if (GetDistanceToTarget() > 10f)
            {
                ChangeAIState(AI_State.Chase);
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
        StartCoroutine(AI_State.Idle.ToString());
    }
    public void StopAI(float time)
    {
        StopAllCoroutines();
        currentState = AI_State.Stop;
        navAgent.enabled = false;
        if(gameObject.activeSelf)
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
}
