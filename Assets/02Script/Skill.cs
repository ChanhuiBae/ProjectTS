using System;
using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private int ID;
    private string weapon_type;
    private int category;
    private int current_level;
    private int max_level;
    private int current_charge;
    private int max_charge;
    public int ChargeMax
    {
        get => max_charge;
    }
    private int current_hit;
    private int max_hit;
    private int currentInfoID;
    private TableEntity_Skill currentInfo;
    private TableEntity_Skill_Hit_Frame hitInfo;
    private bool IsActive;
    private int chargeCount;
    private SkillManager skillManager;

    private bool counting;

    private void Awake()
    {
        if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("Skill - Awake - SkillManager");
        }
    }

    public void init(int id, string weapon_type, int category, int max_level, int max_charge, int max_hit)
    {
        this.ID = id;
        this.weapon_type = weapon_type;
        this.category = category;
        this.max_level = max_level;
        current_level = 1;
        this.max_charge = max_charge;
        current_hit = 0;
        this.max_hit = max_hit;
        current_hit = 1;
        currentInfoID = GetKey();
        GameManager.Inst.GetSkillData(currentInfoID, out currentInfo);
        GameManager.Inst.GetSkillHitFrame(ID, out hitInfo);
        if(category == 3 && ID %10 == 1)
        {
            GameManager.Inst.SetMaxUltimate((int)currentInfo.Need_Damage);
        }
        counting = false;
    }

    public int GetKey()
    {
        string key;
            key = ID + weapon_type + category + current_level + current_charge + current_hit;
        return int.Parse(key);
    }

    public int GetLevel()
    {
        return current_level;
    }

    public void LevelUp()
    { 
        current_level++;
        currentInfoID = GetKey();
        GameManager.Inst.GetSkillData(currentInfoID, out currentInfo);
        if (category == 3 && ID % 10 == 1)
        {
            GameManager.Inst.SetMaxUltimate((int)currentInfo.Need_Damage);
        }
    }

    private void SetIdle()
    {
        IsActive = false;
        counting = false;
        current_charge = 0;
        current_hit = 0;
        if(category == 3 && ID % 10 == 1)
        {
            GameManager.Inst.ResetUltimate();
        }
    }

    private void ChargeUp()
    {
        current_charge++;
        skillManager.ChargeUp(current_charge);
    }

    public void HitUp() 
    {
        IsActive = false;
        current_hit++;
        currentInfoID = GetKey();
        GameManager.Inst.GetSkillData(currentInfoID, out currentInfo);
        if(current_charge > 0 && current_hit > 1)
        {
            skillManager.SetCrowdControl(CrowdControlType.Airback);
        }
        if (current_charge > 1 && current_hit > 2)
        {
            skillManager.SetCrowdControl(CrowdControlType.Airback);
        }
        if (currentInfo != null)
        {
            IsActive = true;
        }
        Debug.Log("hitup" + current_hit);
    }

    public void StartSkill(int skillNum)
    {
        SetIdle();
        if(max_charge != 0)
        {
            StartCoroutine(CountCharge());
        }
        else
        {
            StartCoroutine(CountHit());
        }
    }

    private IEnumerator CountCharge()
    {
        current_charge = 0;
        for (int i = 0; i < max_charge; i++)
        {
            for (chargeCount = 0; chargeCount < 60; chargeCount++)
            {
                yield return null;
            }
            ChargeUp();
        }
        StopCharge();
    }
    public void StopCharge()
    {
        if (!counting)
        {
            StopAllCoroutines();
            StartCoroutine(CountHit());
        }
    }

    private IEnumerator CountHit()
    {
        counting = true;
        current_hit = 0;
        for(int i = 0; i < hitInfo.Hit_01; i++)
        {
            yield return null;
        }
        HitUp();
        skillManager.StartAnimator();
        if (hitInfo.Hit_02 != 0)
        {
            for (int i = 0; i < hitInfo.Hit_02; i++)
            {
                yield return null;
            }
            HitUp();
        }
        if (hitInfo.Hit_03 != 0)
        {
            for (int i = 0; i < hitInfo.Hit_03; i++)
            {
                yield return null;
            }
            HitUp();
        }
        if (hitInfo.Hit_04 != 0)
        {
            for (int i = 0; i < hitInfo.Hit_04; i++)
            {
                yield return null;
            }
            HitUp();
        }
        if (hitInfo.Hit_05 != 0)
        {
            for (int i = 0; i < hitInfo.Hit_05; i++)
            {
                yield return null;
            }
            HitUp();
        }
    }


    public float GetDamageA()
    {
        if(IsActive)
            return currentInfo.Damage_A;
        return 0;
    }

    public float GetDamageB()
    {
        if (IsActive)
            return currentInfo.Damage_B;
        return 0;
    }

}
