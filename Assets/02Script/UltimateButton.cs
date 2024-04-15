using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UltimateButton : MonoBehaviour
{
    private PlayerController player;
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
                Debug.Log("SkillButton - Awake - PlayerController");
            }
        }
        if (!transform.GetChild(0).TryGetComponent<Image>(out ultimateFill))
        {
            Debug.Log("MenuManager - Awake - Image");
        }
        if (!transform.GetChild(1).TryGetComponent<TextMeshProUGUI>(out ultimateText))
        {
            Debug.Log("MenuManager - Awake - TextMeshProUGUI");
        }
        if (!transform.GetChild(2).TryGetComponent<Image>(out icon))
        {
            Debug.Log("SkillButton - Awake - Image");
        }
        if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("SkillButton - Awake - SkillManager");
        }
        if (!TryGetComponent<EventTrigger>(out trigger))
        {
            Debug.Log("SkillButton - Awake -  EventTrigger");
        }
        ultimateFill.fillAmount = 1;
        ultimateText.text = "100%";

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
        if (player.CurrentState() != State.Attack_Skill)
        {
            skillManager.IsCharge = true;
            UseSkill = true;
            AttackSkill();
        }
    }

    void OnPointerUp(PointerEventData eventData)
    {
        if (UseSkill)
        {
            skillManager.IsCharge = false;
            trigger.enabled = false;
        }
    }

    public void Init(int id, string name)
    {
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
            string key = skill.ID + skill.Weapon_ID + skill.Category_ID + "101";
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
}
