using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Idle,
    Attack_Soward,
    Attack_Gun,
    Roll
}

public class PlayerController : MonoBehaviour, IDamage
{
    private Rigidbody rig;
    private FloatingJoystick joystick;
    private WeaponType type;
    private Button sowrdAttack;
    private Button hammerAttack;
    private FixedJoystick gunAttack;
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
        if (!GameObject.Find("Floating Joystick").TryGetComponent< FloatingJoystick > (out joystick))
        {
            Debug.Log("PlayerController - Awake - Floating Joystick");
        }
 
        if (!TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("PlayerController - Awake - PlayerAnimationController");
        }

        if (!GameObject.Find("SowrdAttack").TryGetComponent<Button>(out sowrdAttack))
        {
            Debug.Log("PlayerController - Awake - Button");
        }
        if (!GameObject.Find("HammerAttack").TryGetComponent<Button>(out hammerAttack))
        {
            Debug.Log("PlayerController - Awake - Button");
        }
        if (!GameObject.Find("GunAttack").TryGetComponent<FixedJoystick>(out gunAttack))
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
       
        if(!GameObject.Find("UltimateFill").TryGetComponent<Image>(out ultimateFill))
        {
            Debug.Log("PlayerController - Awake - Image");
        }
        if(!GameObject.Find("ExperienceFill").TryGetComponent<Image>(out expFill))
        {
            Debug.Log("PlayerController - Awake - Image");
        }
        type = WeaponType.Gun;
        Init();
    }

    public void Init()
    {
        anim.Weapon((int)type);
        switch (type)
        {
            case WeaponType.Sowrd:
                sowrdAttack.onClick.AddListener(SowrdAttack);
                if (!GameObject.Find("Sowrd").TryGetComponent<Weapon>(out weapon))
                {
                    Debug.Log("PlayerController - Awake - Weapon");
                }
                hammerAttack.gameObject.SetActive(false);
                //GameObject.Find("Hammer").SetActive(false);
                gunAttack.gameObject.SetActive(false);
                GameObject.Find("Gun").SetActive(false);
                break;
            case WeaponType.Gun:
                sowrdAttack.gameObject.SetActive(false);
                GameObject.Find("Sowrd").SetActive(false);
                hammerAttack.gameObject.SetActive(false);
                //GameObject.Find("Hammer").SetActive(false);
                if (!GameObject.Find("Gun").TryGetComponent<Weapon>(out weapon))
                {
                    Debug.Log("PlayerController - Awake - Weapon");
                }
                break;
        }
        weapon.Init(type);
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
            case State.Attack_Soward:
                anim.Attack(false);
                break;
            case State.Attack_Gun:
                StopAllCoroutines();
                anim.Combat(false);
                break;
            case State.Roll: 
                break;
        }
        this.state = state;
        switch(this.state)
        {
            case State.Idle:
                anim.Move(false);
                break;
            case State.Attack_Soward:
                anim.Attack(true);
                break;
            case State.Attack_Gun:
                anim.Combat(true);
                StartCoroutine(GunAttack());
                break;
            case State.Roll:
                weapon.ResetDamage();
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

            if(direction == Vector3.zero)
            {
                anim.Move(false);
            }
            else if(direction != Vector3.zero && state != State.Roll && state != State.Attack_Soward)
            {
                anim.Move(true);
                rig.MovePosition(transform.position + direction * GameManager.Inst.PlayerInfo.Move_Speed * Time.deltaTime);
            }


            if (state != State.Attack_Gun)
            {
                transform.LookAt(transform.position + direction);
            }
            
            if (type == WeaponType.Gun)
            {
                look = Vector3.forward * gunAttack.Vertical + Vector3.right * gunAttack.Horizontal;
            }
            if (look != Vector3.zero)
            {
                ChangeState(State.Attack_Gun);
                float angle = Quaternion.Angle(transform.rotation, Quaternion.identity);
                transform.LookAt(transform.position + look);
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
                ChangeState(State.Idle);
            }
        }
    }

    private void SowrdAttack()
    {
        ChangeState(State.Attack_Soward);
    }

    private IEnumerator GunAttack()
    {
        yield return null;
        while (state == State.Attack_Gun)
        {
            weapon.NormalAttack();
            yield return YieldInstructionCache.WaitForSeconds(2f);
        }
    }


    private void Roll()
    {
        if(isControll && state != State.Roll)
            ChangeState(State.Roll);
    }

    private IEnumerator RollDelay()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        rig.AddForce(transform.forward * GameManager.Inst.PlayerInfo.Avoid_Distance, ForceMode.Impulse);
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
