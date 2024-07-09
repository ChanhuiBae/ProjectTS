using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecker : MonoBehaviour
{
    private Pattern pattern;

    private void Awake()
    {
        if(!transform.parent.TryGetComponent<Pattern>(out pattern))
        {
            Debug.Log("PlayerChecker - Awake - Pattern");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            pattern.OnEnterPlayer(other);
        }
    }
}
