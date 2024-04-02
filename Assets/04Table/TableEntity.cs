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
    public int Weapon_ID;
    public string Name;
    public int ID;
    public string Category;
}

[System.Serializable]
public class TableEntity_Weapon
{
    public int Physics;
    public int Fire;
    public int Water;
    public int Electric;
    public int Ice;
    public int Wind;
    public float Critical_Chance;
    public float Critical_Mag;
    public float Attack_Speed;
    public float Stagger_Time;
}

[System.Serializable]
public class TableEntity_Armor
{
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
    public int Charge_Level;
    public float Damage_A;
    public float Damage_B;
    public float Stun_Time;
    public float Airbone_Time;
    public int Knockback_Distance;
    public int Level;
    public float Cool_Time;
    public float Invincible_Time;
    public int Buff_A;
    public int Buff_B;
    public int Buff_C;
}

[System.Serializable]
public class TableEntity_Creature
{
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
    public float Move_Speed;
    public float Attack_Speed;
    public float Groggy_HP;
}
