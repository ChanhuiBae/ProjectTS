using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public void CalculateDamage(ITakeDamage hiter);

    public void Stun(int time);
    public void Airborne(int time);
    public void Knockback(int distance);
    public void Pulled(Vector3 center);
}
