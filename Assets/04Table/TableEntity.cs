[System.Serializable]
public class TableEntity_Player_Stats
{
    public int Level;
    public int Max_Weight;
    public float Max_Hp;
    public int Critical_Chance;
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
    public int Skill_ID;
    public string Skill_Category;
}
