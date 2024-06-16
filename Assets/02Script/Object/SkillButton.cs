using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private PlayerController player;
    private SkillManager skillManager;

    private Image coolTimeImage;
    private Image icon;
    private Image bar;
    private RectTransform center;
    private Image block;
    private TextMeshProUGUI time;
    private TextMeshProUGUI stackCount;
    
    private int maxStack;
    private int currentStack;
    private float maxTime;
    private float currentTime;
    private float scopingTime;

    private int skill_ID;
    private int connectedID;
    private string name;
    private string connectedName;
    private int buttonNum;
    private int connectedNum;
    private bool isScoping;
    private bool connectedScoping;
    private bool isCharging;
    private bool connectedCharging;

    private bool isUse;
    private bool connectedUse;
    private bool UseDrag;

    private bool coolTimeRun;
    private Vector3 direction;

    private EventTrigger trigger;
    private EventTrigger.Entry down;
    private EventTrigger.Entry up;
    private EventTrigger.Entry drag;
    private EventTrigger.Entry dragEnd;

    private bool isDrag;
    private float dragDistance;
 

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
            if (!transform.Find("Stack").TryGetComponent<TextMeshProUGUI>(out stackCount))
            {
                Debug.Log("SkillButton - Awake - TextMeshProUGUI");
            }
            if(!transform.Find("Bar").TryGetComponent<Image>(out bar))
            {
                Debug.Log("SkillButton - Awake - Image");
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
        down.eventID = EventTriggerType.PointerDown;
        down.callback.AddListener((data)  => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(down);
    }

    void OnPointerDown(PointerEventData eventData)
    {
        if(transform.childCount == 1)
        {
            BasicAttack(); 
            skillManager.UseSkill(0);
        }
        else
        {
            if (player.ChangeState(State.Attack_Skill))
            {
                skillManager.IsCharge = true;
                AttackSkill();
                isUse = true;
                if (!isCharging && !isScoping)
                {
                    skillManager.IsCharge = false;
                    skillManager.StopCharge();
                    trigger.enabled = false;
                    StartCoroutine(CoolTime());
                }
                if (maxStack > 0)
                {
                    skillManager.MoveAttackArea(player.transform.forward, 2);
                    skillManager.ShowAttackArea();
                }
            }
            else
            {
                if(currentStack > 0)
                {
                    player.IsCombo(true);
                }
            }
            if (scopingTime > 0)
            {
                StartCoroutine(InitTimer(scopingTime));
            }
            if(currentStack < maxStack && !coolTimeRun)
            {
                StartCoroutine(CoolTime());
            }
        }
        isDrag = false;
    }

    void OnPointerUp(PointerEventData eventData)
    {
        if(buttonNum == skillManager.GetCurrentSkill())
        {
            if (isScoping)
            {
                icon.rectTransform.position = center.position;
                if (bar != null)
                {
                    bar.rectTransform.localPosition = center.position;
                }
                skillManager.SetLook(Vector3.zero);
            }
            if (player.GetCurrentState() == State.Attack_Skill)
            {
                if (isCharging)
                {
                    skillManager.IsCharge = false;
                    skillManager.StopCharge();
                    trigger.enabled = false;
                    StartCoroutine(CoolTime());
                }
                if (isScoping && maxStack == 0)
                {
                    player.StopAllCoroutines();
                    player.SetIdle();
                    trigger.enabled = false;
                    StartCoroutine(CoolTime());
                }
                if (maxStack > 0)
                {
                    if (isDrag)
                        skillManager.PushVector(direction);
                    else
                        skillManager.PushVector(player.transform.forward);
                    skillManager.StopAttackArea();
                    player.Sit();
                }
                if (currentStack == 0)
                {
                    trigger.enabled = false;
                    block.enabled = true;
                    time.enabled = true;
                }
                if (currentStack > 0)
                {
                    currentStack--;
                    stackCount.text = currentStack.ToString();
                }
            }
            if (!isDrag && !isCharging && maxStack == 0)
            {
                player.SetIdle();
            }
            isDrag = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (buttonNum == skillManager.GetCurrentSkill() && isScoping && trigger.enabled)
        {
            isDrag = true;
            Vector2 drag = eventData.position;
            float x = eventData.position.x - center.position.x;
            float y = eventData.position.y - center.position.y;
            float distance = Mathf.Sqrt(x * x + y * y);

            Vector3 v1 = new Vector3(x, y, 0f);
            v1.Normalize();
            Vector3 v2 = new Vector3(1, 0, 0);
            Vector3 cross = Vector3.Cross(v1, v2);
            float dot = Vector3.Dot(v1, v2);

            if (Mathf.Abs(distance) <= 40)
            {
                icon.rectTransform.localPosition = new Vector2(x, y);
                if (bar != null)
                {
                    bar.rectTransform.localPosition = center.position;
                }
            }
            else
            {
                float theta = Mathf.Acos(dot);
                x = Mathf.Cos(theta) * dragDistance;
                if (cross.z > 0)
                {
                    y = Mathf.Sin(theta) * dragDistance;
                    y = -y;
                }
                else
                {
                    y = Mathf.Sin(theta) * dragDistance;
                }
                icon.rectTransform.localPosition = new Vector2(x, y);
                bar.rectTransform.localPosition = new Vector2(x, y);
            }
            direction = new Vector3(x, 0, y);
            if (maxStack != 0)
            {
                skillManager.ShowAttackArea();
                skillManager.MoveAttackArea(direction*0.25f, 2);
                if(skillManager.GetVectorCount() == 0)
                    player.LookAttackArea();
            }
            else
            {
                skillManager.SetLook(direction);
            }
            
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (buttonNum == skillManager.GetCurrentSkill() && player.GetCurrentState() == State.Attack_Skill)
        {
            icon.rectTransform.position = center.position;
            if (bar != null)
            {
                bar.rectTransform.localPosition = center.position;
            }
            if (maxStack == 0)
            {
                player.StopAllCoroutines();
                player.SetIdle();
                trigger.enabled = false;
                skillManager.SetLook(Vector3.zero);
                StartCoroutine(CoolTime());
            }
            if (maxStack > 0)
            {
                if (isDrag)
                    skillManager.PushVector(direction);
                skillManager.StopAttackArea();
                player.Sit();
            }

            if (currentStack == 0)
            {
                trigger.enabled = false;
                block.enabled = true;
                time.enabled = true;
            }
        }
        isDrag = false;
    }


    public void Init(int buttonNum, int id, string name, int level)
    {
        this.buttonNum = buttonNum;
        skill_ID = id;
        this.name = name;
        isUse = false;
        if (skill_ID == 0)
        {
            icon.sprite = Resources.Load<Sprite>("Image/NoneSkill");
            coolTimeImage.fillAmount = 0;
            trigger.enabled = false;
        }
        else
        {
            if(skill_ID > 300)
            {
                dragDistance = 35;
            }
            else
            {
                dragDistance = 30;
            }
            trigger.enabled = true;
            TableEntity_Skill skill;
            GameManager.Inst.GetSkillData(skill_ID,out skill);
            isScoping = skill.Is_Scoping;
            if (isScoping)
            {
                SetDrag();
            }
            scopingTime = skill.Scoping_Time;
            isCharging = skill.Is_Charging;
            if (isCharging)
            {
                up = new EventTrigger.Entry();
                up.eventID = EventTriggerType.PointerUp;
                up.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
                trigger.triggers.Add(up);
            }
            maxStack = skill.Stack_Max;
            string key = skill.ID + skill.Weapon_ID + skill.Category_ID + level + "01";
            TableEntity_Skill_Info info;
            GameManager.Inst.GetSkillInfoData(int.Parse(key), out info);
            icon.sprite = Resources.Load<Sprite>("Image/" + name);
            if (transform.childCount != 1)
            {
                maxTime = info.Cool_Time;
                currentTime = maxTime;
                coolTimeImage.fillAmount = 1;
                stackCount.text = "";
                currentStack = 0;
                coolTimeRun = false;
                StartCoroutine(CoolTime());
            }
        }
    }

    public void InitConnectedSkill(int buttonNum, int id, string name, int level)
    {
        connectedNum = buttonNum;
        connectedID = id;
        connectedName = name;
        if (skill_ID != 0)
        {
            TableEntity_Skill skill;
            GameManager.Inst.GetSkillData(connectedID, out skill);
            connectedScoping = skill.Is_Scoping;
            if (connectedScoping)
            {
                SetDrag();
            }
            connectedCharging = skill.Is_Charging;
            if (connectedCharging)
            {
                up = new EventTrigger.Entry();
                up.eventID = EventTriggerType.PointerUp;
                up.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
                trigger.triggers.Add(up);
            }
            connectedUse = false;
        }
    }

    private void SetDrag()
    {
        up = new EventTrigger.Entry();
        up.eventID = EventTriggerType.PointerUp;
        up.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(up);

        drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.Drag;
        drag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(drag);

        dragEnd = new EventTrigger.Entry();
        dragEnd.eventID = EventTriggerType.EndDrag;
        dragEnd.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
        trigger.triggers.Add(dragEnd);
    }

    private IEnumerator CoolTime()
    {
        currentTime = 0;
        coolTimeRun = true;
        coolTimeImage.enabled = true;
        if (currentStack == 0)
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
            trigger.enabled = true;
            block.enabled = false;
            time.enabled = false;
            icon.transform.SetSiblingIndex(1);
            coolTimeImage.enabled = false;
        }
        else
        {
            while (currentTime < maxTime)
            {
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
                currentTime += 0.1f;
                coolTimeImage.fillAmount = currentTime / maxTime;
                time.text = ((int)(maxTime - currentTime)).ToString();
            }
            trigger.enabled = true;
            block.enabled = false;
            time.enabled = false;
            icon.transform.SetSiblingIndex(1);
        }

        if(maxStack != 0)
        {
            currentStack++;
            stackCount.text = currentStack.ToString();
        }
        
        if (currentStack < maxStack)
        {
            currentTime = 0;
            StartCoroutine(CoolTime());
        }
        else if(maxStack != 0 && currentStack >= maxStack)
        {
            currentStack = maxStack;
            stackCount.text = currentStack.ToString();
            coolTimeImage.enabled = false;
        }
        coolTimeRun = false;
    }

    private void BasicAttack()
    {
        if(skill_ID < 200)
        {
            player.SwordAttack();
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
        player.ChangeState(State.Attack_Skill);
        player.UseSkill(skill_ID);
        skillManager.UseSkill(buttonNum);
    }

    private IEnumerator InitTimer(float time)
    {
        yield return YieldInstructionCache.WaitForSeconds(time);
        icon.rectTransform.position = center.position;
        bar.rectTransform.localPosition = center.position;
        skillManager.IsCharge = false;
        skillManager.StopCharge();
        skillManager.SetLook(Vector3.zero);
        trigger.enabled = false;
        StartCoroutine(CoolTime());
    }

    public void AttackConnectedSkill()
    {
        player.UseSkill(connectedID);
        skillManager.UseSkill(connectedNum);
    }

}
