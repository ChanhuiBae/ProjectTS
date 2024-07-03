using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Choice : MonoBehaviour
{
    private Image pick;
    private Image icon;
    private TextMeshProUGUI name;
    private TextMeshProUGUI description;
    private Button btn;
    private bool isLevelUp;

    private void Awake()
    {
        if(!transform.Find("Pick").TryGetComponent<Image>(out pick))
        {
            Debug.Log("Choice - Awake - Image");
        }
        else
        {
            pick.enabled = false;
        }
        if(!transform.Find("Icon").TryGetComponent<Image>(out icon))
        {
            Debug.Log("Choice - Awake - Image");
        }
        if(transform.Find("Name").TryGetComponent<TextMeshProUGUI>(out name))
        {
            Debug.Log("Choice - Awake - TextMeshProUGUI");
        }
        if (transform.Find("Description").TryGetComponent<TextMeshProUGUI>(out description))
        {
            Debug.Log("Choice - Awake - TextMeshProUGUI");
        }
        if(!TryGetComponent<Button>(out btn))
        {
            Debug.Log("Choice - Awake - Button");
        }
        else
        {
            btn.onClick.AddListener(PickUp);
        }
        isLevelUp = false;
    }

    public void SetNewSkill(int id)
    {
        isLevelUp = false;
        TableEntity_Skill skill;
        GameManager.Inst.GetSkillData(id, out skill);
        icon.sprite = Resources.Load<Sprite>("Image/" + skill.Skill_Name_Eng);
        name.text = skill.Skill_Name;
        description.text = skill.Explanation;
    }

    public void SetNewPassive(int id)
    {
        isLevelUp = false;
        TableEntitiy_Passive_Skill skill;
        GameManager.Inst.GetPassiveData(id, out skill);
        icon.sprite = Resources.Load<Sprite>("Image/" + skill.Name_Eng);
        name.text = skill.Name;
        description.text = skill.Description;
    }

    public void LevelUpSkill(int id, int level)
    {
        isLevelUp = true;
        TableEntity_Skill skill;
        GameManager.Inst.GetSkillData(id, out skill);
        icon.sprite = Resources.Load<Sprite>("Image/" + skill.Skill_Name_Eng);
        name.text = skill.Skill_Name;
        description.text = level.ToString() + " -> " + (level + 1).ToString(); 
    }

    public void LevelUpPassive(int id, int level)
    {
        isLevelUp = true;
        TableEntitiy_Passive_Skill skill;
        GameManager.Inst.GetPassiveData(id, out skill);
        icon.sprite = Resources.Load<Sprite>("Image/" + skill.Name_Eng);
        name.text = skill.Name;
        description.text = level.ToString() + " -> " + (level + 1).ToString();
    }

    private void PickUp()
    {
        
    }
}
