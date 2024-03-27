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
        LeanTween.move(gameObject, new Vector3(transform.position.x, 1.5f, transform.position.z), 1f).setEase(LeanTweenType.easeOutBounce);
    }
    private IEnumerator Absorb(GameObject target)
    {
        Vector3 goal = Vector3.zero;
        while (transform.position != goal)
        {
            goal = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            LeanTween.move(gameObject, goal, 0.3f);
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
        }
        poolManager.TakeToPool<HPItem>(name, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            rig.velocity = Vector3.zero;
        }
        if(other.tag == "Player" && rig.velocity == Vector3.zero)
        {
            if (other.transform.root.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.ApplyHP(-hp);
                StartCoroutine(Absorb(other.gameObject));
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
