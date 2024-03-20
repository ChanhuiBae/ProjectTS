using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sorwd,
    Hamer,
    AR
}
public class Weapon : MonoBehaviour, ITakeDamage
{
    private WeaponType type;
    private float damage;

    private void Awake()
    {
        type = WeaponType.Sorwd;
        damage = 0;
    }

    public WeaponType GetType()
    {
        return type;
    }

    public void ResetDamage()
    {
        damage = 0;
    }

    public void NormalAttack()
    {
        damage = 5;
    }

    public float TakeDamage()
    {
        return damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root != this.transform.root)
        {
            if(other.TryGetComponent<IDamage>(out IDamage creture))
            {
                creture.GetDamage(this);
            }
        }
    }
}
