using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAnimationController : MonoBehaviour
{
    private Animator animator;
    private int H_IsMove = Animator.StringToHash("IsMove");
    private int H_Hit = Animator.StringToHash("Hit");
    private int H_Pattern = Animator.StringToHash("Pattern");
    private int H_Stun = Animator.StringToHash("Stun");

    private void Awake()
    {
        if(!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("CreatureAnimationController - Awake - Animator");
        }
    }

    public void SetPattern(int pattern)
    {
        Debug.Log("id" + pattern);
        animator.SetInteger(H_Pattern, pattern);
    }

    public void SetStun(bool use)
    {
        animator.SetBool(H_Stun, use);
    }

    public void Hit()
    {
        animator.SetTrigger(H_Hit);
    }

    public void Move(bool use)
    {
        animator.SetBool(H_IsMove, use);
    }


}
