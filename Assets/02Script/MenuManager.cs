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
    private TextMeshProUGUI killCount;
    private SkillButton basic;
    private SkillButton skill1;
    private SkillButton skill2;
    private SkillButton skill3;
    private UltimateButton ultimate; 

    private void Awake()
    {
        timeBackgroun = GameObject.Find("TimeBackground");
        if (!GameObject.Find("GameTime").TryGetComponent<TextMeshProUGUI>(out countdown))
        {
            Debug.Log("SpawnMananger - Awake - TextMeshProUGUI");
        }
        time = 900;
        
        if (!GameObject.Find("KillCount").TryGetComponent<TextMeshProUGUI>(out killCount))
        {
            Debug.Log("MenuManager - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("BasicAttack").TryGetComponent<SkillButton>(out basic))
        {
            Debug.Log("PlayerController - Awake - Button");
        }

        if (!GameObject.Find("Skill1").TryGetComponent<SkillButton>(out skill1))
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
        if (!GameObject.Find("Ultimate").TryGetComponent<UltimateButton>(out ultimate))
        {
            Debug.Log("MenuManager - Awake - UltimateButton");
        }
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

    public void InitSkillButton(int num, int skill, string name, int level)
    {
        switch(num)
        { 
            case 0:
                basic.Init(0, skill, name, level); 
                break;
            case 1:
                skill1.Init(1,skill, name, level);
                break;
            case 2:
                skill2.Init(2,skill, name, level);
                break;
            case 3:
                skill3.Init(3,skill, name, level);
                break;
            case 4:
                ultimate.InitSkill1(4,skill, name);
                break;
            case 5:
                ultimate.InitSkill2(5,skill, name);
                break;
        }
    }

    public void SetUaltimate(int value)
    {
        ultimate.SetUaltimate(value);
    }

    public void SetKillCount(int value)
    {
        killCount.text = value.ToString();
    }
}
