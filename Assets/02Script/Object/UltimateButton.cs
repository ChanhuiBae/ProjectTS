using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UltimateButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private PlayerController player;
    private Image ultimateFill;
    private TextMeshProUGUI ultimateText;
    private Image icon;
    private RectTransform center;

    private int ID;
    private int connectedID;
    private string name;
    private string connectedName;
    private int buttonNum;
    private int connectedNum;
    private bool isScoping;
    private bool connectedScoping;
    private bool isCharging;
    private bool connectedCharging;

    private SkillManager skillManager;

    private EventTrigger trigger;
    private EventTrigger.Entry down;
    private EventTrigger.Entry up;
    private EventTrigger.Entry drag;
    private EventTrigger.Entry dragEnd;

    private bool isUse;
    private bool connectedUse;
    private bool UseDrag;


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

        if (!transform.Find("UltimateFill").TryGetComponent<Image>(out ultimateFill))
        {
            Debug.Log("UltimateManager - Awake - Image");
        }
        if (!transform.Find("UltimateValue").TryGetComponent<TextMeshProUGUI>(out ultimateText))
        {
            Debug.Log("UltimateManager - Awake - TextMeshProUGUI");
        }
        if (!transform.Find("Icon").TryGetComponent<Image>(out icon))
        {
            Debug.Log("UltimateButton - Awake - Image");
        }
        if (!transform.GetChild(0).TryGetComponent<RectTransform>(out center))
        {
            Debug.Log("SkillButton - Awake - RectTransform");
        }
        if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("UltimateButton - Awake - SkillManager");
        }
        if (!TryGetComponent<EventTrigger>(out trigger))
        {
            Debug.Log("UltimateButton - Awake -  EventTrigger");
        }
        down = new EventTrigger.Entry();
        up = new EventTrigger.Entry(); 
        drag = new EventTrigger.Entry();
        dragEnd = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        up.eventID = EventTriggerType.PointerUp;
        drag.eventID = EventTriggerType.Drag;
        dragEnd.eventID = EventTriggerType.EndDrag;
    }

    public void SetUaltimate(float value)
    {
        ultimateFill.fillAmount = value;
        value = (float)Math.Truncate(value * 1000)/10;
        ultimateText.text = value.ToString() + "%";
        if(ultimateFill.fillAmount == 1)
        {
            ultimateText.text = "";
            trigger.enabled = true;
            icon.transform.SetSiblingIndex(3);
        }
    }

    void OnPointerDown(PointerEventData eventData)
    {
        ultimateFill.fillAmount = 1;
        if (ultimateFill.fillAmount >= 1)
        {
            if (player.ChangeState(State.Attack_Skill))
            {
                isUse = true;
                skillManager.IsCharge = true;
                AttackSkill();
            }
        }
     
        if (isUse)
        {
            skillManager.IsCharge = false;
            GameManager.Inst.ResetUltimate();
            connectedUse = false;
        }
        if(ID == 331)
        {
            player.StopAllCoroutines();
            skillManager.SetBoxes();
        }
        else
        {
            StartCoroutine(CheckEnd());
        }
    }

    void OnPointerUp(PointerEventData eventData)
    {
        UseDrag = !UseDrag;
        if (connectedID != 0)
        {
            if (isUse && connectedUse)
            {
                connectedUse = false;
                transform.SetSiblingIndex(5);
                skillManager.StopAttackArea();
                icon.rectTransform.position = center.position;
                AttackConnectedSkill();
                icon.sprite = Resources.Load<Sprite>("Image/" + name); 
                icon.transform.SetSiblingIndex(0);
                trigger.enabled = false;
            }
            else
            {
                icon.sprite = Resources.Load<Sprite>("Image/" + connectedName);
            }
        }
        else
        {
            icon.rectTransform.position = center.position;
            icon.transform.SetSiblingIndex(0);
            icon.sprite = Resources.Load<Sprite>("Image/" + name);
            trigger.enabled = false;
            if (ID == 331)
            {
                skillManager.AttackAnim();
                skillManager.DropMissile();

                SetUaltimate(1);  // todo: delete. this is using for test.
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 direction = Vector3.zero;
        if (UseDrag || (isScoping && isUse))
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

            if (Mathf.Abs(distance) <= 50)
                icon.rectTransform.localPosition = new Vector2(x, y);
            else
            {
                float theta = Mathf.Acos(dot);
                x = Mathf.Cos(theta) * 50;
                if (cross.z > 0)
                {
                    y = Mathf.Sin(theta) * 50;
                    y = -y;
                }
                else
                {
                    y = Mathf.Sin(theta) * 50;
                }
                icon.rectTransform.localPosition = new Vector2(x, y);
            }
            direction = new Vector3(x, 0, y);
        }
           

        if (isScoping && isUse)
        {
            if (ID == 331)
            {
                skillManager.MoveBoxes(direction);
            }
        }
        else if (UseDrag)
        {
            connectedUse = true;
            if (connectedScoping)
            {
                skillManager.StopAttackArea();
                skillManager.MoveAttackArea(direction * 0.075f, 2);
                skillManager.ShowAttackArea(4);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isUse && connectedUse)
        {
            UseDrag = false;
            connectedUse = false;
            transform.SetSiblingIndex(5);
            skillManager.StopAttackArea();
            icon.rectTransform.position = center.position;
            AttackConnectedSkill();
            icon.sprite = Resources.Load<Sprite>("Image/" + name);
            icon.transform.SetSiblingIndex(0);
            trigger.enabled = false;
        }
    }

    public void InitSkill(int buttonNum, int id, string name)
    {
        down.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(down);
        this.buttonNum = buttonNum;
        this.name = name;
        ID = id;
        isUse = false;
        if (id == 0)
        {
            icon.sprite = Resources.Load<Sprite>("Image/NoneSkill");
            trigger.enabled = false;
            ultimateText.enabled  = false;
        }
        else
        {
            icon.sprite = Resources.Load<Sprite>("Image/" + name);
            trigger.enabled = true;
            icon.transform.SetSiblingIndex(3);
            TableEntity_Skill skill;
            GameManager.Inst.GetSkillData(ID, out skill);
            isScoping = skill.Is_Scoping;
            if(isScoping)
            {
                SetDrag();
            }
            isCharging = skill.Is_Charging;
            if (isCharging)
            {
                up = new EventTrigger.Entry();
                up.eventID = EventTriggerType.PointerUp;
                up.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
                trigger.triggers.Add(up);
            }
            ultimateText.enabled = true;
        }
    }

    public void InitConnectedSkill(int buttonNum, int id, string name)
    {
        connectedNum = buttonNum;
        connectedName = name;
        connectedID = id;
        connectedUse = false;
        if (id != 0)
        {
            TableEntity_Skill skill;
            GameManager.Inst.GetSkillData(connectedID, out skill);
            connectedScoping = skill.Is_Scoping;
            connectedCharging = skill.Is_Charging;
            if (connectedScoping)
            {
                SetDrag();
            }
            isCharging = skill.Is_Charging;
            if (connectedCharging)
            {
                up = new EventTrigger.Entry();
                up.eventID = EventTriggerType.PointerUp;
                up.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
                trigger.triggers.Add(up);
            }
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

    private void AttackSkill()
    {
        player.UseSkill(ID);
        skillManager.UseSkill(buttonNum);
    }

    public void AttackConnectedSkill()
    {
        if (player.GetCurrentState() == State.Attack_Skill)
        {
            player.UseSkill(connectedID);
            skillManager.UseSkill(connectedNum);
        }
    }

    private IEnumerator CheckEnd()
    {
        for (int i = 0; i < 80; i++)
        {
            yield return null;
        }
        isUse = false;
        skillManager.StopAttackArea();
        icon.rectTransform.position = center.position;
        icon.transform.SetSiblingIndex(0);
        icon.sprite = Resources.Load<Sprite>("Image/" + name);
        trigger.enabled = false;

        SetUaltimate(1);  // todo: delete. this is using for test.
    }

}
