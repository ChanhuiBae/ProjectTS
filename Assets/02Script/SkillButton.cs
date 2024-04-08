using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private PlayerController player;
    private Button button;
    private Image coolTimeImage;
    private Image icon;
    private float maxTime;
    private float currentTime;
    private int skill_ID;
    private SkillManager skillManager;
    private int buttonNum;

    private void Awake()
    {
        GameObject obj = GameObject.Find("Player");
        if (obj != null)
        {
            if(!obj.TryGetComponent<PlayerController>(out player))
            {
                Debug.Log("SkillButton - Awake - PlayerController");
            }
        }
        if(!TryGetComponent<Button>(out button))
        {
            Debug.Log("SkillButton - Awake - Button");
        }
        if(!transform.GetChild(0).TryGetComponent<Image>(out coolTimeImage))
        {
            Debug.Log("SkillButton - Awake - Image");
        }
        if(!transform.GetChild(1).TryGetComponent<Image>(out icon))
        {
            Debug.Log("SkillButton - Awake - Image");
        }
        if(!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("SkillButton - Awake - SkillManager");
        }
    }

    public void Init(int buttonNum, int id)
    {
        this.buttonNum = buttonNum;
        skill_ID = id;
        if(skill_ID == 0)
        {
            icon.sprite = Resources.Load<Sprite>("Image/NoneSkill");
            coolTimeImage.fillAmount = 0;
            button.enabled = false;
        }
        else
        {
            icon.sprite = Resources.Load<Sprite>("Image/Hammer");
            button.onClick.AddListener(AttackSkill);
            button.enabled = false;
            TableEntity_Skill_List skill;
            GameManager.Inst.GetSkillList(skill_ID,out skill);
            string key = skill.ID + skill.Weapon_ID + skill.Category_ID + "101";
            TableEntity_Skill info;
            GameManager.Inst.GetSkillData(int.Parse(key), out info);
            maxTime = info.Cool_Time;
            currentTime = 0;
            coolTimeImage.fillAmount = 0;
            StartCoroutine(CoolTime());
        }
    }

    private IEnumerator CoolTime()
    {
        while(currentTime < maxTime)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            currentTime += 0.1f;
            coolTimeImage.fillAmount = currentTime / maxTime;
        }
        button.enabled = true;
    }

    private void AttackSkill()
    {
        currentTime = 0;
        player.ChangeState(State.Attack_Skill);
        player.UseSkill(skill_ID);
        skillManager.UseSkill(buttonNum);
        StartCoroutine (CoolTime());
        button.enabled = false;
    }
}
