using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private Image ultimateFill;
    private TextMeshProUGUI ultimateText;
    private TextMeshProUGUI killCount;
    private SkillButton skill1;
    private SkillButton skill2;
    private SkillButton skill3;

    private void Awake()
    {
        if (!GameObject.Find("UltimateFill").TryGetComponent<Image>(out ultimateFill))
        {
            Debug.Log("MenuManager - Awake - Image");
        }
        if (!GameObject.Find("UltimateValue").TryGetComponent<TextMeshProUGUI>(out ultimateText))
        {
            Debug.Log("MenuManager - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("KillCount").TryGetComponent<TextMeshProUGUI>(out killCount))
        {
            Debug.Log("MenuManager - Awake - TextMeshProUGUI");
        }
        if(!GameObject.Find("Skill1").TryGetComponent<SkillButton>(out skill1))
        {
            Debug.Log("MenuManager - Awake - SkillButton");
        }
        if (!GameObject.Find("Skill2").TryGetComponent<SkillButton>(out skill2))
        {
            Debug.Log("MenuManager - Awake - SkillButton");
        }
        if (!GameObject.Find("Skill3").TryGetComponent<SkillButton>(out skill3))
        {
            Debug.Log("MenuManager - Awake - SkillButton");
        }
        ultimateFill.fillAmount = 0;
        ultimateText.text = "0%";
    }

    public void InitSkills(State skill1, float cooltime1, State skill2, float cooltime2, State skill3, float cooltime3)
    {
        //this.skill1.Init(skill1, cooltime1);
        this.skill2.Init(skill2, cooltime2);
        //this.skill3.Init(skill3, cooltime3);
    }

    public void SetUaltimate(int value)
    {
        ultimateFill.fillAmount = (float)value / 100f;
        ultimateText.text = value.ToString() + "%";
    }

    public void SetKillCount(int value)
    {
        killCount.text = value.ToString();
    }
}
