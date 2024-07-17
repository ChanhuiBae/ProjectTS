using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAnimationController : MonoBehaviour
{
    private Animator animator;
    private int H_IsMove = Animator.StringToHash("IsMove");
    private int H_Stagger = Animator.StringToHash("Stagger");
    private int H_Pattern = Animator.StringToHash("Pattern");
    private int H_Groggy = Animator.StringToHash("Groggy");
    private int H_Struge = Animator.StringToHash("Struge");


    private void Awake()
    {
        if(!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("CreatureAnimationController - Awake - Animator");
        }
    }

    public void SetPattern(int pattern)
    {
        animator.SetInteger(H_Pattern, pattern);
    }

    public void SetGroggy(bool use)
    {
        animator.SetBool(H_Groggy, use);
    }

    public void SetStagger(bool use)
    {
        animator.SetBool(H_Stagger, use);
    }

    public void Move(bool use)
    {
        animator.SetBool(H_IsMove, use);
    }

    public void Struge()
    {
        animator.SetTrigger(H_Struge);
    }


}
