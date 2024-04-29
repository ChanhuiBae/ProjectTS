using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UltimateButton : MonoBehaviour
{
    private PlayerController player;
    private SkillJoystick ultimate2;
    private Image ultimateFill;
    private TextMeshProUGUI ultimateText;
    private Image icon;
    private float maxTime;
    private float currentTime;
    private int skill_ID;
    private SkillManager skillManager;
    private int buttonNum;
    private EventTrigger trigger;
    private EventTrigger.Entry down;
    private EventTrigger.Entry up;
    private EventTrigger.Entry dragStart;
    private EventTrigger.Entry dragEnd;
    private bool UseSkill;


    private void Awake()
    {
        GameObject obj = GameObject.Find("Player");
        if (obj != null)
        {
            if (!obj.TryGetComponent<PlayerController>(out player))
            {
                Debug.Log("UltimateButton - Awake - PlayerController");
            }
        }
        if(!GameObject.Find("Ultimate2").TryGetComponent<SkillJoystick>(out ultimate2))
        {
            Debug.Log("UltimateButton - Awake - SkillJoystick");
        }
        if (!transform.GetChild(0).TryGetComponent<Image>(out ultimateFill))
        {
            Debug.Log("UltimateManager - Awake - Image");
        }
        if (!transform.GetChild(1).TryGetComponent<TextMeshProUGUI>(out ultimateText))
        {
            Debug.Log("UltimateManager - Awake - TextMeshProUGUI");
        }
        if (!transform.GetChild(2).TryGetComponent<Image>(out icon))
        {
            Debug.Log("UltimateButton - Awake - Image");
        }
        if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("UltimateButton - Awake - SkillManager");
        }
        if (!TryGetComponent<EventTrigger>(out trigger))
        {
            Debug.Log("UltimateButton - Awake -  EventTrigger");
        }
        SetUaltimate(100);
        down = new EventTrigger.Entry();
        up = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        up.eventID = EventTriggerType.PointerUp;
        down.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(down);
        up.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(up);
    }

    public void SetUaltimate(int value)
    {
        ultimateFill.fillAmount = (float)value / 100f;
        ultimateText.text = value.ToString() + "%";
    }

    void OnPointerDown(PointerEventData eventData)
    {
        if (player.GetCurrentState() != State.Attack_Skill)
        {
            if(ultimateFill.fillAmount >= 1)
            {
                UseSkill = true;
                skillManager.IsCharge = true;
                AttackSkill();
            }
        }
    }

    void OnPointerUp(PointerEventData eventData)
    {
        if (UseSkill)
        {
            skillManager.IsCharge = false;
            GameManager.Inst.ResetUltimate();
            transform.SetSiblingIndex(5);
            UseSkill = false;
            StartCoroutine(CheckEnd());
        }
    }

    public void Init(int buttonNum, int id, string name, int level)
    {
        this.buttonNum = buttonNum;
        skill_ID = id;
        UseSkill = false;
        if (skill_ID == 0)
        {
            icon.sprite = Resources.Load<Sprite>("Image/NoneSkill");
            trigger.enabled = false;
        }
        else
        {
            icon.sprite = Resources.Load<Sprite>("Image/" + name);
            trigger.enabled = true;
            TableEntity_Skill_List skill;
            GameManager.Inst.GetSkillList(skill_ID, out skill);
            string key = skill.ID + skill.Weapon_ID + skill.Category_ID + level + "01";
            TableEntity_Skill info;
            GameManager.Inst.GetSkillData(int.Parse(key), out info);
        }
    }

    private void AttackSkill()
    {
        currentTime = 0;
        player.ChangeState(State.Attack_Skill);
        player.UseSkill(skill_ID);
        skillManager.UseSkill(buttonNum);
    }

    private IEnumerator CheckEnd()
    {
        bool IsUse = false;
        for (int i = 0; i < 70; i++)
        {
            yield return null;
            if (ultimate2.GetUseSkill())
            {
                IsUse = true;
                break;
            }
        }
        if (!IsUse)
        {
            transform.SetSiblingIndex(6);
        }
    }
}
