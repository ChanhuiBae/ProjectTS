using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Skill,
    Hit
}

public class Effect : MonoBehaviour
{
    private EffectType type;

    public void Init(EffectType type)
    {
        this.type = type;
        gameObject.SetActive(false);
    }

    public void OnEffect()
    {
        gameObject.SetActive(true);
    }

    public void OffEffect()
    {
        gameObject.SetActive(false);
    }
}
