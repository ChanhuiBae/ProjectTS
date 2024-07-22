using System.Collections;
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
    private PlayerController player;

    private int key;
    private Passive type;
    private int level;
    private int Max_Level;
    private float Base_Figure;
    private float Increase_Value;


    private void Awake()
    {
        if(!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("");
        }
        if(!GameObject.Find("Player").TryGetComponent<PlayerController>(out player))
        {
            Debug.Log("");
        }
    }

    public void Init(int ID, int max_Level, float base_Figure, float increase_Value, int passiveType)
    {
        key = ID;
        level = 1;
        Max_Level = max_Level;
        Base_Figure = base_Figure;
        Increase_Value = increase_Value;
     
        switch (passiveType)
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
        ApplySkill();
    }

    private int GetLevel()
    {
        return level;
    }

    public float GetFigure()
    {
        return Base_Figure + (Increase_Value * (level-1));
    }

    public void LevelUp(int id)
    {
        if (key == id && level < Max_Level)
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
                skillManager.ATK_Passive(Base_Figure + (level - 1) * Increase_Value);
                break;
            case Passive.CoolTime:
                GameManager.Inst.menuManager.CoolTime_Passive(Base_Figure + (level - 1) * Increase_Value);
                break;
            case Passive.MaxHP:
                player.MaxHP_Passive(Base_Figure + (level - 1) * Increase_Value);
                break;
            case Passive.AttackSpeed:
                skillManager.Speed_Passive(Base_Figure + (level - 1) * Increase_Value);
                break;
            case Passive.AdditionalDamage:
                skillManager.AdditionalDamgae = Base_Figure + (level -1) * Increase_Value;
                break;
            case Passive.Heal:
                StopAllCoroutines();
                StartCoroutine(Heal());
                break;
        }
    }

    private IEnumerator Heal()
    {
        float add = player.MAXHP * (Base_Figure + (level - 1) * Increase_Value);
        while (true)
        {
            yield return YieldInstructionCache.WaitForSeconds(10f);
        }
    }
}
