using Redcode.Pools;
using System.Collections;
using UnityEngine;

public enum ProjectileType
{
    Bullet,
    Grenade,
    Laser
}
public class Projectile : MonoBehaviour, IPoolObject
{
    private ProjectileType type;
    private Rigidbody rig;
    private TrailRenderer trail;
    [SerializeField]
    private string poolName;
    private FixedJoystick dir;
    private SkillManager skillManager;

    private int key;
    public int Key
    {
        get { return key; }
    }

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
    public void Init(ProjectileType type, Vector3 pos, int key)
    {
        this.type = type;
        transform.position = pos;
        trail.enabled = true;
        this.key = key;
    }

    public void AttackBullet(Quaternion rotation)
    {
        transform.rotation = rotation;
        rig.velocity = Vector3.zero;
        rig.AddForce(transform.forward * 8f, ForceMode.Impulse);
        StartCoroutine(TimeOut(5f));
    }
    public void AttackGrenade(Quaternion rotation)
    {
        transform.rotation = rotation;
        rig.velocity = Vector3.zero;
        rig.useGravity = true;
        rig.AddForce(transform.forward * 6f, ForceMode.Impulse);
        StartCoroutine(TimeOut(5f));
    }

    public void AttackLaser(Quaternion rotation)
    {
        transform.rotation = rotation;
        rig.velocity = Vector3.zero;
        rig.AddForce(transform.forward * 8f, ForceMode.Impulse);
        StartCoroutine(TimeOut(0.5f));
    }

    private IEnumerator TimeOut(float time)
    {
        yield return YieldInstructionCache.WaitForSeconds(time);
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
        switch (type)
        {
            case ProjectileType.Bullet:
                if (other.tag == "Creature")
                {
                    skillManager.TakeDamageByKey(AttackType.Projectile, key, other);
                    skillManager.TakeProjectile(poolName, this);
                }
                if (other.tag == "Ground")
                {
                    trail.enabled = false;
                    skillManager.TakeProjectile(poolName, this);
                }
                break;
            case ProjectileType.Grenade:
                if (other.tag == "Ground")
                {
                    trail.enabled = false;
                    rig.velocity = Vector3.zero;
                    Effect effect = skillManager.SpawnEffect(8);
                    effect.Init(EffectType.Once, transform.position, 1f);
                    effect.Key = key;
                    skillManager.TakeProjectile(poolName, this);
                }
                break;
            case ProjectileType.Laser:
                if (other.tag == "Creature")
                {
                    skillManager.TakeDamageByKey(AttackType.Projectile, key, other);
                }
                break;
        }

    }
}
