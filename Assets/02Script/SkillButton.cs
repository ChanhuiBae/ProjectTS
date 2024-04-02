using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private PlayerController player;
    private Button button;
    private Image coolTimeImage;
    private Image icon;
    private float maxTime;
    private float currentTime;
    private State thisState;

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
        if(!TryGetComponent<Button>(out button))
        {
            Debug.Log("SkillButton - Awake - Button");
        }
        if(!transform.GetChild(0).TryGetComponent<Image>(out coolTimeImage))
        {
            Debug.Log("SkillButton - Awake - Image");
        }
        if(!transform.GetChild(1).TryGetComponent<Image>(out icon))
        {
            Debug.Log("SkillButton - Awake - Image");
        }
    }

    public void Init(State state, float coolTime)
    {
        thisState = state;
        icon.sprite = Resources.Load<Sprite>("Image/Hammer"); // State name
        button.onClick.AddListener(AttackSkill);
        button.enabled = false;
        maxTime = coolTime;
        currentTime = 0;
        coolTimeImage.fillAmount = 0;
        StartCoroutine(CoolTime());
    }

    private IEnumerator CoolTime()
    {
        while(currentTime < maxTime)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            currentTime += 0.1f;
            coolTimeImage.fillAmount = currentTime / maxTime;
        }
        button.enabled = true;
    }

    private void AttackSkill()
    {
        currentTime = 0;
        player.ChangeState(thisState);
        StartCoroutine (CoolTime());
        button.enabled = false;
    }
}
