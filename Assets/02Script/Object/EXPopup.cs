using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXPopup : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        if(!TryGetComponent<Button>(out button))
        {
            Debug.Log("");
        }
        else
        {
            button.onClick.AddListener(Close);
        }
    }


    private void Close()
    {
        gameObject.SetActive(false);
    }

}
