using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Idle,
    Move,
    Attack,
    Roll
}

public class PlayerController : MonoBehaviour, IDamage
{
    private Rigidbody rig;
    private FloatingJoystick joystick;
    [SerializeField]
    private int weaponType;
    private Button sowrdAttack;
    private FixedJoystick soldierAttack;
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
        else
        {
            if(weaponType == 0)
            {
                sowrdAttack.onClick.AddListener(SowrdAttack);
            }
            else
            {
                sowrdAttack.gameObject.SetActive(false);
            }
        }
        if(!GameObject.Find("Floating Joystick").TryGetComponent< FloatingJoystick > (out joystick))
        {
            Debug.Log("PlayerController - Awake - Floating Joystick");
        }
 
        if (!TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("PlayerController - Awake - PlayerAnimationController");
        }

        if(!GameObject.Find("SoldierAttack").TryGetComponent<FixedJoystick>(out soldierAttack))
        {
            Debug.Log("PlayerController - Awake - FixedJoystic");
        }
        else
        {
            if(weaponType != 2)
            {
                soldierAttack.gameObject.SetActive(false);
            }
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
            case State.Attack:
                anim.Attack(false);
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
            case State.Attack:
                transform.LookAt(transform.position + look);
                if(weaponType != 2)
                    anim.Attack(true);
                anim.Combat(true);
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
            if(weaponType == 2)
            {
                look = Vector3.forward * soldierAttack.Vertical + Vector3.right * soldierAttack.Horizontal;
            }

            if(direction != Vector3.zero && state != State.Roll)
            {
                ChangeState(State.Move);
                rig.MovePosition(transform.position + direction * GameManager.Inst.PlayerInfo.Move_Speed * Time.deltaTime);
                float angle = Quaternion.Angle(transform.rotation, Quaternion.identity);
                if(state == State.Attack)
                {
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
                    transform.LookAt(transform.position + direction);
                }
            }
            else
            {
                ChangeState(State.Idle);
            }

            if (weaponType == 2 && look != Vector3.zero)
            {
                if(state != State.Attack)
                {
                    transform.LookAt(transform.position + look);
                    
                }
                ChangeState(State.Attack);
            }
        }
    }

    private void SowrdAttack()
    {
        ChangeState(State.Attack);
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
