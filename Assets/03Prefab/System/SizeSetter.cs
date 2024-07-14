using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeSetter : MonoBehaviour
{
    private BoxCollider col;

    private void Awake()
    {
        if(!TryGetComponent<BoxCollider>(out col))
        {
            Debug.Log("SizeSetter - Awake - BoxCollider");
        }
        else
        {
            col.size = new Vector3(Screen.width/50, 1, Screen.height/50);
        }
    }
}
