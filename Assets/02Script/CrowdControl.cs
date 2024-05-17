using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CrowdControlType
{
    None,
    Stagger,
    Stun,
    Knockback,
    Airborne,
    Airback,
    Pulled
}

public class CrowdControl : MonoBehaviour
{
    private SkillManager skillManager;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
            {
                Debug.Log("CrowdControl - Awake - SkillManager");
            }
        }
    }

    public void SetAirborne()
    {
        skillManager.SetCrowdControl(CrowdControlType.Airborne);
    }

    public void SetKnockback()
    {
        skillManager.SetCrowdControl(CrowdControlType.Knockback);
    }


    public void Push()
    {
        skillManager.SetCrowdControl(CrowdControlType.Pulled);
        skillManager.SetPulledPoint(transform.forward * 12f + transform.position);
        LeanTween.move(gameObject, transform.forward * 10f + transform.position, 1f);
    }

    public void CrowdControlNone()
    {
        skillManager.SetCrowdControl(CrowdControlType.None);
    }
}
