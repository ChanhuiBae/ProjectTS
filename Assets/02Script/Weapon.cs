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

    private void Awake()
    {
        type = WeaponType.Sorwd;
    }

    public WeaponType GetType()
    {
        return type;
    }

    public float TakeDamage()
    {
        return 5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root != this.transform.root)
        {
            
        }
    }
}
