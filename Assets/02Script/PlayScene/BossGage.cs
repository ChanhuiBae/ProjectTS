using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossGage : MonoBehaviour
{
    private Image bossHP;

    private void Awake()
    {
        if(!transform.Find("Fill").TryGetComponent<Image>(out bossHP))
        {
            Debug.Log("BossGage - Awake - Image");
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetBossHP(float value)
    {
        bossHP.fillAmount = value;
    }
}
