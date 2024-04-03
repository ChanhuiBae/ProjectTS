using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public void CalculateDamage(ITakeDamage hiter);
    public void Knockback();
    public void Pulled(Vector3 center);
}
