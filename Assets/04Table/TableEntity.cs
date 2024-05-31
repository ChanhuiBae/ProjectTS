using JetBrains.Annotations;

[System.Serializable]
public class TableEntity_Player
{
    public int Level;
    public int Max_Weight;
    public float Max_Hp;
    public int Health;
    public int Endurance;
    public int Strength;
    public int Dexterity;
    public int Instinct;
    public int Adaptation;
    public int Available_Point;
    public int Move_Speed;
    public int Avoid_Distance;
    public int Avoid_Frame;
    public int Avoid_Invincible_Frame;
    public float Exp_Need;
}

[System.Serializable]
public class TableEntity_Skill
{
    public int ID;
    public string Weapon_ID;
    public int Category_ID;
    public int Skill_Level_Max;
    public int Charge_Max;
    public int Hit_Max;
    public string Skill_Name;
    public string Skill_Name_Eng;
    public bool Is_Scoping;
    public float Scoping_Time;
    public bool Is_Charging;
    public int Stack_Max;
    public int Linked_Skill;
}

[System.Serializable]
public class TableEntity_Weapon
{
    public int ID;
    public int Type;
    public int Weight;
    public string Physics_Type;
    public bool Is_Slash;
    public bool Is_Strike;
    public bool Is_Explosion;
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
    public int Fire_Def;
    public int Water_Def;
    public int Electric_Def;
    public int Ice_Def;
    public int Wind_Def;
    public float Physics_Cut;
    public float Fire_Cut;
    public float Water_Cut;
    public float Electric_Cut;
    public float Ice_Cut;
    public float Wind_Cut;
}

[System.Serializable] 
public class TableEntity_Skill_Info
{
    public int ID;
    public string Type;
    public int Skill_Level;
    public int Charge_Level;
    public float Damage_A;
    public float Damage_B;
    public int Stagger_Time;
    public float Stun_Time;
    public float Airborne_Time;
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
    public int ID;
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
    public string Name;
    public int Category_ID;
    public string Category;
    public float Max_HP;
    public float Move_Speed;
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
    public int Pattern_01;
}


[System.Serializable]
public class TableEntity_Pattern
{
    public int ID;
    public int Category_ID;
    public int Hit_Max;
    public float Delay_Min;
    public float Delay_Max;
    public float Cool_Time;
    public bool Trigger_Phase_01;
    public bool Trigger_Phase_02;
    public bool Trigger_Phase_03;
    public string Name;
}

[System.Serializable]
public class TableEntity_Pattern_Info
{
    public int ID;
    public string Damage_Type;
    public float Physics_A;
    public float Fire_A;
    public float Water_A;
    public float Electric_A;
    public float Ice_A;
    public float Wind_A;
    public float Stagger_Time;
    public float Stun_Time;
    public float Stun_Chance;
    public float Airborne_Time;
    public float Knockback_Distance;
}

[System.Serializable]
public class TableEntity_Pattern_Hit_Frame
{
    public int ID;
    public int Hit_01;
    public int Hit_02;
    public int Hit_03;
    public int Hit_04;
    public int Hit_05;
}