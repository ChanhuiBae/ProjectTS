using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillJoystick : MonoBehaviour
{
    private PlayerController player;
    private float maxTime;
    private float currentTime;
    private Image icon;
    private int skill_ID;
    private SkillManager skillManager;
    private int buttonNum;
    private FixedJoystick joystick;
    private bool UseSkill;
    private Vector3 direction;


    private void Awake()
    {
        GameObject obj = GameObject.Find("Player");
        if (obj != null)
        {
            if (!obj.TryGetComponent<PlayerController>(out player))
            {
                Debug.Log("SkillJoystick - Awake - PlayerController");
            }
        }

        if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("SkillJoystick - Awake - SkillManager");
        }

        if(!TryGetComponent<FixedJoystick>(out joystick))
        {
            Debug.Log("SkillJoystick - Awake - Fixedjoystick");
        }
    }

    public void Init(int buttonNum, int id, string name)
    {
        this.buttonNum = buttonNum;
        skill_ID = id;
        UseSkill = false;
        if (skill_ID == 0)
        {
            icon.sprite = Resources.Load<Sprite>("Image/NoneSkill");
        }
        else
        {
            icon.sprite = Resources.Load<Sprite>("Image/" + name);
            TableEntity_Skill_List skill;
            GameManager.Inst.GetSkillList(skill_ID, out skill);
            string key = skill.ID + skill.Weapon_ID + skill.Category_ID + "101";
            TableEntity_Skill info;
            GameManager.Inst.GetSkillData(int.Parse(key), out info);
            maxTime = info.Cool_Time;
            currentTime = maxTime;
        }
        icon.enabled = false;
        joystick.enabled = false;
    }

    private IEnumerator CoolTime()
    {
        while (currentTime < maxTime)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            currentTime += 0.1f;
        }
        icon.enabled = false;
        joystick.enabled = false;
    }

    public void UseJoystick()
    {
        icon.enabled = true;
        joystick.enabled = true;
        currentTime = 0;
        StartCoroutine(CoolTime());
    }
    private void GetDirection()
    {
        direction += Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        direction.Normalize();
    }

    private void AttackSkill()
    {
        player.ChangeState(State.Attack_Skill);
        player.UseSkill(skill_ID);
        skillManager.UseSkill(buttonNum);
    }
}
