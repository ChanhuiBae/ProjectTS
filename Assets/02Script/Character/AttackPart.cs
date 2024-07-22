using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPart : MonoBehaviour
{
    private PatternManager patternManager;
    private int key;
    public int Key
    {
        set => key = value;
    }

    private void Awake()
    {
        if(!GameObject.Find("PatternManager").TryGetComponent<PatternManager>(out patternManager))
        {
            Debug.Log("AttackPart - Awake - PatternManager");
        }
        key = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(key != 0 && other.tag == "Player")
        {
            patternManager.TakeDamageOther(4000, key, other);
        }
    }
}
