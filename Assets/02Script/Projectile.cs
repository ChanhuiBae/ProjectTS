using Redcode.Pools;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolObject
{
    private Rigidbody rig;
    private float damage;
    private TrailRenderer trail;
    [SerializeField]
    private string poolName;
    private FixedJoystick dir;
    private SkillManager skillManager;
    
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
        if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("Projectile - Awake - SkillManager");
        }

    }
    public void Init(Vector3 pos)
    {
        transform.position = pos;
        trail.enabled = true;
    }

    public void AttackBullet(Quaternion rotation)
    {
        transform.rotation = rotation;
        rig.velocity = Vector3.zero;
        rig.AddForce(transform.forward * 8f, ForceMode.Impulse);
        StartCoroutine(TimeOut());
    }
    public void AttackGrenade(Quaternion rotation)
    {
        transform.rotation = rotation;
        rig.velocity = Vector3.zero;
        rig.useGravity = true;
        rig.AddForce(transform.forward * 5f + Vector3.up * 2f, ForceMode.Impulse);
        StartCoroutine(TimeOut());
    }

    private IEnumerator TimeOut()
    {
        yield return YieldInstructionCache.WaitForSeconds(5f);
        trail.enabled = false;
        skillManager.TakeProjectile(poolName, this);
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
        if(other.tag == "Creature")
        {
            skillManager.TakeDamageOther("Projectile", other);
            skillManager.TakeProjectile(poolName, this);
        }
        if(other.tag == "Ground")
        {
            trail.enabled = false;
            skillManager.TakeProjectile(poolName, this);
        }
    }
}
