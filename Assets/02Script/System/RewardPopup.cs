using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{
    private Image active1;
    private Image active2;
    private Image active3;

    private Image passive1;
    private Image passive2;
    private Image passive3;

    private Image ultimate;

    private Image reward1;
    private Image reward2;
    private Image reward3;
    private Image reward4;
    private Image reward5;

    private TextMeshProUGUI DNA;
    private TextMeshProUGUI EXP;

    private Button acept;

    private void Awake()
    {
        if(!transform.Find("Active1").TryGetComponent<Image>(out active1))
        {
            Debug.Log("RewardPopup - Awake - Image");
        }
        if(!transform.Find("Active2").TryGetComponent<Image>(out active2))
        {
            Debug.Log("RewardPopup - Awake - Image");
        }
        if(!transform.Find("Active3").TryGetComponent<Image>(out active3))
        {
            Debug.Log("RewardPopup - Awake - Image");
        }
        if(!transform.Find("Passive1").TryGetComponent<Image>(out passive1))
        {
            Debug.Log("RewardPopup - Awake - Image");
        }
        if(!transform.Find("Passive2").TryGetComponent<Image>(out passive2))
        {
            Debug.Log("RewardPopup - Awake - Image");
        }
        if(!transform.Find("Passive3").TryGetComponent<Image>(out passive3))
        {
            Debug.Log("RewardPopup - Awkae - Image");
        }
        if (!transform.Find("Ultimate").TryGetComponent<Image>(out ultimate))
        {
            Debug.Log("RewardPopup - Awkae - Image");
        }
        if (!transform.Find("Reward1").TryGetComponent<Image>(out reward1))
        {
            Debug.Log("RewardPopup - Awkae - Image");
        }
        if (!transform.Find("Reward2").TryGetComponent<Image>(out reward2))
        {
            Debug.Log("RewardPopup - Awkae - Image");
        }
        if (!transform.Find("Reward3").TryGetComponent<Image>(out reward3))
        {
            Debug.Log("RewardPopup - Awkae - Image");
        }
        if (!transform.Find("Reward4").TryGetComponent<Image>(out reward4))
        {
            Debug.Log("RewardPopup - Awkae - Image");
        }
        if (!transform.Find("Reward5").TryGetComponent<Image>(out reward5))
        {
            Debug.Log("RewardPopup - Awkae - Image");
        }
        if(!transform.Find("DNA").TryGetComponent<TextMeshProUGUI>(out DNA))
        {
            Debug.Log("RewardPopup - Awkae - TextMeshProUGUI");
        }
        if(!transform.Find("EXP").TryGetComponent<TextMeshProUGUI>(out EXP))
        {
            Debug.Log("RewardPopup - Awkae - TextMeshProUGUI");
        }
        if(!transform.Find("Acept").TryGetComponent<Button>(out acept))
        {
            Debug.Log("RewardPopup - Awake - Button");
        }
        else
        {
            acept.onClick.AddListener(End);
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);    
    }

    public void SetRewardPopup(int time, bool boss)
    {
        if(GameManager.Inst.WeaponSkillData.skill1_ID != 0)
        {
            TableEntity_Skill data;
            GameManager.Inst.GetSkillData(GameManager.Inst.WeaponSkillData.skill1_ID, out data);
            active1.sprite = Resources.Load<Sprite>("Image/" + data.Skill_Name_Eng);
        }
        if (GameManager.Inst.WeaponSkillData.skill2_ID != 0)
        {
            TableEntity_Skill data;
            GameManager.Inst.GetSkillData(GameManager.Inst.WeaponSkillData.skill2_ID, out data);
            active2.sprite = Resources.Load<Sprite>("Image/" + data.Skill_Name_Eng);
        }
        if (GameManager.Inst.WeaponSkillData.skill3_ID != 0)
        {
            TableEntity_Skill data;
            GameManager.Inst.GetSkillData(GameManager.Inst.WeaponSkillData.skill3_ID, out data);
            active3.sprite = Resources.Load<Sprite>("Image/" + data.Skill_Name_Eng);
        }
        if (GameManager.Inst.WeaponSkillData.ultimate_ID != 0)
        {
            TableEntity_Skill data;
            GameManager.Inst.GetSkillData(GameManager.Inst.WeaponSkillData.ultimate_ID, out data);
            ultimate.sprite = Resources.Load<Sprite>("Image/" + data.Skill_Name_Eng);
        }
        if (GameManager.Inst.WeaponSkillData.passive1_ID != 0)
        {
            TableEntitiy_Passive_Skill data;
            GameManager.Inst.GetPassiveData(GameManager.Inst.WeaponSkillData.passive1_ID, out data);
            passive1.sprite = Resources.Load<Sprite>("Image/" + data.Name_Eng);
        }
        if (GameManager.Inst.WeaponSkillData.passive2_ID != 0)
        {
            TableEntitiy_Passive_Skill data;
            GameManager.Inst.GetPassiveData(GameManager.Inst.WeaponSkillData.passive2_ID, out data);
            passive2.sprite = Resources.Load<Sprite>("Image/" + data.Name_Eng);
        }
        if (GameManager.Inst.WeaponSkillData.passive3_ID != 0)
        {
            TableEntitiy_Passive_Skill data;
            GameManager.Inst.GetPassiveData(GameManager.Inst.WeaponSkillData.passive3_ID, out data);
            passive3.sprite = Resources.Load<Sprite>("Image/" + data.Name_Eng);
        }

        if(time > 480)
        {

        }
        else if (time > 360)
        {

        }
        else if(time > 240)
        {

        }
        else if(time > 120)
        {

        }
        else if(!boss)
        {

        }
        else
        {

        }
    }

    private void End()
    {
        GameManager.Inst.AsyncLoadNextScene(SceneName.LobbyScene);
    }
}
