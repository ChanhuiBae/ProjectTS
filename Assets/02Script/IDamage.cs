using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public void CalculateDamage(ITakeDamage hiter);

    public void Stagger(float time);
    public void Stun(float time);
    public void Airborne(float time);
    public void Knockback(float distance);
    public void Pulled(Vector3 center);
}
