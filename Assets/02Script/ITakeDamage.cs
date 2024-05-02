using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
public interface ITakeDamage
{
    public float TakeDamage(float Physics_Cut, float Fire_Cut, float Water_Cut, float Electric_Cut, float Ice_Cut, float Wind_Cut);
    public float TakeDamageByKey(int key, float Physics_Cut, float Fire_Cut, float Water_Cut, float Electric_Cut, float Ice_Cut, float Wind_Cut);
}
