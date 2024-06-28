using System.Collections;
using System.Collections.Generic;
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
    private int key;
    private Passive type;
    private int level;
    private TableEntitiy_Passive_Skill info;

    public void Init()
    {

    }

    private void GetData()
    {

    }

    public void ApplySkill()
    {

    }
}
