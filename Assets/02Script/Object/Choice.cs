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
    }

    public void SetNew(int id)
    {

    }

    public void SetLevelUp(int id, int level)
    {

    }

    private void PickUp()
    {

    }
}
