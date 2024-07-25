using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform target;
    private bool shack;
    public bool Shack
    {
        set => shack = value;
    }
    private float shackAmount;

    private bool isMove;
    public bool IsMove
    {
        set => isMove = value;
    }

    [SerializeField]
    private Vector3 offset;

    private void Awake()
    {
        InitFollowCamera();
    }

    private bool InitFollowCamera()
    {
        target = GameObject.Find("Player").transform;
        shack = false;
        shackAmount = 0.1f;
        isMove = true;
        return target != null;
    }

    private void Update()
    {
        if (target == null)
        {
            if (InitFollowCamera())
                Debug.Log("FollowCamera - Update() - target is null");
        }
        else if (shack && GameManager.Inst.OnCamaraShake)
        {
            transform.position = ((Vector3)Random.insideUnitCircle) * shackAmount + target.position + offset;
        }
        else if(isMove)
        {
            transform.position = target.position + offset;
        }
    }
}
