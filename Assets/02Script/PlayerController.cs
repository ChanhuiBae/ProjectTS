using JetBrains.Annotations;
using System.Collections;
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
    private float moveSpeed;
    [SerializeField]
    private float rollSpeed;
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
    private GameObject grenade;

    private SkillManager skillManager;
    private AttackArea attackArea;

    private ParticleSystem charge1;
    private ParticleSystem charge2;
    private ParticleSystem chargeLight;
    private bool fullCharge;

    private Effect effect;

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
            if (!transform.Find("Charge1").TryGetComponent<ParticleSystem>(out charge1))
            {
                Debug.Log("PlayerController - Awake - ParticleSystem");
            }
            else
            {
                charge1.Stop();
            }
            if (!transform.Find("Charge2").TryGetComponent<ParticleSystem>(out charge2))
            {
                Debug.Log("PlayerController - Awake - ParticleSystem");
            }
            else
            {
                charge2.Stop();
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
            attackCount = 0;
            isInvincibility = false;
            currentHP = GameManager.Inst.PlayerInfo.Max_HP;
            currentEXP = 0;
            state = State.Idle;
            expFill.fillAmount = 0;
            fullCharge = false;
            isControll = true;
            StartCoroutine(Idle());
        }
        else
        {
            anim.Weapon(0);
            weapon = new Weapon();
            weapon.Init(WeaponType.None);
            isControll = true;
            StartCoroutine(Idle());
        }
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
                    if (SceneManager.GetActiveScene().buildIndex > 2)
                        skillManager.UseSkill(-1);
                    StartCoroutine(Idle());
                    break;
                case State.MoveForward:
                    StartCoroutine(MoveForward()); 
                    break;
                case State.Attack_Sword:
                    skillManager.UseSkill(0);
                    weapon.OnTrail();
                    anim.Attack(true);
                    skillManager.SetCrowdControl(CrowdControl.Stun);
                    attackCount++;
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
        if(skill_id == 302)
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
        skillManager.SetCrowdControl(CrowdControl.None);
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

            if (count == 60)
            {
                skillManager.SpawnBasicProjectile(weapon.transform.GetChild(1).transform.position);
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

            if (count == 60)
            {
                skillManager.SpawnAPHEProjectile(weapon.transform.GetChild(1).transform.position);
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
        anim.Roll();
        StartCoroutine(Roll());
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
        Vector3 look = skillManager.PopVector();
        if(look != Vector3.zero)
        {
            transform.LookAt(look +transform.position);
        }
        if (skillManager.GetVectorCount() == 0)
            anim.IsCombo(false);
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

    public void StopCharge()
    {
        charge1.Stop();
        charge2.Stop();
    }

    public void StartCharge()
    {
        fullCharge = false;
        charge1.Play();
    }

    public void ChargeUp(int count)
    {
        if(count == 1)
        {
            charge1.Stop();
            charge2.Play();
        }
        if (count == 2)
            fullCharge = true;
        StartCoroutine(ChargeUpEffect());
    }

    private IEnumerator ChargeUpEffect()
    {
        chargeLight.Play();
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        chargeLight.Stop();
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

    public void PinPointDown()
    {
        effect = skillManager.SpawnEffect(11);
        effect.Init(EffectType.Once, transform.position, 1.3f);
        effect.SetRotation(transform.rotation);
    }

    public void SetAttackArea(float radius)
    {
        attackArea.Attack(Vector3.zero, radius);
    }

    public void SetTrail()
    {
        effect = skillManager.SpawnEffect(4);
        effect.Init(EffectType.None, weapon.transform.position, 1);
        StartCoroutine(FollowWeapon());
    }

    private IEnumerator FollowWeapon()
    {
        int i = 0;
        while (i < 60)
        {
            yield return null;
            effect.transform.position = weapon.transform.position;
            i++;
        }
    }

    public void NarakaEffect()
    {
        effect = skillManager.SpawnEffect(12);
        effect.Init(EffectType.None, transform.position, 1);
    }

    public void SamsaraEffect()
    {
        effect = skillManager.SpawnEffect(13);
        effect.Init(EffectType.None, transform.position, 1);
    }

    public void SetSlashCycle()
    {
        StopCharge();  
        effect = skillManager.SpawnEffect(3);
        effect.Init(EffectType.None, transform.position, 1);
    }

    public void SetSlashForward()
    {
        skillManager.SpawnSlash(transform.position);
    }

    public void SetPowerWave()
    {
        if (fullCharge)
        {
            attackArea.SetTargets();
            effect = skillManager.SpawnEffect(7);
            effect.Init(EffectType.None, transform.position + Vector3.up, 1f);
            effect.SetRotation(transform.rotation);
            effect.Powerwave(0.5f);
            attackArea.AttackInAngle();
        }
    }
    public void CallDrone()
    {
        effect = skillManager.SpawnEffect(9);
        effect.Init(EffectType.Multiple, transform.position + Vector3.up, 600 * 0.017f);
        effect.SetRotation(transform.rotation);
        effect.Key = skillManager.GetCurrentKey();
        effect.StayCount(600, weapon.Attack_Speed);
    }

    public void SetPull()
    {
        skillManager.SetCrowdControl(CrowdControl.Pulled);
        skillManager.SetPulledPoint(transform.position);
        effect = skillManager.SpawnEffect(5);
        effect.Init(EffectType.None, transform.position, 4f);
    }

    public void SetAirborne()
    {
        skillManager.SetCrowdControl(CrowdControl.Airborne);
    }

    public void SetKnockback()
    {
        skillManager.SetCrowdControl(CrowdControl.Knockback);
    }

    public void SetAttackField()
    {
        effect = skillManager.SpawnEffect(6);
        effect.Init(EffectType.None, transform.position, 1f);
    }


    public void Push()
    {
        skillManager.SetCrowdControl(CrowdControl.Pulled);
        skillManager.SetPulledPoint(transform.forward * 12f + transform.position);
        LeanTween.move(gameObject, transform.forward * 10f + transform.position, 1f);
    }

    public void CrowdControlNone()
    {
        skillManager.SetCrowdControl(CrowdControl.None);
    }

    public void CalculateDamage(AttackType attack, ITakeDamage hiter)
    {
        if (!isInvincibility)
        {
           // ApplyHP(hiter.TakeDamage(Physics_Cut, Fire_Cut, Water_Cut, Electric_Cut, Ice_Cut, Wind_Cut));
        }
    }

    public void CalculateDamageByKey(AttackType attack, int key, ITakeDamage hiter)
    {
        if (!isInvincibility)
        {

        }
    }
    public void Stagger(float time)
    {

    }

    public void Stun(float time)
    {
        StartCoroutine(StunControl(time));
    }

    private IEnumerator StunControl(float time)
    {
        isControll = false;
        yield return YieldInstructionCache.WaitForSeconds(time);
        isControll = true;
        
    }

    public void Airborne(float time)
    {
        
    }

    public void Knockback(float distance)
    {
        
    }
    public void Pulled(Vector3 center)
    {
        // todo: move to center
    }

    public void Airback(float time, float distance)
    {

    }
}
