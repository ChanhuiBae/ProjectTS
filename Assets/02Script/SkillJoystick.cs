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
        if (!transform.GetChild(0).TryGetComponent<Image>(out icon))
        {
            Debug.Log("SkillButton - Awake - Image");
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

    public void Init(int buttonNum, int id, string name, int level)
    {
        this.buttonNum = buttonNum;
        skill_ID = id;
        direction = Vector3.zero;
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
            string key = skill.ID + skill.Weapon_ID + skill.Category_ID + level + "01";
            TableEntity_Skill info;
            GameManager.Inst.GetSkillData(int.Parse(key), out info);
            maxTime = info.Cool_Time;
            currentTime = maxTime;
        }
    }

    private void GetDirection()
    {
        direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        direction.Normalize();
    }

    private void AttackSkill()
    {
        player.ChangeState(State.Attack_Skill);
        player.UseSkill(skill_ID);
        skillManager.UseSkill(buttonNum);
    }

    public bool GetUseSkill()
    {
        return UseSkill;
    }

    private void Update()
    {
        if (skill_ID != 0)
        {
            if (UseSkill)
            {
                GetDirection();
                if (direction == Vector3.zero)
                {
                    if(player.GetCurrentState() == State.Attack_Skill)
                    {
                        UseSkill = false;
                        transform.SetSiblingIndex(5);
                        skillManager.StopAttackArea();
                        AttackSkill();
                    }
                    else
                    {
                        skillManager.StopAttackArea();
                    }
                }
                else
                {
                    skillManager.StopAttackArea();
                    skillManager.MoveAttackArea(direction * 4.5f, 2);
                    skillManager.ShowAttackArea();
                }
            }
            else
            {
                GetDirection();
                if (direction != Vector3.zero)
                {
                    UseSkill = true;
                }
            }
        }
    }
}
