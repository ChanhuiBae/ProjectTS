using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private int H_SowrdAttack = Animator.StringToHash("SowrdAttack");
    private int H_SoldierAttack = Animator.StringToHash("SoldierAttack");
    private int H_MeleeMove = Animator.StringToHash("IsMeleeMove");
    private int H_SoldierMove = Animator.StringToHash("IsSoldierMove");
    private int H_Roll = Animator.StringToHash("IsRoll");
    private int H_X = Animator.StringToHash("X");   
    private int H_Y = Animator.StringToHash("Y");
    private int H_TurnLeft = Animator.StringToHash("TurnLeft");
    private int H_TurnRight = Animator.StringToHash("TurnRight");

    private void Awake()
    {
        if(!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("PlayerAnimationController - Awake - Animator");
        }
    }

    public void MeleeMove(bool value)
    {
        animator.SetBool(H_MeleeMove, value);
    }

    public void SoldierMove(bool value)
    {
        animator.SetBool(H_SoldierMove, value);
    }

    public void MoveDir(float x, float y)
    {
        animator.SetFloat(H_X, x);
        animator.SetFloat(H_Y, y);
    }

    public void SowrdAttack(bool use)
    {
        if (use)
        {
            animator.SetTrigger(H_SowrdAttack);
        }
        else
        {
            animator.ResetTrigger(H_SowrdAttack);
        }
    }

    public void Roll(bool value)
    {
        animator.SetBool(H_Roll, value);
    }

    public void TurnLeft(bool value)
    {
        animator.SetBool(H_TurnLeft, value);
    }

    public void TurnRight(bool value)
    {
        animator.SetBool(H_TurnRight, value);
    }
}
