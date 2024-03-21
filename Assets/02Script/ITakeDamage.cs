using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    public float TakeDamage();

    public float TakeDamage(float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut);

}
