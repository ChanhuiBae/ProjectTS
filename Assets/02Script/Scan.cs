using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scan : MonoBehaviour
{
    private SphereCollider col;
    private void Awake()
    {
        if (!TryGetComponent<SphereCollider>(out col))
            Debug.Log("Scan - Awake - SphereCollider");
        else
        {
            col.isTrigger = true;
            col.radius = 20f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<CretureAI>().SetTarget(other.gameObject);
        }
    }
}
