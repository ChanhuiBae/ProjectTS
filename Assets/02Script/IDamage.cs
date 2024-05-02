using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public void CalculateDamage(AttackType attack, ITakeDamage hiter);

    public void CalculateDamageProjectile(AttackType attack, Projectile projrectile, ITakeDamage hiter);
    public void Stagger(float time);
    public void Stun(float time);
    public void Airborne(float time);
    public void Knockback(float distance);
    public void Airback(float time, float distance);
    public void Pulled(Vector3 center);
}
