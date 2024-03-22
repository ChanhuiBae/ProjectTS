using Redcode.Pools;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolObject, ITakeDamage
{
    private Rigidbody rig;
    private float damage;
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
        transform.rotation = Quaternion.identity;
        Attack();
    }

    public void Attack()
    {
        //rig.velocity = transform.position + transform.forward * 100f;
        StartCoroutine(TimeOut());
    }
    private IEnumerator TimeOut()
    {
        yield return YieldInstructionCache.WaitForSeconds(10f);
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

    public float TakeDamage()
    {
        return damage;
    }

    public float TakeDamage(float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        return damage;
    }
}
