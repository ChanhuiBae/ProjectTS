using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private int H_Weapon = Animator.StringToHash("Weapon");
    private int H_Attack = Animator.StringToHash("Attack");
    private int H_Move = Animator.StringToHash("IsMove");
    private int H_Roll = Animator.StringToHash("IsRoll");
    private int H_Combat = Animator.StringToHash("IsCombat");
    private int H_X = Animator.StringToHash("X");
    private int H_Y = Animator.StringToHash("Y");

    private void Awake()
    {
        if (!TryGetComponent<Animator>(out animator))
        {
            Debug.Log("PlayerAnimationController - Awake - Animator");
        }
    }

    public void Weapon(int weapon)
    {
        animator.SetInteger(H_Weapon, weapon);
    }

    public void Combat(bool value)
    {
        animator.SetBool(H_Combat, value);
    }

    public void Move(bool value)
    {
        animator.SetBool(H_Move, value);
    }

    public void MoveDir(float x, float y)
    {
        animator.SetFloat(H_X, x);
        animator.SetFloat(H_Y, y);
    }

    public void Attack(bool use)
    {
        if (use)
        {
            animator.SetTrigger(H_Attack);
        }
        else
        {
            animator.ResetTrigger(H_Attack);
        }
    }

    

    public void Roll(bool use)
    {
        if (use)
        {
            animator.SetTrigger(H_Roll);
        }
        else
        {
            animator.ResetTrigger(H_Roll);
        }
    }
}