using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    private TableEntity_Pattern pattern;
    private TableEntity_Pattern_Hit_Frame hit;
    private int current_hit;
    private int currentTime;
    
    public void Init()
    {
        current_hit = 0;
        currentTime = 0;
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
        // todo: ai에게 공격 시키기
    }

    public void StartCoolTime()
    {
        StartCoroutine(CoolTime());
    }
}
