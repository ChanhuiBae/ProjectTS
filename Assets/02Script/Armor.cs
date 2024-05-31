using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    private int id;
    private int weight;

    private float physics_Def;
    public float Physics_Def
    {
        get => physics_Def;
    }
    private float fire_Def;
    public float Fire_Def
    {
        get => fire_Def;
    }
    private float water_Def;
    public float Water_Def
    {
        get => water_Def;
    }
    private float electric_Def;
    public float Electric_Def
    {
        get => electric_Def;
    }
    private float ice_Def;
    public float Ice_Def
    {
        get => ice_Def;
    }
    private float wind_Def;
    public float Wind_Def
    {
        get => wind_Def;
    }

    private float physics_Cut;
    public float Physics
    {
        get => physics_Cut;
    }
    private float fire_Cut;
    public float Fire_Cut
    {
        get => fire_Cut;
    }
    private float water_Cut;
    public float Water_Cut
    {
        get => water_Cut;
    }
    private float electric_Cut;
    public float Electric_Cut
    {
        get => electric_Cut;
    }
    private float ice_Cut;
    public float Ice_Cut
    {
        get => ice_Cut;
    }
    private float wind_Cut;
    public float Wind_Cut
    {
        get => wind_Cut;
    }

    private void Awake()
    {
        id = GameManager.Inst.PlayerInfo.ArmorID;
        TableEntity_Armor armorData;
        GameManager.Inst.GetArmorData(id, out armorData);
        Init(armorData);
    }

    public void Init(TableEntity_Armor armor)
    {
        id = armor.ID;
        weight = armor.Weight;
        physics_Def = armor.Physics_Def;
        fire_Def = armor.Fire_Def;
        water_Def = armor.Water_Def;
        electric_Def = armor.Electric_Def;
        ice_Def = armor.Ice_Def;
        wind_Def = armor.Wind_Def;
        physics_Cut = armor.Physics_Cut;
        fire_Cut = armor.Fire_Cut;
        water_Cut = armor.Water_Cut;
        electric_Cut = armor.Electric_Cut;
        ice_Cut = armor.Ice_Cut;
        wind_Cut = armor.Wind_Cut;
    }
}
