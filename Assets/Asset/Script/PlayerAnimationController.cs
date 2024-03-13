using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private int H_NormalAttack = Animator.StringToHash("NormalAttack");
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

    public void NormalAttack()
    {
        animator.SetTrigger(H_NormalAttack);
    }

}
