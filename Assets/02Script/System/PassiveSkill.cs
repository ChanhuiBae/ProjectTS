using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public enum Passive
{
    ATK,
    CoolTime,
    MaxHP,
    AttackSpeed,
    AdditionalDamage,
    Heal,
}


public class PassiveSkill : MonoBehaviour
{
    private SkillManager skillManager;
    private MenuManager menuManager;
    private PlayerController playerController;

    private int key;
    private Passive type;
    private int level;
    private TableEntitiy_Passive_Skill info;

    public void Init(TableEntitiy_Passive_Skill passive)
    {
        key = passive.ID;
        level = 1;
        info = new TableEntitiy_Passive_Skill();
        info.Base_Figure = passive.Base_Figure;
        info.Increase_Value = passive.Increase_Value;
        switch(passive.Passive_Type)
        {
            case 1:
                type = Passive.ATK;
                break;
            case 2:
                type = Passive.CoolTime;
                break;
            case 3:
                type = Passive.MaxHP;
                break;
            case 4:
                type = Passive.AttackSpeed;
                break;
            case 5:
                type = Passive.AdditionalDamage;
                break;
            case 6:
                type = Passive.Heal;
                break;
        }
    }

    private int GetLevel()
    {
        return level;
    }

    public float GetFigure()
    {
        return info.Base_Figure + (info.Increase_Value * (level-1));
    }

    public void LevelUp(int id)
    {
        if (key == id && level < info.Max_Level)
        {
            level++;
            ApplySkill();
        }
    }

    public void ApplySkill()
    {
        switch(type)
        {
            case Passive.ATK:
                skillManager.ATK_Passive(info.Base_Figure + (level - 1) * info.Increase_Value);
                break;
            case Passive.CoolTime:
                menuManager.CoolTime_Passive(info.Base_Figure + (level - 1) * info.Increase_Value);
                break;
            case Passive.MaxHP:
                playerController.MaxHP_Passive(info.Base_Figure + (level - 1) * info.Increase_Value);
                break;
            case Passive.AttackSpeed:
                skillManager.Speed_Passive(info.Base_Figure + (level - 1) * info.Increase_Value);
                break;
            case Passive.AdditionalDamage:
                skillManager.AdditionalDamgae = info.Base_Figure + (level -1) * info.Increase_Value;
                break;
            case Passive.Heal:
                StopAllCoroutines();
                StartCoroutine(Heal());
                break;
        }
    }

    private IEnumerator Heal()
    {
        float add = playerController.MAXHP * (info.Base_Figure + (level - 1) * info.Increase_Value);
        while (true)
        {
            playerController.ApplyHP(add);
            yield return YieldInstructionCache.WaitForSeconds(10f);
        }
    }
}
