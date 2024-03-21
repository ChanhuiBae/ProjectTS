using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Idle,
    Attack,
    Roll
}

public class PlayerController : MonoBehaviour, IDamage
{
    private Rigidbody rig;
    private FloatingJoystick joystick;
    private FixedJoystick normalAttack;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float dashDistance;
    private PlayerAnimationController anim;
    private Button roll;
    private bool isControll;
    private bool isInvincibility;
    private State state;
    private Weapon weapon;
    private Image ultimateFill;
    private float ultimateValue;
    private Vector3 direction;
    private Vector3 look;
    private float currentHP;
    private float maxHP;
    private float currentEXP;
    private float maxEXP;
    private Image expFill;

    private void Awake()
    {
        if(!TryGetComponent<Rigidbody>(out rig))
        {
            Debug.Log("PlayerController - Awake - Rigidbody");
        }

        if(!GameObject.Find("Floating Joystick").TryGetComponent< FloatingJoystick > (out joystick))
        {
            Debug.Log("PlayerController - Awake - Floating Joystick");
        }
        if (!TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("PlayerController - Awake - PlayerAnimationController");
        }

        if(!GameObject.Find("NormalAttack").TryGetComponent<FixedJoystick>(out normalAttack))
        {
            Debug.Log("PlayerController - Awake - FixedJoystic");
        }

        if (!GameObject.Find("Roll").TryGetComponent<Button>(out roll))
        {
            Debug.Log("PlayerController - Awake - Button");
        }
        else
        {
            roll.onClick.AddListener(Roll);
        }
        
        if(!GameObject.Find("Weapon").TryGetComponent<Weapon>(out weapon))
        {
            Debug.Log("PlayerController - Awake - Weapon");
        }
        if(!GameObject.Find("UltimateFill").TryGetComponent<Image>(out ultimateFill))
        {
            Debug.Log("PlayerController - Awake - Image");
        }
        else
        {
            ultimateFill.fillAmount = 0;
        }
        if(!GameObject.Find("ExperienceFill").TryGetComponent<Image>(out expFill))
        {
            Debug.Log("PlayerController - Awake - Image");
        }
        else
        {
            expFill.fillAmount = 0;
        }
        ultimateValue = 0;
        isControll = true;
        isInvincibility = false;
        maxHP = 10f; // todo : get MaxHP
        currentHP = maxHP;
        maxEXP = 10f; // todo : get MaxEXP;
        currentEXP = 0;
        state = State.Idle;
    }

    private void ChangeState(State state)
    {
        if(this.state == state)
        {
            return;
        }
        switch (this.state)
        {
            case State.Idle:
                break;
            case State.Attack:
                anim.Attack(false);
                break;
            case State.Roll: 
                break;
        }
        this.state = state;
        switch(this.state)
        {
            case State.Idle:
                break;
            case State.Attack:
                transform.LookAt(transform.position + look);
                anim.Attack(true);
                break;
            case State.Roll:
                anim.Roll(true);
                transform.LookAt(transform.position + direction);
                anim.MoveDir(direction.x, direction.z);
                StartCoroutine(RollDelay());
                break;
        }
    }

    private void Update()
    {
        if (isControll)
        {
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.z = Input.GetAxisRaw("Vertical");
            direction += Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            direction.Normalize();
            look = Vector3.forward * normalAttack.Vertical + Vector3.right * normalAttack.Horizontal;

            if(direction != Vector3.zero && state != State.Roll)
            {
                rig.MovePosition(transform.position + direction * speed * Time.deltaTime);
                anim.Move(true);
                float angle = Quaternion.Angle(transform.rotation, Quaternion.identity);
                if (Mathf.Abs(angle) > 90)
                {
                    anim.MoveDir(-direction.x, -direction.z);
                }
                else
                {
                    anim.MoveDir(direction.x, direction.z);
                }
            }
            else
            {
                anim.Move(false);
            }

            if (look != Vector3.zero)
            {
                if(state != State.Attack)
                {
                    transform.LookAt(transform.position + look);
                }
                ChangeState(State.Attack);
            }
        }
    }


    private void Roll()
    {
        if(isControll && state != State.Roll)
            ChangeState(State.Roll);
    }

    private IEnumerator RollDelay()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        rig.MovePosition(transform.position + transform.forward * dashDistance * Time.deltaTime);
        anim.Roll(false);
        ChangeState(State.Idle);
    }

    public void GetDamage(ITakeDamage hiter)
    {
        if (!isInvincibility)
        {
            ApplyHP(hiter.TakeDamage());
        }
    }

    public void ApplyHP(float value)
    {
        currentHP -= value;
        if(currentHP < 0)
        {
            currentHP = 0;
        }
        else if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void ChargeUaltimateGage(float value)
    {
        ultimateValue += value;
        ultimateFill.fillAmount = ultimateValue;
    }

    public void RollStart()
    {
        isInvincibility = true;
    }

    public void RollEnd()
    {
        isInvincibility = false;
    }

    public void AttackStart()
    {
        weapon.NormalAttack();
    }

    public void AttackDamageZero()
    {
        weapon.ResetDamage();
    }

    public void AttackEnd()
    {
        ChangeState(State.Idle);
    }
    public void GetEXP(float value)
    {
        currentEXP += value;
        if(currentEXP > maxEXP)
        {
            currentEXP = 0;
            // todo : Level Up
        }
        expFill.fillAmount = currentEXP / maxEXP;
    }
}
