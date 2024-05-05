using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private PlayerController player;

    private bool isScoping;
    private bool isCharging;
    private Image coolTimeImage;
    private Image icon;
    private RectTransform center;
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
    private EventTrigger.Entry drag;
    private EventTrigger.Entry dragEnd;


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
        if(transform.childCount > 1)
        {
            if (!transform.Find("CoolTime").TryGetComponent<Image>(out coolTimeImage))
            {
                Debug.Log("SkillButton - Awake - Image");
            }
            if (!transform.Find("block").TryGetComponent<Image>(out block))
            {
                Debug.Log("SkillButton - Awake - Image");
            }
            if (!transform.Find("SkillTime").TryGetComponent<TextMeshProUGUI>(out time))
            {
                Debug.Log("SkillButton - Awake - TextMeshProUGUI");
            }
        }
        if (!transform.Find("Icon").TryGetComponent<Image>(out icon))
        {
            Debug.Log("SkillButton - Awake - Image");
        }
        if (!transform.TryGetComponent<RectTransform>(out center))
        {
            Debug.Log("SkillButton - Awake - RectTransform");
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
        drag = new EventTrigger.Entry();
        dragEnd = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        up.eventID = EventTriggerType.PointerUp;
        drag.eventID = EventTriggerType.Drag;
        dragEnd.eventID = EventTriggerType.EndDrag;
        down.callback.AddListener((data)  => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(down);
        up.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(up);
        drag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(drag);
        dragEnd.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
        trigger.triggers.Add(dragEnd);
    }

    void OnPointerDown(PointerEventData eventData)
    {
        if(transform.childCount == 1)
        {
            BasicAttack();
        }
        else
        {
            if (player.GetCurrentState() != State.Attack_Skill)
            {
                skillManager.IsCharge = true;
                UseSkill = true;
                AttackSkill();
            }
            if (isScoping)
            {
                StartCoroutine(InitTimer(5));
            }
            if (!isCharging && !isScoping)
            {
                skillManager.IsCharge = false;
                skillManager.StopCharge();
                trigger.enabled = false;
                StartCoroutine(CoolTime());
            }
        }
    }

    void OnPointerUp(PointerEventData eventData)
    {
        if (isScoping)
        {
            icon.rectTransform.position = center.position;
            skillManager.SetLook(Vector3.zero);
        }
        if (UseSkill && (isCharging || isScoping))
        {
            skillManager.IsCharge = false;
            skillManager.StopCharge();
            trigger.enabled = false;
            StartCoroutine(CoolTime());
            if (isScoping)
            {
                UseSkill = false;
                player.StopAllCoroutines();
                player.SetIdle();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isScoping)
        {
            if(skill_ID %100 == 0 || !coolTimeImage.enabled)
            {
                Vector2 drag = eventData.position;
                float x = eventData.position.x - center.position.x;
                float y = eventData.position.y - center.position.y;
                float distance = Mathf.Sqrt(x * x + y * y);

                Vector3 v1 = new Vector3(x, y, 0f);
                v1.Normalize();
                Vector3 v2 = new Vector3(1, 0, 0);
                Vector3 cross = Vector3.Cross(v1, v2);
                float dot = Vector3.Dot(v1, v2);

                if (Mathf.Abs(distance) <= 30)
                    icon.rectTransform.localPosition = new Vector2(x, y);
                else
                {
                    float theta = Mathf.Acos(dot);
                    x = Mathf.Cos(theta) * 30;
                    if (cross.z > 0)
                    {
                        y = Mathf.Sin(theta) * 30;
                        y = -y;
                    }
                    else
                    {

                        y = Mathf.Sin(theta) * 30;
                    }
                    icon.rectTransform.localPosition = new Vector2(x, y);
                }
                skillManager.SetLook(new Vector3(x, 0, y));
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.rectTransform.position = center.position;
        skillManager.SetLook(Vector3.zero);
    }


    public void Init(int buttonNum, int id, string name, int level)
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
            trigger.enabled = true;
            TableEntity_Skill_List skill;
            GameManager.Inst.GetSkillList(skill_ID,out skill);
            isScoping = skill.Is_Scoping;
            isCharging = skill.Is_Charging;
            string key = skill.ID + skill.Weapon_ID + skill.Category_ID + level + "01";
            TableEntity_Skill info;
            GameManager.Inst.GetSkillData(int.Parse(key), out info);
            if (transform.childCount != 1)
            {
                icon.sprite = Resources.Load<Sprite>("Image/" + name);
                maxTime = info.Cool_Time;
                currentTime = maxTime;
                coolTimeImage.fillAmount = 1;
                StartCoroutine(CoolTime());
            }
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

    private void BasicAttack()
    {
        if(skill_ID < 200)
        {
            player.SwordAttack();
            Debug.Log("sword");
        }
        else if(skill_ID < 300)
        {
            player.HammerAttack();
        }
        else
        {
            player.GunAttack();
        }
    }
    private void AttackSkill()
    {
        currentTime = 0;
        player.ChangeState(State.Attack_Skill);
        player.UseSkill(skill_ID);
        skillManager.UseSkill(buttonNum);
    }

    private IEnumerator InitTimer(float time)
    {
        yield return YieldInstructionCache.WaitForSeconds(time);
        icon.rectTransform.position = center.position;
        skillManager.IsCharge = false;
        skillManager.StopCharge();
        skillManager.SetLook(Vector3.zero);
        trigger.enabled = false;
        StartCoroutine(CoolTime());
    }
}
