using Redcode.Pools;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolObject
{
    private Rigidbody rig;
    [SerializeField]
    private string poolName;
    private PoolManager pool;
    
    private void Awake()
    {
        if (!TryGetComponent<Rigidbody>(out rig))
        {
            Debug.Log("Projectile - Awake - Rigidbody");
        }
        GameObject pm = GameObject.Find("PoolManager");
        if (pm == null || !pm.TryGetComponent<PoolManager>(out pool))
        {
            Debug.Log("Projectile - Awake - ProjectilePool");
        }
    }
    public void Init(float damage, Vector3 pos)
    {
        transform.position = pos;
        Attack();
    }

    public void Attack()
    {
        rig.AddForce(transform.forward * 50f + Vector3.up, ForceMode.Impulse);
        StartCoroutine(TimeOut());
    }
    private IEnumerator TimeOut()
    {
        yield return YieldInstructionCache.WaitForSeconds(2f);
        pool.TakeToPool<Projectile>(poolName, this);
    }

    public void OnCreatedInPool()
    {
        //throw new System.NotImplementedException();
    }

    public void OnGettingFromPool()
    {
        //throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        pool.TakeToPool<Projectile>(poolName, this);
    }
}
