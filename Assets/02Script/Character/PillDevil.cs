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

    private Transform rock;


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
        if(!GameObject.Find("RockPoint").TryGetComponent<Transform>(out rock))
        {
            Debug.Log("");
        }
    }

    public void SuperStamp()
    {
        ai.StopAI(8);
        StartCoroutine(SetDownPosition());
    }

    private IEnumerator SetDownPosition()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        effect = skillManager.SpawnEffect(25);
        effect.Init(EffectType.None, player.transform.position + new Vector3(0, 0.01f, 0),1f);
        transform.position = effect.transform.position + new Vector3(0,20,0);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        transform.LeanMove(transform.position + new Vector3(0, -20, 0), 0.5f);
        anim.Struge();
    }

    public void Dig()
    {
        transform.LeanMove(transform.position + new Vector3(0, -20, 0), 0.5f);
    }

    public void Rush()
    {
        ai.StopAI(7.5f);
        StartCoroutine(RushTime());
    }
    private IEnumerator RushTime()
    {
        Effect effect = skillManager.SpawnEffect(30);
        effect.InitFollow(EffectType.None, rock.gameObject, 6f);

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.position = spawnManager.RushInit();
        transform.rotation = Quaternion.identity;
        transform.LookAt(player.transform.position);
        yield return null;
        transform.position -= new Vector3(0, 5f, 0);
        effect = skillManager.SpawnEffect(26);
        effect.Init(EffectType.None, new Vector3(player.transform.position.x, 0.01f, player.transform.position.z), 1f);
        effect.SetRotation(transform.rotation);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        transform.LeanMove(transform.position + (transform.forward * 60f), 0.5f);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);


        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.position = spawnManager.RushInit();
        transform.rotation = Quaternion.identity;
        transform.LookAt(player.transform.position);
        transform.position -= new Vector3(0, 5f, 0);
        effect = skillManager.SpawnEffect(26);
        effect.Init(EffectType.None,new Vector3(player.transform.position.x, 0.01f, player.transform.position.z), 1f);
        effect.SetRotation(transform.rotation);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        transform.LeanMove(transform.position + (transform.forward * 60f), 0.5f);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);


        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.position = spawnManager.RushInit();
        transform.rotation = Quaternion.identity;
        transform.LookAt(player.transform.position);
        transform.position -= new Vector3(0, 5f, 0);
        effect = skillManager.SpawnEffect(26);
        effect.Init(EffectType.None, new Vector3(player.transform.position.x, 0.01f, player.transform.position.z), 1f);
        effect.SetRotation(transform.rotation);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        transform.LeanMove(transform.position + (transform.forward * 60f), 0.5f);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.position = spawnManager.RushInit();
        transform.rotation = Quaternion.identity;
        transform.LookAt(player.transform.position);
        transform.position -= new Vector3(0, 5f, 0);
        effect = skillManager.SpawnEffect(26);
        effect.Init(EffectType.None, new Vector3(player.transform.position.x, 0.01f, player.transform.position.z), 1f);
        effect.SetRotation(transform.rotation);
        yield return YieldInstructionCache.WaitForSeconds(1f);
        transform.LeanMove(transform.position + (transform.forward * 60f), 0.5f);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        SuperStamp();
    }

}
