using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private int H_NormalAttack = Animator.StringToHash("NormalAttack");
    private int H_X = Animator.StringToHash("X");
    private int H_Y = Animator.StringToHash("Y");
    private int H_Run = Animator.StringToHash("Run");
    private int H_Dash = Animator.StringToHash("Dash");

    private void Awake()
    {
        if(!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("PlayerAnimationController - Awake - Animator");
        }
    }

    public void SetX(float value)
    {
        animator.SetFloat(H_X, value);
    }
    public void SetY(float value)
    {
        animator.SetFloat(H_Y, value);
    }
    public void NormalAttack()
    {
        animator.SetTrigger(H_NormalAttack);
    }

    public void SetRun(bool value)
    {
        animator.SetBool(H_Run, value);
    }

    public void Dash()
    {
        animator.SetTrigger(H_Dash);
    }
}
