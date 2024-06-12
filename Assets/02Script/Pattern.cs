using System.Collections;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    private CretureAI ai;
    private PatternManager patternManager;
    private TableEntity_Pattern pattern;
    private TableEntity_Pattern_Hit_Frame hit;
    private int creatureKey;
    private int current_hit;
    private float currentTime;


    public void Init(int creatureKey)
    {
        if(!GameObject.Find("PatternManager").TryGetComponent<PatternManager>(out patternManager))
        {
            Debug.Log("Pattern - Init - PatternManager");
        }
        if (!transform.parent.parent.transform.TryGetComponent<CretureAI>(out ai))
        {
            Debug.Log("Pattern - Init - CreatrureAI");
        }
        GameManager.Inst.GetPatternData(int.Parse(transform.name), out pattern);
        GameManager.Inst.GetPatternHitFrameData(int.Parse(transform.name), out hit);
        StopAllCoroutines();
        this.creatureKey = creatureKey;
        current_hit = 0;
        currentTime = pattern.Cool_Time;

    }

    public bool IsUsePhase(int i)
    {
        switch (i)
        {
            case 1:
                return pattern.Trigger_Phase_01;
            case 2:
                return pattern.Trigger_Phase_02;
            case 3:
                return pattern.Trigger_Phase_03;
        }
        return false;
    }

    public int GetPatternKey()
    {
        return pattern.ID;
    }

    public int GetKey()
    {
        int key = pattern.ID * 10 + current_hit;
        return key;
    }

    private void HitUp()
    {
        current_hit++;
    }

    private IEnumerator CountHit()
    {
        current_hit = 0;
        for (int i = 0; i < hit.Hit_01; i++)
        {
            yield return null;
        }
        HitUp();
        if (hit.Hit_02 != 0)
        {
            for (int i = 0; i < hit.Hit_02; i++)
            {
                yield return null;
            }
            HitUp();
        }
        if (hit.Hit_03 != 0)
        {
            for (int i = 0; i < hit.Hit_03; i++)
            {
                yield return null;
            }
            HitUp();
        }
        if (hit.Hit_04 != 0)
        {
            for (int i = 0; i < hit.Hit_04; i++)
            {
                yield return null;
            }
            HitUp();
        }
        if (hit.Hit_05 != 0)
        {
            for (int i = 0; i < hit.Hit_05; i++)
            {
                yield return null;
            }
            HitUp();
        }
    }

    public void StartPattern()
    {
        StartCoroutine(CountHit());
    }


    private IEnumerator CoolTime()
    {
        for (currentTime = 0; currentTime < pattern.Cool_Time; currentTime++)
        {
            yield return null;
        }
        ai.SetPatternEnable(this);
    }

    public void StartCoolTime()
    {
        StartCoroutine(CoolTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && current_hit != 0)
        {
            patternManager.TakeDamageOther(creatureKey, GetKey(), other);
        }
    }
}
