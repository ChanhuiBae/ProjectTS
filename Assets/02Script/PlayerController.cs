using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Idle,
    MoveForward,
    Attack_Soward,
    Attack_Hammer,
    Attack_Gun,
    Attack_Skill,
    Roll,
    Stun,
    Airborne,
    Knockback,
    Pulled
}

public class PlayerController : MonoBehaviour, IDamage
{
    private Rigidbody rig;
    private FloatingJoystick joystick;
    private WeaponType type;
    [SerializeField]
    private float attackTime;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rollSpeed;
    private Button meleeAttack;
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
    private int attackCount;

    private float currentHP;
    private float currentEXP;
    private Image expFill;

    private Vector3 direction;
    private Vector3 look;

    private SkillManager skillManager;
    private AttackArea attackArea;

    private ParticleSystem charge;
    private ParticleSystem chargeLight;
    private Effect effect;

    private void Awake()
    {
        if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("PlayerController - Awake - SkillManager");
        }
        if (!TryGetComponent<Rigidbody>(out rig))
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

        if (!GameObject.Find("MeleeAttack").TryGetComponent<Button>(out meleeAttack))
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
            roll.onClick.AddListener(ChangeRoll);
        }
       
        if(!GameObject.Find("ExperienceFill").TryGetComponent<Image>(out expFill))
        {
            Debug.Log("PlayerController - Awake - Image");
        }
        if (!transform.Find("AttackArea").TryGetComponent<AttackArea>(out attackArea))
        {
            Debug.Log("SkillManager - Awake - AttackArea");
        }
        if(!transform.Find("Charge").TryGetComponent<ParticleSystem>(out charge))
        {
            Debug.Log("PlayerController - Awake - ParticleSystem");
        }
        else
        {
            charge.Stop();
        }
        if (!transform.Find("ChargeLight").TryGetComponent<ParticleSystem>(out chargeLight))
        {
            Debug.Log("PlayerController - Awake - ParticleSystem");
        }
        else
        {
            chargeLight.Stop();
        }
    }

    public void Init(WeaponType type)
    {
        anim.Weapon((int)type);
        switch (type)
        {
            case WeaponType.Sowrd:
                meleeAttack.onClick.AddListener(SowrdAttack);
                if (!GameObject.Find("Sowrd").TryGetComponent<Weapon>(out weapon))
                {
                    Debug.Log("PlayerController - Awake - Weapon");
                }
                GameObject.Find("Hammer").SetActive(false);
                gunAttack.gameObject.SetActive(false);
                GameObject.Find("Gun").SetActive(false);
                break;
            case WeaponType.Hammer:
                Sprite hammerImage = Resources.Load<Sprite>("Image/Hammer");
                if(!meleeAttack.gameObject.TryGetComponent<Image>(out Image image))
                {
                    Debug.Log("PlayerController - Init - Resources Sprite");
                }
                else
                {
                    image.sprite = hammerImage;
                }
                meleeAttack.onClick.AddListener(HammerAttack);
                if (!GameObject.Find("Hammer").TryGetComponent<Weapon>(out weapon))
                {
                    Debug.Log("PlayerController - Awake - Weapon");
                }
                GameObject.Find("Sowrd").SetActive(false);
                gunAttack.gameObject.SetActive(false);
                GameObject.Find("Gun").SetActive(false);
                break;
            case WeaponType.Gun:
                meleeAttack.gameObject.SetActive(false);
                GameObject.Find("Sowrd").SetActive(false);
                GameObject.Find("Hammer").SetActive(false);
                if (!GameObject.Find("Gun").TryGetComponent<Weapon>(out weapon))
                {
                    Debug.Log("PlayerController - Awake - Weapon");
                }
                break;
        }
        weapon.Init(type);
        attackCount = 0;
        isInvincibility = false;
        currentHP = GameManager.Inst.PlayerInfo.Max_HP;
        currentEXP = 0;
        state = State.Idle;
        expFill.fillAmount = 0; 
        isControll = true;
        StartCoroutine(Idle());
    }

    public void ChangeState(State state)
    {
        if(this.state != state)
        {
            this.state = state;
            StopAllCoroutines();
            switch (this.state)
            {
                case State.Idle:
                    skillManager.UseSkill(-1);
                    StartCoroutine(Idle());
                    break;
                case State.MoveForward:
                    StartCoroutine(MoveForward()); 
                    break;
                case State.Attack_Soward:
                    break;
                case State.Attack_Hammer:
                    skillManager.UseSkill(0); 
                    weapon.OnTrail();
                    anim.Attack(true);
                    skillManager.SetCrowdControl(CrowdControl.Stun);
                    attackCount++;
                    break;
                case State.Attack_Skill:
                    roll.enabled = false;
                    weapon.OnTrail();
                    break;
                case State.Attack_Gun:
                    StartCoroutine(Attack_Gun());
                    break;
                case State.Roll:
                    break;
            }
        }
    }


    public State GetCurrentState()
    {
        return state;
    }

    public void UseSkill(int skill_id)
    {
        anim.Skill(skill_id);
    }

    private void GetDirection()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.z = Input.GetAxisRaw("Vertical");
        direction += Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        direction.Normalize();
    }

    private void GetLook()
    {
        look = Vector3.forward * gunAttack.Vertical + Vector3.right * gunAttack.Horizontal;
    }

    private void Move()
    {
        rig.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

    }

    public void SetIdle()
    {
        roll.enabled = true;
        anim.Skill(0);
        anim.IsCombo(false);
        attackArea.StopAttack();
        skillManager.SetCrowdControl(CrowdControl.None);
        weapon.OffTrail();
        ChangeState(State.Idle);
    }

    private IEnumerator Idle()
    {
        anim.Move(false);
        while (true)
        {
            if(weapon.Type == WeaponType.Gun)
            {
                GetLook();
                if(look != Vector3.zero)
                {
                    ChangeState(State.Attack_Gun);
                }
            }
            GetDirection();
            if (direction != Vector3.zero)
            {
                ChangeState(State.MoveForward);
            }
            yield return null;
        }
    }

    private IEnumerator MoveForward()
    {
        anim.Move(true);
        while (true)
        {
            if (weapon.Type == WeaponType.Gun)
            {
                GetLook();
                if (look != Vector3.zero)
                {
                    ChangeState(State.Attack_Gun);
                    break;
                }
            }
            GetDirection();
            if(direction != Vector3.zero)
            {
                Move();
                transform.LookAt(transform.position + direction);
            }
            else
            {
                ChangeState(State.Idle);
            }
            yield return null;
        }
    }

    private IEnumerator Attack_Gun()
    {
        anim.Move(true);
        anim.Combat(true);
        int count = 0;
        while (true)
        {
            GetLook();
            transform.LookAt(transform.position + look);
            if (look == Vector3.zero)
            {
                anim.Combat(false);
                ChangeState(State.MoveForward);
                break;
            }
            else
            {
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
            GetDirection();
            if (direction != Vector3.zero)
            {
                anim.Move(true);
                Move();
            }
            else
            {
                anim.Move(false);
            }

            if (count == 20)
            {
              //  weapon.NormalAttack();
                count = 0;
            }
            count++;
            yield return null;
        }
    }


    private void SowrdAttack()
    {
        ChangeState(State.Attack_Soward);
    }

    private void HammerAttack()
    {
        if (!anim.GetCombo() && anim.IsHammerAttack1())
        {
            anim.IsCombo(true);
        }
        ChangeState(State.Attack_Hammer);
    }

    private IEnumerator ComboCount()
    {
        yield return null;
        //anim.IsCombo(false);
    }
 
    private void ChangeRoll()
    {
        ChangeState(State.Roll);
        anim.Roll();
        StartCoroutine(Roll());
    }

    private IEnumerator Roll()
    {
        GetDirection();
        transform.LookAt(transform.position + direction);
        isInvincibility = true;
        for(int i = 0; i < 45; i++)
        {
            yield return null;
        }
        isInvincibility = false;
        ChangeState(State.Idle);
    }

    public void RollMove()
    {
        LeanTween.move(gameObject, transform.position + transform.forward * rollSpeed, 0.4f).setEase(LeanTweenType.easeOutSine);
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

    public void LookAttackArea()
    {
        transform.LookAt(attackArea.GetCenter() + transform.position); 
    }

    public void StopAnimator()
    {
        if (skillManager.IsCharge)
        {
            anim.PlayAnim(false);
        }
    }

    public void StartAnimator()
    {
        anim.PlayAnim(true);
    }

    public void ChargeEffect()
    {
        charge.Play(true);
    }

    public void StopCharge()
    {
        charge.Stop();
    }

    public void ChargeUp()
    {
        StartCoroutine(ChargeUpEffect());
    }

    private IEnumerator ChargeUpEffect()
    {
        chargeLight.Play();
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        chargeLight.Stop();
    }
    
    public void SetAttackArea(float radius)
    {
        attackArea.Attack(Vector3.zero, radius);
    }

    public void Aura()
    {
        effect = skillManager.SpawnEffect(4);
        effect.Init(transform.position + transform.forward *1.5f, 1);
    }

    public void Slash()
    {
        StopCharge();   
        effect = skillManager.SpawnEffect(3);
        effect.Init(transform.position, 1);
    }

    public void PowerWave()
    {
        effect = skillManager.SpawnEffect(7);
        effect.Init(transform.position + Vector3.up, 1);
        effect.SetRotation(transform.rotation);
        attackArea.AttackInAngle();
    }

    public void Pull()
    {
        skillManager.SetCrowdControl(CrowdControl.Pulled);
        effect = skillManager.SpawnEffect(5);
        effect.Init(transform.position, 1.5f);
        
    }

    public void Airborne()
    {
        skillManager.SetCrowdControl(CrowdControl.Airborne);
    }

    public void AttackField()
    {
        effect = skillManager.SpawnEffect(6);
        effect.Init(transform.position, 1f);
    }

    public void CrowdControlNone()
    {
        skillManager.SetCrowdControl(CrowdControl.None);
    }

    public void CalculateDamage(ITakeDamage hiter)
    {
        if (!isInvincibility)
        {
           // ApplyHP(hiter.TakeDamage(Physics_Cut, Fire_Cut, Water_Cut, Electric_Cut, Ice_Cut, Wind_Cut));
        }
    }

    public void Stun(int time)
    {
        StartCoroutine(StunControl(time));
    }

    private IEnumerator StunControl(int time)
    {
        isControll = false;
        yield return YieldInstructionCache.WaitForSeconds(time);
        isControll = true;
        
    }

    public void Airborne(int time)
    {
        
    }

    public void Knockback(int distance)
    {
        
    }
    public void Pulled(Vector3 center)
    {
        // todo: move to center
    }
}
