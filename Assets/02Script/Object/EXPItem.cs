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
    public float EXP
    {
        get => exp;
    }

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
        transform.position = pos + new Vector3(Random.Range(-2f, 2f), 3f, Random.Range(-2f, 2f));
        LeanTween.move(gameObject, new Vector3(transform.position.x,0.5f, transform.position.z), 1f).setEase(LeanTweenType.easeOutBounce);
    }

    private IEnumerator Absorb(GameObject target)
    {
        LeanTween.rotate(gameObject, new Vector3(0f, 1080f, 0f), 1f).setEaseInOutQuad();
        yield return YieldInstructionCache.WaitForSeconds(1f);
        Vector3 goal = Vector3.zero;
        while (transform.position != goal)
        {
            goal = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            LeanTween.move(gameObject, goal, 0.3f);
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
        }
        GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_EXP);
        poolManager.TakeToPool<EXPItem>(name, this);
    }

    public void ReturnItem()
    {
        poolManager.TakeToPool<EXPItem>(name, this);
    }

    private void OnTriggerEnter(Collider other)
    {;
        if (other.tag == "Ground")
        {
            rig.velocity = Vector3.zero;
        }
        if (other.tag == "Player" && rig.velocity == Vector3.zero)
        {
            if (other.transform.root.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.GetEXP(exp);
                StartCoroutine(Absorb(other.gameObject));
            }
        }

    }
}
