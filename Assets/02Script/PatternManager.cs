using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour, ITakeDamage
{
    private Armor playerArmor;
    private Dictionary<int, Dictionary<int, TableEntity_Pattern>> creaturePattern;

    private void Awake()
    {
        if(!GameObject.Find("Player").TryGetComponent<Armor>(out playerArmor))
        {
            Debug.Log("PatternManager - Awake - Armor");
        }
    }

    public void Init(List<int> creatures)
    {

    }

    public float TakeDamage(float Physics_Cut, float Fire_Cut, float Water_Cut, float Electric_Cut, float Ice_Cut, float Wind_Cut)
    {
        return 0;
    }

    public float TakeDamage(int key, float Physics_Cut, float Fire_Cut, float Water_Cut, float Electric_Cut, float Ice_Cut, float Wind_Cut)
    {
        return 0;
    }

    public float TakeDamage(int creatureKey, int PatternInfoKey)
    {
        TableEntity_Creature creature;
        GameManager.Inst.GetCreatureData(creatureKey, out creature);
        TableEntity_Pattern_Info pattern;
        GameManager.Inst.GetPatternInfoData(PatternInfoKey, out pattern);
        float Physics = creature.Physics * (1 - playerArmor.Physics_Cut) * pattern.Physics_Mul;
        float Fire = creature.Fire * (1- playerArmor.Fire_Cut) * pattern.Fire_Mul;
        float Water = creature.Water * (1- playerArmor.Water_Cut) * pattern.Water_Mul;
        float Electric = creature.Electric * (1 - playerArmor.Electric_Cut) * pattern.Electric_Mul;
        float Ice = creature.Ice * (1 - playerArmor.Ice_Cut) * pattern.Ice_Mul;
        float Wind = creature.Wind_Cut * (1 - playerArmor.Wind_Cut) * pattern.Wind_Mul;
        float damage = Physics + Fire + Water + Electric + Ice + Wind;
        return damage;
    }


}


