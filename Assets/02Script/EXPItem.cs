using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPItem : MonoBehaviour
{
    [SerializeField]
    private string name;
    private PoolManager poolManager;
    private Rigidbody rig;
    private float exp;

    private void Awake()
    {
        if (!GameObject.Find("PoolManager").TryGetComponent<PoolManager>(out poolManager))
        {
            Debug.Log("EXPItem - Awake - PoolManager");
        }
        if (!TryGetComponent<Rigidbody>(out rig))
        {
            Debug.Log("EXPItem - Awkae - Rigidbody");
        }
    }
    public void Init(float exp, Vector3 pos)
    {
        this.exp = exp;
        transform.position = pos +new Vector3(Random.Range(-2f, 2f), 3f, Random.Range(-2f, 2f));
        rig.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {;
        if (other.tag == "Ground")
        {
            rig.useGravity = false;
            rig.velocity = Vector3.zero;    
        }
        if (other.tag == "Player")
        {
            if (other.transform.root.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.GetEXP(exp);
                poolManager.TakeToPool<EXPItem>(name, this);
            }
        }

    }
}
