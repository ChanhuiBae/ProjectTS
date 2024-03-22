using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Idle,
    Move,
    Attack_Soward,
    Attack_AR,
    Roll
}

public class PlayerController : MonoBehaviour, IDamage
{
    private Rigidbody rig;
    private FloatingJoystick joystick;
    private WeaponType type;
    private Button sowrdAttack;
    private FixedJoystick ARAttack;
    private PlayerAnimationController anim;
    private Button roll;
    private bool isControll = true;
    public bool CONTROLL
    {
        set => isControll = value;
        get => isControll;
    }
    private bool isInvincibility;
    private State state;
    private Weapon weapon;
    private Image ultimateFill;
    private float ultimateValue;
    private Vector3 direction;
    private Vector3 look;
    private float currentHP;
    private float currentEXP;
    private Image expFill;

    private void Awake()
    {
        if(!TryGetComponent<Rigidbody>(out rig))
        {
            Debug.Log("PlayerController - Awake - Rigidbody");
        }
        if(!GameObject.Find("SowrdAttack").TryGetComponent<Button>(out sowrdAttack))
        {
            Debug.Log("PlayerController - Awake - Button");
        }
        if(!GameObject.Find("Floating Joystick").TryGetComponent< FloatingJoystick > (out joystick))
        {
            Debug.Log("PlayerController - Awake - Floating Joystick");
        }
 
        if (!TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("PlayerController - Awake - PlayerAnimationController");
        }

        if(!GameObject.Find("ARAttack").TryGetComponent<FixedJoystick>(out ARAttack))
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
        if(!GameObject.Find("ExperienceFill").TryGetComponent<Image>(out expFill))
        {
            Debug.Log("PlayerController - Awake - Image");
        }
    }

    public void Init()
    {
        type = WeaponType.AR;
        anim.Weapon((int)type);
        switch (type)
        {
            case WeaponType.Sorwd:
                sowrdAttack.onClick.AddListener(SowrdAttack);
                ARAttack.gameObject.SetActive(false);
                break;
            case WeaponType.AR:
                sowrdAttack.gameObject.SetActive(false);
                break;
        }
        ultimateValue = 0;
        ultimateFill.fillAmount = 0;
        isInvincibility = false;
        currentHP = GameManager.Inst.PlayerInfo.Max_HP;
        currentEXP = 0;
        state = State.Idle;
        expFill.fillAmount = 0; 
        isControll = true;
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
            case State.Move:
                anim.Move(false);
                break;
            case State.Attack_Soward:
                anim.Attack(false);
                break;
            case State.Attack_AR:
                anim.Combat(false);
                break;
            case State.Roll: 
                break;
        }
        this.state = state;
        switch(this.state)
        {
            case State.Idle:
                break;
            case State.Move:
                anim.Move(true);
                break;
            case State.Attack_Soward:
                anim.Attack(true);
                break;
            case State.Attack_AR:
                transform.LookAt(transform.position + look);
                anim.Combat(true);
                break;
            case State.Roll:
                anim.Roll(true);
                transform.LookAt(transform.position + direction);
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

            if(direction != Vector3.zero && state != State.Roll)
            {
                ChangeState(State.Move);
                rig.MovePosition(transform.position + direction * GameManager.Inst.PlayerInfo.Move_Speed * Time.deltaTime);
            }
            else
            {
                ChangeState(State.Idle);
            }

            if (state != State.Attack_AR)
            {
                transform.LookAt(transform.position + direction);
            }
            
            if (type == WeaponType.AR)
            {
                look = Vector3.forward * ARAttack.Vertical + Vector3.right * ARAttack.Horizontal;
            }
            if (look != Vector3.zero)
            {
                ChangeState(State.Attack_AR);
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
                anim.Combat(false);
            }
        }
    }

    private void SowrdAttack()
    {
        anim.Attack(true);
    }




    private void Roll()
    {
        if(isControll && state != State.Roll)
            ChangeState(State.Roll);
    }

    private IEnumerator RollDelay()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        rig.MovePosition(transform.position + transform.forward * GameManager.Inst.PlayerInfo.Avoid_Distance * Time.deltaTime);
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
        else if(currentHP > GameManager.Inst.PlayerInfo.Max_HP)
        {
            currentHP = GameManager.Inst.PlayerInfo.Max_HP;
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
        if(currentEXP > GameManager.Inst.PlayerInfo.Exp_Need)
        {
            currentEXP = 0;
            // todo : Level Up
        }
        expFill.fillAmount = currentEXP / GameManager.Inst.PlayerInfo.Exp_Need;
    }

    public float GetCritical_Mag()
    {
        return 1f; // todo : Calculate value
    }
}
