using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sorwd,
    Hamer,
    AR
}
public class Weapon : MonoBehaviour
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

}
