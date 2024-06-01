using JetBrains.Annotations;
using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public enum State
{
    Idle,
    MoveForward,
    Attack_Sword,
    Attack_Hammer,
    Attack_Gun,
    Attack_Skill,
    Roll,
    CrowdControl,
    Die
}

public class PlayerController : MonoBehaviour, IDamage
{
    private Rigidbody rig;
    private FloatingJoystick joystick;
    private WeaponType weaponType;
    private CrowdControlType CCType;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rollSpeed;
    private PlayerAnimationController anim;
    private EffectManager effect;
    private Button roll;
    private bool isControll = true;
    public bool CONTROLL
    {
        set => isControll = value;
        get => isControll;
    }
    private bool isInvincibility;
    private bool isDie;

    private State state;
    private Weapon weapon;
    private Armor armor;
    private int attackCount;

    private float currentHP;
    private float currentEXP;
    private Image expFill;

    private Vector3 direction;
    private Vector3 look;
    private GameObject grenade;

    private SkillManager skillManager;
    private AttackArea attackArea;

    private Effect stun;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().buildIndex > 2)
        {
            if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
            {
                Debug.Log("PlayerController - Awake - SkillManager");
            }
            grenade = GameObject.Find("Grenade");

            if (!GameObject.Find("Roll").TryGetComponent<Button>(out roll))
            {
                Debug.Log("PlayerController - Awake - Button");
            }
            else
            {
                roll.onClick.AddListener(ChangeRoll);
            }

            if (!GameObject.Find("ExperienceFill").TryGetComponent<Image>(out expFill))
            {
                Debug.Log("PlayerController - Awake - Image");
            }
            if (!transform.Find("AttackArea").TryGetComponent<AttackArea>(out attackArea))
            {
                Debug.Log("PlayerController - Awake - AttackArea");
            }
            if (!TryGetComponent<EffectManager>(out effect))
            {
                Debug.Log("PlayerController - Awake - EffectManager");
            }

        }
        if (!TryGetComponent<Rigidbody>(out rig))
        {
            Debug.Log("PlayerController - Awake - Rigidbody");
        }
        if (!GameObject.Find("Floating Joystick").TryGetComponent<FloatingJoystick>(out joystick))
        {
            Debug.Log("PlayerController - Awake - Floating Joystick");
        }

        if (!TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("PlayerController - Awake - PlayerAnimationController");
        }

        if(!TryGetComponent<Armor>(out armor))
        {
            Debug.Log("PlayerController - Awake - Armor");
        }
    }

    public void Init(WeaponType type)
    {
        if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            anim.Weapon((int)type);

            switch (type)
            {
                case WeaponType.Sowrd:
                    if (!GameObject.Find("Sowrd").TryGetComponent<Weapon>(out weapon))
                    {
                        Debug.Log("PlayerController - Awake - Weapon");
                    }
                    GameObject.Find("Hammer").SetActive(false);
                    GameObject.Find("Gun").SetActive(false);
                    break;
                case WeaponType.Hammer:
                    if (!GameObject.Find("Hammer").TryGetComponent<Weapon>(out weapon))
                    {
                        Debug.Log("PlayerController - Awake - Weapon");
                    }
                    GameObject.Find("Sowrd").SetActive(false);
                    GameObject.Find("Gun").SetActive(false);
                    break;
                case WeaponType.Gun:
                    GameObject.Find("Sowrd").SetActive(false);
                    GameObject.Find("Hammer").SetActive(false);
                    if (!GameObject.Find("Gun").TryGetComponent<Weapon>(out weapon))
                    {
                        Debug.Log("PlayerController - Awake - Weapon");
                    }
                    break;
            }
            weapon.Init(type);
            effect.Init(weapon);
            CCType = CrowdControlType.None;
            attackCount = 0;
            isInvincibility = false;
            currentHP = GameManager.Inst.PlayerInfo.Max_HP;
            currentEXP = 0;
            state = State.Idle;
            expFill.fillAmount = 0;
            isDie = false;
            isControll = true;
            StartCoroutine(Idle());
        }
        else
        {
            anim.Weapon(0);
            weapon = new Weapon();
            weapon.Init(WeaponType.None);
            CCType = CrowdControlType.None;
            isDie = false;
            isControll = true;
            StartCoroutine(Idle());
        }
    }

    public bool ChangeState(State state)
    {
        if(this.state != state && this.state != State.Die)
        {
            if(state == State.Idle)
            {
                this.state = state;
                StopAllCoroutines();
                if (SceneManager.GetActiveScene().buildIndex > 2)
                    skillManager.UseSkill(-1);
                StartCoroutine(Idle());
                return true;
            }
            else if(this.state == State.CrowdControl)
            {
                return false;
            }
            else if(state == State.Roll)
            {
                if(this.state == State.Attack_Skill)
                {
                    return false;
                }
                else
                {
                    this.state = state;
                    StopAllCoroutines();
                    anim.Roll();
                    StartCoroutine(Roll());
                    return true;
                }
            }
            else
            {
                this.state = state;
                StopAllCoroutines();
                switch (this.state)
                {
                    case State.MoveForward:
                        StartCoroutine(MoveForward());
                        break;
                    case State.Attack_Sword:
                        skillManager.UseSkill(0);
                        weapon.OnTrail();
                        anim.Attack(true);
                        skillManager.SetCrowdControl(CrowdControlType.Stun);
                        attackCount++;
                        break;
                    case State.Attack_Hammer:
                        skillManager.UseSkill(0);
                        weapon.OnTrail();
                        anim.Attack(true);
                        skillManager.SetCrowdControl(CrowdControlType.Stun);
                        attackCount++;
                        break;
                    case State.Attack_Gun:
                        StartCoroutine(Attack_Gun());
                        break;
                    case State.Attack_Skill:
                        roll.enabled = false;
                        weapon.OnTrail();
                        break;
                    case State.CrowdControl:
                        break;
                    case State.Die:
                        break;
                }
                return true;
            }
        }
        else
        {
            return false;
        }
    }


    public State GetCurrentState()
    {
        return state;
    }

    public void UseSkill(int skill_id)
    {
        anim.Skill(skill_id);
        if (skill_id == 302)
        {
            StopAllCoroutines();
            StartCoroutine(APHE_Shoot());
        }
    }

    private void GetDirection()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.z = Input.GetAxisRaw("Vertical");
        direction += Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        direction.Normalize();
    }


    private void Move()
    {
        rig.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    public void SetIdle()
    {
        isInvincibility = false;
        roll.enabled = true;
        anim.Skill(0);
        anim.IsCombo(false);
        attackArea.StopAttack();
        skillManager.SetCrowdControl(CrowdControlType.None);
        skillManager.ClearVector();
        weapon.OffTrail(); 
        StopAllCoroutines();
        ChangeState(State.Idle);
    }

    private IEnumerator Idle()
    {
        anim.Move(false);
        while (true)
        {
            if(weapon.Type == WeaponType.Gun)
            {
                look = skillManager.GetLook();
                if (look != Vector3.zero)
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
                look = skillManager.GetLook();
                if (look != Vector3.zero)
                {
                    ChangeState(State.Attack_Gun);
                    break;
                }
            }
            GetDirection();
            if(direction != Vector3.zero && anim.CanMove())
            {
                Move();
                transform.LookAt(transform.position + direction);
            }
            else if(state != State.Attack_Skill && direction == Vector3.zero)
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
        float hit_count = 60 / weapon.Attack_Speed;
        while (true)
        {
            look = skillManager.GetLook();
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

            if (count >= hit_count)
            {
                skillManager.SpawnBasicProjectile(weapon.transform.GetChild(0).transform.position);
                skillManager.UseSkill(0);
                count = 0;
            }
            count++;
            yield return null;
        }
    }

    private IEnumerator APHE_Shoot()
    {
        anim.Move(true);
        anim.Combat(true);
        int count = 60;
        float hit_count = 30 / weapon.Attack_Speed;
        int total = 0;
        while (true)
        {
            look = skillManager.GetLook();
            transform.LookAt(transform.position + look);
            if (look == Vector3.zero)
            {
                anim.Combat(false);
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

            if (count >= hit_count)
            {
                skillManager.SpawnAPHEProjectile(weapon.transform.GetChild(0).transform.position);
                count = 0;
            }
            if (total >= 300)
            {
                break;
            }
            count++;
            total++;
            yield return null;
        }
        SetIdle();
    }


    public void SwordAttack()
    {
        if (!anim.GetCombo() && anim.IsHammerAttack1())
        {
            anim.IsCombo(true);
        }
        else if(anim.GetCombo() && !anim.IsHammerAttack1())
        {
            anim.IsCombo(false);
        }
        ChangeState(State.Attack_Sword);
    }

    public void HammerAttack()
    {
        if (!anim.GetCombo() && anim.IsHammerAttack1())
        {
            anim.IsCombo(true);
        }
        else if (anim.GetCombo() && !anim.IsHammerAttack1())
        {
            anim.IsCombo(false);
        }
        ChangeState(State.Attack_Hammer);
    }

    public void GunAttack()
    {
        ChangeState(State.Attack_Gun);
    }

    private void ChangeRoll()
    {
        ChangeState(State.Roll);
    }

    public void IsInvincibility(float time)
    {
        StartCoroutine(InvincibilityTime(time));
    }

    private IEnumerator InvincibilityTime(float time)
    {
        isInvincibility = true;
        yield return YieldInstructionCache.WaitForSeconds(time);
        isInvincibility = false;
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

    public void MoveBack()
    {
        isInvincibility = true;
        LeanTween.move(gameObject, transform.position - transform.forward * rollSpeed, 0.4f).setEase(LeanTweenType.easeOutSine);
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

    public void LookAtVector()
    {
        Debug.Log(skillManager.GetVectorCount());
        if(skillManager.GetVectorCount() != 0)
        {
            Vector3 look = skillManager.PopVector();
            Debug.Log(look);
            transform.LookAt(look + transform.position);
            skillManager.PinPointDown(look / 4 + transform.position);
            if (skillManager.GetVectorCount() == 0)
            {
                anim.IsCombo(false);
            }
        }
    }

    public void MoveAttackArea()
    {
        LeanTween.move(gameObject, attackArea.GetCenter() + transform.position, 0.4f);
    }

    public void Sit()
    {
        anim.Sit();
    }

    public void IsCombo(bool value)
    {
        anim.IsCombo(value);
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


    
    public void AttackGrenade()
    {
        skillManager.SpawnGrenade(grenade.transform.position, transform.rotation);

        Quaternion quaternion = Quaternion.Euler(new Vector3(0, 180, 0));
        quaternion = transform.rotation * quaternion;
        skillManager.SpawnGrenade(grenade.transform.position, quaternion);

        quaternion = Quaternion.Euler(new Vector3(0, 30, 0));
        quaternion = transform.rotation * quaternion;
        skillManager.SpawnGrenade(grenade.transform.position,quaternion);

        quaternion = Quaternion.Euler(new Vector3(0, -30, 0));
        quaternion = transform.rotation * quaternion;
        skillManager.SpawnGrenade(grenade.transform.position, quaternion);
    }

    public void SetAttackArea(float radius)
    {
        attackArea.Attack(Vector3.zero, radius);
    }

    

    public void CalculateDamage(AttackType attack, ITakeDamage hiter)
    {
        if (!isInvincibility && !isDie)
        {
            float damage = hiter.TakeDamage();
            currentHP -= damage;
            Debug.Log("Take Damage: " + damage);

            if(currentHP <= 0)
            {
                isDie = true;
                ChangeState(State.Die);
            }
        }
    }

    public void CalculateDamage(AttackType attack, int key, ITakeDamage hiter)
    {
        if (!isInvincibility && !isDie)
        {

        }
    }
    public void Stagger(float time)
    {
        if (!isDie)
        {
            ChangeState(State.CrowdControl);
            isControll = false;
            StartCoroutine(StaggerTime(time));
        }
    }

    private IEnumerator StaggerTime(float time)
    {
        // todo: stagger animation;
        yield return new WaitForSeconds(time);
        ChangeState(State.Idle); 
        isControll = true;
    }

    public void Stun(float time)
    {
        if (!isDie)
        {
            ChangeState(State.CrowdControl);
            isControll = false;
            StartCoroutine(StunControl(time));
        }
    }

    private IEnumerator StunControl(float time)
    {
        stun.gameObject.SetActive(true);
        for (float i = 0.1f; i < time; i += 0.1f)
        {
            LeanTween.move(gameObject, transform.position - transform.forward * 0.1f, 0.1f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            LeanTween.move(gameObject, transform.position + transform.forward * 0.1f, 0.1f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        stun.gameObject.SetActive(false);
        ChangeState(State.Idle);
        isControll = true;
        
    }

    public void Airborne(float time)
    {
        if (isDie)
        {
            ChangeState(State.CrowdControl);
            isControll = false;
            StartCoroutine(MoveAirborne(time));
        }
    }
    private IEnumerator MoveAirborne(float time)
    {
        Vector3 pos = transform.position;
        LeanTween.move(gameObject, pos + (Vector3.up * 3f), time / 2).setEase(LeanTweenType.easeOutCubic);
        yield return YieldInstructionCache.WaitForSeconds(time / 2);
        LeanTween.move(gameObject, pos, time / 2).setEase(LeanTweenType.easeInSine);
        yield return YieldInstructionCache.WaitForSeconds(time / 2);
        ChangeState(State.Idle);
        isControll = true;
    }

    public void Knockback(float distance)
    {
        if (isDie)
        {
            ChangeState(State.CrowdControl);
            isControll = false;
            StartCoroutine(MoveKnockback(distance));
        }
    }

    private IEnumerator MoveKnockback(float distance)
    {
        LeanTween.move(gameObject, transform.position + Vector3.up - transform.forward * distance, distance * 0.01f).setEase(LeanTweenType.easeOutQuart);
        yield return YieldInstructionCache.WaitForSeconds(distance * 0.01f);
        ChangeState(State.Idle);
    }

    public void Pulled(Vector3 center)
    {
        
    }

    public void Airback(float time, float distance)
    {

    }
}
