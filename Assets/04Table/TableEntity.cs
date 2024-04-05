using JetBrains.Annotations;

[System.Serializable]
public class TableEntity_Player_Stats
{
    public int Level;
    public int Max_Weight;
    public float Max_Hp;
    public int Health;
    public int Endurance;
    public int Strength;
    public int Dexterity;
    public int Adaptation;
    public int Available_Point;
    public int Move_Speed;
    public int Avoid_Distance;
    public int Avoid_Frame;
    public int Avoid_Invincible_Frame;
    public float Exp_Need;
}

[System.Serializable]
public class TableEntity_Skill_List
{
    public int ID;
    public string Weapon_ID;
    public int Category_ID;
    public int Skill_Level_Max;
    public int Charge_Max;
    public int Hit_Max;
    public string Skill_Name;
}

[System.Serializable]
public class TableEntity_Weapon
{
    public int ID;
    public int Type;
    public int Physics;
    public int Fire;
    public int Water;
    public int Electric;
    public int Ice;
    public int Wind;
    public float Critical_Chance;
    public float Critical_Mag;
    public float Attack_Speed;
}

[System.Serializable]
public class TableEntity_Armor
{
    public int ID;
    public int Weight;
    public int Physics_Def;
    public int Physics_Cut;
    public int Fire_Def;
    public int Water_Def;
    public int Electric_Def;
    public int Ice_Def;
    public int Wind_Def;
}

[System.Serializable] 
public class TableEntity_Skill
{
    public int ID;
    public string Type;
    public int Skill_Level;
    public int Charge_Level;
    public float Damage_A;
    public float Damage_B;
    public int Stagger_Time;
    public float Stun_Time;
    public float Airbone_Time;
    public int Knockback_Distance;
    public float Cool_Time;
    public float Need_Damage;
    public float Invincible_Time;
    public int Buff_A;
    public int Buff_B;
    public int Buff_C;
}

[System.Serializable]
public class TableEntity_Skill_Hit_Frame
{
    public int Skill_ID;
    public int Hit_01;
    public int Hit_02;
    public int Hit_03;
    public int Hit_04;
    public int Hit_05;
}

[System.Serializable]
public class TableEntity_Creature
{
    public int ID;
    public int Physics;
    public int Fire;
    public int Water;
    public int Electric;
    public int Ice;
    public int Wind;
    public float Physics_Cut;
    public float Fire_Cut;
    public float Water_Cut;
    public float Electric_Cut;
    public float Ice_Cut;
    public float Wind_Cut;
    public float HP;
    public float Move_Speed;
    public float Attack_Speed;
    public float Groggy_HP;
}
