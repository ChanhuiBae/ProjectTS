using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour, ITakeDamage
{
    private Armor playerArmor;

    public void InitArmor(Armor armor)
    {
        playerArmor = armor;
    }

    public float TakeDamage(float Physics_Cut, float Fire_Cut, float Water_Cut, float Electric_Cut, float Ice_Cut, float Wind_Cut)
    {
        return 0;
    }

    public float TakeDamage(int key, float Physics_Cut, float Fire_Cut, float Water_Cut, float Electric_Cut, float Ice_Cut, float Wind_Cut)
    {
        return 0;
    }

    public float TakeDamage()
    {
        return 0;
    }


}


