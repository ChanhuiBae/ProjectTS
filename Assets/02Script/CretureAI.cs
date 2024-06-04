using System;
using System.Collections;
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
    private AI_State currentState;
    public AI_State State
    {
        get => currentState;
    }
    private CretureType type;
    private Creture creture;
    private GameObject attackTarget;
    private PlayerController target;

    private Vector3 homePos;
    private Vector3 movePos;

    private int currentPhase;
    private float attackDistance;

    private bool isInit;

    // MosterBase �ʱ�ȭ �� ȣ��
    public void InitAI(CretureType type, float speed)
    {
        if (!TryGetComponent<NavMeshAgent>(out navAgent))
            Debug.Log("CreatureAI - Init - NavMeshAgent");
        if (!TryGetComponent<CreatureAnimationController>(out anim))
            Debug.Log("CreatureAI - Init - CreatureAnimationController");
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
        currentPhase = 1;
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

    public void SetPhase(int phase)
    {
        currentPhase = phase;
    }

    protected void SetMoveTarget(Vector3 targetPos)
    {
        // debug message ���� �� �ֵ��� �̵� ������ �ϳ��� ������ ����
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
        anim.Move(true);
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
                SetMoveTarget(attackTarget.transform.position); // �̵� ��ǥ���� ����
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
            if (navAgent.remainingDistance < 1f) // ��ǥ���� ���� �Ÿ�
                ChangeAIState(AI_State.Roaming);
        }
    }

    protected IEnumerator Attack()
    {
        anim.SetPattern(1);
        yield return null;
        while (true)
        {
            yield return null;
            if (GetDistanceToTarget() > 6f)
            {
                anim.SetPattern(0);
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
        anim.Move(false);
        anim.SetPattern(0);
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
