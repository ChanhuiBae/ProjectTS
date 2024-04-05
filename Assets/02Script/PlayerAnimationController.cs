using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private int H_Weapon = Animator.StringToHash("Weapon");
    private int H_Attack1 = Animator.StringToHash("Attack1");
    private int H_Attack2 = Animator.StringToHash("Attack2");
    private int H_Move = Animator.StringToHash("IsMove");
    private int H_Roll = Animator.StringToHash("IsRoll");
    private int H_Combat = Animator.StringToHash("IsCombat");
    private int H_X = Animator.StringToHash("X");
    private int H_Y = Animator.StringToHash("Y");
    private int H_Gordian_Wheel = Animator.StringToHash("Gordian_Wheel");
    private int H_Dragon_Hammer = Animator.StringToHash("Dragon_Hammer");
    private int H_Attraction_Field = Animator.StringToHash("Attraction_Field");
    private int H_Samsara = Animator.StringToHash("Samsara");
    private int H_Naraka = Animator.StringToHash("Naraka");

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

    public void Attack(int count, bool use)
    {
        if (!use)
        {
            animator.ResetTrigger(H_Attack1);
            animator.ResetTrigger(H_Attack2);
        }
        switch (count)
        {
            case 0:
                animator.SetTrigger(H_Attack1);
                break;
            case 1:
                animator.SetTrigger(H_Attack2);
                break;

        }

    }

    public void Roll(bool use)
    {
        if (use)
        {
            animator.SetTrigger(H_Roll);
            Move(false);
            Attack(0,false);

        }
        else
        {
            animator.ResetTrigger(H_Roll);
        }
    }

    public void Gordian_Wheel(bool use)
    {
        if (use)
        {
            animator.SetTrigger(H_Gordian_Wheel);
        }
        else
        {
            animator.ResetTrigger(H_Gordian_Wheel);
        }
    }


    public void Dragon_Hammer(bool use)
    {
        if (use)
        {
            animator.SetTrigger(H_Dragon_Hammer);
        }
        else
        {
            animator.ResetTrigger(H_Dragon_Hammer);
        }

    }

    public void Attaction_Field(bool use)
    {
        if (use)
        {
            animator.SetTrigger(H_Attraction_Field);
        }
        else
        {
            animator.ResetTrigger(H_Attraction_Field);
        }
    }

    public void Samsara(bool use)
    {
        if (use)
        {
            animator.SetTrigger(H_Samsara);
        }
        else
        {
            animator.ResetTrigger(H_Samsara);
        }
    }

    public void Naraka(bool use)
    {
        animator.SetBool(H_Naraka, use);
    }

}