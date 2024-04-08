using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private PlayerController player;
    private Image coolTimeImage;
    private Image icon;
    private float maxTime;
    private float currentTime;
    private int skill_ID;
    private SkillManager skillManager;
    private int buttonNum;
    private EventTrigger trigger;
    private EventTrigger.Entry down;
    private EventTrigger.Entry up;
 

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
        if(!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
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
        skillManager.IsCharge = true;
        AttackSkill();
    }

    void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("up");
        skillManager.IsCharge = false;
    }

    public void Init(int buttonNum, int id)
    {
        this.buttonNum = buttonNum;
        skill_ID = id;
        if(skill_ID == 0)
        {
            icon.sprite = Resources.Load<Sprite>("Image/NoneSkill");
            coolTimeImage.fillAmount = 0;
            trigger.enabled = false;
        }
        else
        {
            icon.sprite = Resources.Load<Sprite>("Image/Hammer");
            trigger.enabled = false;
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
        trigger.enabled = true;
    }

    private void AttackSkill()
    {
        currentTime = 0;
        player.ChangeState(State.Attack_Skill);
        player.UseSkill(skill_ID);
        skillManager.UseSkill(buttonNum);
        StartCoroutine (CoolTime());
        trigger.enabled = false;
    }
}
