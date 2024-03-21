using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPItem : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private string name;
    private PoolManager poolManager;
    private Rigidbody rig;
    private float hp;

    private void Awake()
    {
        if (!GameObject.Find("PoolManager").TryGetComponent<PoolManager>(out poolManager))
        {
            Debug.Log("HPItem - Awake - PoolManager");
        }
        if (!TryGetComponent<Rigidbody>(out  rig))
        {
            Debug.Log("HPItem - Awkae - Rigidbody");
        }
    }

    public void Init(float hp, Vector3 pos)
    {
        this.hp = hp;
        transform.position = pos + new Vector3(Random.Range(-2f, 2f), 3f, Random.Range(-2f, 2f));
        rig.useGravity = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            rig.useGravity = false;
            rig.velocity = Vector3.zero;
        }
        if(other.tag == "Player")
        {
            if (other.transform.root.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.ApplyHP(-hp);
                poolManager.TakeToPool<HPItem>(name, this);
            }
        }
    }

    public void OnCreatedInPool()
    {
        //throw new System.NotImplementedException();
    }

    public void OnGettingFromPool()
    {
        //throw new System.NotImplementedException();
    }

    
}
