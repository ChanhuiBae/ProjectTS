using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public bool CalculateDamage(AttackType attack, ITakeDamage hiter);
    public bool CalculateDamage(AttackType attack, int key, ITakeDamage hiter);
    public bool CalulateDamage(int creatueKey, int patternKey, ITakeDamage hiter);

    public void Stagger(float time);
    public void Stun(float time);
    public void Airborne(float time);
    public void Knockback(float distance);
    public void Airback(float time, float distance);
    public void Pulled(Vector3 center);

    public bool IsAttack();
}
