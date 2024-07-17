using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillDevil : MonoBehaviour
{
    private CreatureAnimationController anim;
    private CretureAI ai;
    private PlayerController player;
    private SkillManager skillManager;
    private SpawnManager spawnManager;
    private Effect effect;
    private int rushCount;

    private void Awake()
    {
        if(!TryGetComponent<CreatureAnimationController>(out anim))
        {
            Debug.Log("PillDevil - Awake - CreatureAnimationController");
        }
        if(!TryGetComponent<CretureAI>(out ai))
        {
            Debug.Log("PillDevil");
        }
        if(!GameObject.Find("Player").TryGetComponent<PlayerController>(out player))
        {
            Debug.Log("");
        }
        if(!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("");
        }
        if(!GameObject.Find("PoolManager").TryGetComponent<SpawnManager>(out spawnManager))
        {
            Debug.Log("");
        }

    }

    public void SuperStamp()
    {
        ai.StopAI(8);
        transform.LeanMove(new Vector3(0, 10, 0), 0.5f);
        StartCoroutine(SetDownPosition());
    }

    private IEnumerator SetDownPosition()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        effect = skillManager.SpawnEffect(25);
        effect.Init(EffectType.None, player.transform.position, 1f);
        transform.position = effect.transform.position + new Vector3(0,10,0);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        transform.LeanMove(new Vector3(0, -10, 0), 0.5f);
        anim.Struge();
    }

    public void Rush()
    {
        rushCount = 0;
        ai.StopAI(7.5f);
        StartCoroutine(RushTime());
    }
    private IEnumerator RushTime()
    {
        transform.position = spawnManager.RushInit();
        transform.LookAt(player.transform.position);
        effect = skillManager.SpawnEffect(26);
        effect.Init(EffectType.None, player.transform.position, 1f);
        effect.SetRotation(transform.rotation);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        transform.LeanMove(transform.position + transform.forward  * 20f, 0.5f);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        rushCount++;
        if(rushCount < 4)
        {
            StartCoroutine(RushTime());
        }
        else
        {
            SuperStamp();
        }
    }

}
