using Redcode.Pools;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolObject, ITakeDamage
{
    private Rigidbody rig;
    private float damage;
    private TrailRenderer trail;
    [SerializeField]
    private string poolName;
    private PoolManager pool;
    private FixedJoystick dir;
    
    private void Awake()
    {
        if (!TryGetComponent<Rigidbody>(out rig))
        {
            Debug.Log("Projectile - Awake - Rigidbody");
        }
        if(!TryGetComponent<TrailRenderer>(out trail))
        {
            Debug.Log("Projectile - Awake - TrailRenderer");
        }
        GameObject pm = GameObject.Find("PoolManager");
        if (pm == null || !pm.TryGetComponent<PoolManager>(out pool))
        {
            Debug.Log("Projectile - Awake - ProjectilePool");
        }
    }
    public void Init(float damage, Vector3 pos)
    {
        this.damage = damage;
        transform.position = pos;
        trail.enabled = true;
        Attack();
    }

    public void Attack()
    {
        rig.velocity = Vector3.zero;
        rig.AddForce(transform.forward * 8f, ForceMode.Impulse);
        StartCoroutine(TimeOut());
    }
    private IEnumerator TimeOut()
    {
        yield return YieldInstructionCache.WaitForSeconds(3f);
        trail.enabled = false;
        pool.TakeToPool<Projectile>(poolName, this);
    }

    private IEnumerator ReturnPool()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        trail.enabled = false;
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
        if(other.tag == "Creture")
        {
            if (other.TryGetComponent<IDamage>(out IDamage creture))
            {
                creture.GetDamage(this);
            }
        }
        if(other.tag == "Ground")
        {
            trail.enabled = false;
            pool.TakeToPool<Projectile>(poolName, this);
        }
    }

    public float TakeDamage()
    {
        StartCoroutine(ReturnPool());
        return damage;
    }

    public float TakeDamage(float Creature_Physics_Cut, float Creature_Fire_Cut, float Creature_Water_Cut, float Creature_Electric_Cut, float Creature_Ice_Cut, float Creature_Wind_Cut)
    {
        return damage;
    }
}
