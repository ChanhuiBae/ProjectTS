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

    private int ID1;
    private int ID2;
    private string name1;
    private string name2;
    private int buttonNum1;
    private int buttonNum2;
    private bool isScoping1;
    private bool isScoping2;
    private bool isCharging1;
    private bool isCharging2;

    private SkillManager skillManager;

    private EventTrigger trigger;
    private EventTrigger.Entry down;
    private EventTrigger.Entry up;
    private EventTrigger.Entry drag;
    private EventTrigger.Entry dragEnd;

    private bool UseSkill1;
    private bool UseSkill2;
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
        down.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(down);
        up.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(up);
        drag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(drag);
        dragEnd.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
        trigger.triggers.Add(dragEnd);
    }

    public void SetUaltimate(float value)
    {
        ultimateFill.fillAmount = value;
        value = (float)Math.Truncate(value * 1000)/10;
        ultimateText.text = value.ToString() + "%";
        if(ultimateFill.fillAmount == 1)
        {
            trigger.enabled = true;
            icon.transform.SetSiblingIndex(3);
        }
    }

    void OnPointerDown(PointerEventData eventData)
    {
        if (player.GetCurrentState() != State.Attack_Skill)
        {
            if(ultimateFill.fillAmount >= 1)
            {
                UseSkill1 = true;
                skillManager.IsCharge = true;
                AttackSkill1();
            }
        }
        else
        {
            if (UseSkill1)
            {
                UseDrag = true;
            }
            else
            {
                UseDrag = false;
            }
        }
        if (UseSkill1)
        {
            skillManager.IsCharge = false;
            GameManager.Inst.ResetUltimate();
            StartCoroutine(CheckEnd());
            UseSkill2 = false;
        }
    }

    void OnPointerUp(PointerEventData eventData)
    {
        if (ID2 != 0)
        {
            if (UseSkill1 && UseSkill2)
            {
                UseSkill2 = false;
                transform.SetSiblingIndex(5);
                skillManager.StopAttackArea();
                icon.rectTransform.position = center.position;
                AttackSkill2();
                icon.sprite = Resources.Load<Sprite>("Image/" + name1); 
                icon.transform.SetSiblingIndex(0);
                trigger.enabled = false;
            }
            else
            {
                icon.sprite = Resources.Load<Sprite>("Image/" + name2);
            }
        }
        else
        {
            icon.rectTransform.position = center.position;
            icon.transform.SetSiblingIndex(0);
            trigger.enabled = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isScoping1 && UseSkill1)
        {

        }
        else if (UseSkill1 && UseDrag)
        {
            UseSkill2 = true;
            if (isScoping2)
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
                Vector3 direction = new Vector3(x, 0, y);
                skillManager.StopAttackArea();
                skillManager.MoveAttackArea(direction * 0.075f, 2);
                skillManager.ShowAttackArea();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (UseSkill1 && UseSkill2)
        {
            UseSkill2 = false;
            transform.SetSiblingIndex(5);
            skillManager.StopAttackArea();
            icon.rectTransform.position = center.position;
            AttackSkill2();
            icon.sprite = Resources.Load<Sprite>("Image/" + name1);
            icon.transform.SetSiblingIndex(0);
            trigger.enabled = false;
        }
    }

    public void InitSkill1(int buttonNum, int id, string name)
    {
        buttonNum1 = buttonNum;
        name1 = name;
        ID1 = id;
        UseSkill1 = false;
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
            TableEntity_Skill_List skill;
            GameManager.Inst.GetSkillList(ID1, out skill);
            isScoping1 = skill.Is_Scoping;
            isCharging1 = skill.Is_Charging;
            ultimateText.enabled = true;
        }
    }

    public void InitSkill2(int buttonNum, int id, string name)
    {
        buttonNum2 = buttonNum;
        name2 = name;
        ID2 = id;
        UseSkill2 = false;
        if (id != 0)
        {
            TableEntity_Skill_List skill;
            GameManager.Inst.GetSkillList(ID2, out skill);
            isScoping2 = skill.Is_Scoping;
            isCharging2 = skill.Is_Charging;
        }
    }

    private void AttackSkill1()
    {
        player.ChangeState(State.Attack_Skill);
        player.UseSkill(ID1);
        skillManager.UseSkill(buttonNum1);
    }

    public void AttackSkill2()
    {
        player.ChangeState(State.Attack_Skill);
        player.UseSkill(ID2);
        skillManager.UseSkill(buttonNum2);
    }

    private IEnumerator CheckEnd()
    {
        bool IsUse = false;
        for (int i = 0; i < 80; i++)
        {
            yield return null;
        }
        UseSkill1 = false;
        skillManager.StopAttackArea();
        icon.rectTransform.position = center.position;
        icon.transform.SetSiblingIndex(0);
        icon.sprite = Resources.Load<Sprite>("Image/" + name1);
        trigger.enabled = false;
    }
}
