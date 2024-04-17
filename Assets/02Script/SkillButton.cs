using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private PlayerController player;
    private Image coolTimeImage;
    private Image icon;
    private Image block;
    private TextMeshProUGUI time;
    private float maxTime;
    private float currentTime;
    private int skill_ID;
    private SkillManager skillManager;
    private int buttonNum;
    private EventTrigger trigger;
    private EventTrigger.Entry down;
    private EventTrigger.Entry up;
    private bool UseSkill;
 

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
        if(!transform.GetChild(0).TryGetComponent<Image>(out coolTimeImage))
        {
            Debug.Log("SkillButton - Awake - Image");
        }
        if(!transform.GetChild(1).TryGetComponent<Image>(out icon))
        {
            Debug.Log("SkillButton - Awake - Image");
        }
        if (!transform.GetChild(2).TryGetComponent<Image>(out block))
        {
            Debug.Log("SkillButton - Awake - Image");
        }
        if (!transform.GetChild(3).TryGetComponent<TextMeshProUGUI>(out time))
        {
            Debug.Log("SkillButton - Awake - TextMeshProUGUI");
        }
        if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("SkillButton - Awake - SkillManager");
        }
        if(!TryGetComponent<EventTrigger>(out trigger))
        {
            Debug.Log("SkillButton - Awake -  EventTrigger");
        }
        down = new EventTrigger.Entry();
        up = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        up.eventID = EventTriggerType.PointerUp;
        down.callback.AddListener((data)  => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(down);
        up.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(up);
    }

    void OnPointerDown(PointerEventData eventData)
    {
        if(player.CurrentState() != State.Attack_Skill)
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

    public void Init(int buttonNum, int id, string name)
    {
        this.buttonNum = buttonNum;
        skill_ID = id;
        UseSkill = false;
        if (skill_ID == 0)
        {
            icon.sprite = Resources.Load<Sprite>("Image/NoneSkill");
            coolTimeImage.fillAmount = 0;
            trigger.enabled = false;
        }
        else
        {
            icon.sprite = Resources.Load<Sprite>("Image/"+ name);
            trigger.enabled = true;
            TableEntity_Skill_List skill;
            GameManager.Inst.GetSkillList(skill_ID,out skill);
            string key = skill.ID + skill.Weapon_ID + skill.Category_ID + "101";
            TableEntity_Skill info;
            GameManager.Inst.GetSkillData(int.Parse(key), out info);
            maxTime = info.Cool_Time;
            currentTime = maxTime;
            coolTimeImage.fillAmount = 1;
            StartCoroutine(CoolTime());
        }
    }

    private IEnumerator CoolTime()
    {
        coolTimeImage.enabled = true;
        block.enabled = true;
        time.enabled = true;
        icon.transform.SetSiblingIndex(0);
        while (currentTime < maxTime)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            currentTime += 0.1f;
            coolTimeImage.fillAmount = currentTime / maxTime;
            time.text = ((int)(maxTime - currentTime)).ToString();
        }
        UseSkill = false;
        trigger.enabled = true;
        block.enabled = false;
        time.enabled = false;
        icon.transform.SetSiblingIndex(1);
        coolTimeImage.enabled = false;
    }

    private void AttackSkill()
    {
        currentTime = 0;
        player.ChangeState(State.Attack_Skill);
        player.UseSkill(skill_ID);
        skillManager.UseSkill(buttonNum);
        StartCoroutine (CoolTime());
    }
}
