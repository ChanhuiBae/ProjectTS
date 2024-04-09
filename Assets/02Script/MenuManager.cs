using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private GameObject timeBackgroun;
    private TextMeshProUGUI countdown;
    private int time;
    private Image ultimateFill;
    private TextMeshProUGUI ultimateText;
    private TextMeshProUGUI killCount;
    private SkillButton skill1;
    private SkillButton skill2;
    private SkillButton skill3;

    private void Awake()
    {
        timeBackgroun = GameObject.Find("TimeBackground");
        if (!GameObject.Find("Time").TryGetComponent<TextMeshProUGUI>(out countdown))
        {
            Debug.Log("SpawnMananger - Awake - TextMeshProUGUI");
        }
        time = 1200;
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
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (time > 0)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
            time--;
            countdown.text = (time / 60).ToString() + ":" + (time % 60).ToString();
        }
        countdown.gameObject.SetActive(false);
        timeBackgroun.SetActive(false);
    }

    public void InitSkill(int num, int skill, string name)
    {
        switch(num)
        { 
            case 1:
                skill1.Init(1,skill, name);
                break;
            case 2:
                skill2.Init(2,skill, name);
                break;
            case 3:
                skill3.Init(3,skill, name);
                break;
        }
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
