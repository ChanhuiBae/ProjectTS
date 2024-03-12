using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private int H_NormalAttack = Animator.StringToHash("NormalAttack");
    private int H_MoveX = Animator.StringToHash("X");
    private int H_MoveY = Animator.StringToHash("Y");
    private int H_Run = Animator.StringToHash("Run");

    private void Awake()
    {
        if(!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("PlayerAnimationController - Awake - Animator");
        }
    }

    public void SetRun(bool value)
    {
        animator.SetBool(H_Run, value);
    }

    public void SetX(float value)
    {
        animator.SetFloat(H_MoveX, value*10);
    }

    public void SetY(float value)
    {
        animator.SetFloat(H_MoveY, value*10);
    }

    public void NormalAttack()
    {
        animator.SetTrigger(H_NormalAttack);
    }

}
