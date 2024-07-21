using HighlightPlus;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum State
{
    Idle,
    MoveForward,
    Attack_Sword,
    Attack_Hammer,
    Attack_Gun,
    Attack_Skill,
    CrowdControl,
    Die
}

public class PlayerController : MonoBehaviour, IDamage
{
    private bool rollvalue;

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
    private HighlightEffect inner;

    private Button roll;
    private Image rollBlock;
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
    private int sceneNum;

    private float currentHP;
    private float MaxHP;
    public float MAXHP
    {
        get => MaxHP;
    }
    private Image hpFill;
    private float MaxHPpassive;

    public void MaxHP_Passive(float value)
    {
        MaxHPpassive = value;
        MaxHP = GameManager.Inst.PlayerInfo.Max_HP + (GameManager.Inst.PlayerInfo.Max_HP * MaxHPpassive);
    }

    private float currentEXP;
    private float maxEXP;
    private Image expFill;
    private int level;

    private Vector3 direction;
    private Vector3 look;
    private GameObject grenade;

    private SkillManager skillManager;
    private int skillID;
    private AttackArea attackArea;

    private Effect stun;

    private GameObject basePlayer;
    private SkinnedMeshRenderer dissolve;

    private void Awake()
    {
        sceneNum = SceneManager.GetActiveScene().buildIndex;
        if (sceneNum > 2)
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
                if(!roll.transform.Find("block").TryGetComponent<Image>(out rollBlock))
                {
                    Debug.Log("PlayerController - Awake - Image");
                }
                else
                {
                    rollBlock.enabled = false;
                }
            }
            if (!GameObject.Find("ExperienceFill").TryGetComponent<Image>(out expFill))
            {
                Debug.Log("PlayerController - Awake - Image");
            }
            if (!GameObject.Find("HPFill").TryGetComponent<Image>(out hpFill))
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
            if(!TryGetComponent<HighlightEffect>(out inner))
            {
                Debug.Log("PlayerController - Awake - HighlightEffect");
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
        basePlayer = transform.GetChild(0).Find("Base").gameObject;
        dissolve = transform.GetChild(0).Find("Dissolve").GetComponent<SkinnedMeshRenderer>();

        if (!TryGetComponent<PlayerAnimationController>(out anim))
        {
            Debug.Log("PlayerController - Awake - PlayerAnimationController");
        }
    }

    public void Init(WeaponType type)
    {
        skillID = 0;
        rollvalue = false;
        if (sceneNum > 2 && type != WeaponType.None)
        {
            anim.Weapon((int)type);
            inner.innerGlow = 0;
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
            isInvincibility = false;
            MaxHPpassive = 0;
            MaxHP = GameManager.Inst.PlayerInfo.Max_HP;
            level = 1;
            currentHP = GameManager.Inst.PlayerInfo.Max_HP;
            currentEXP = 0;
            SetMaxEXP();
            expFill.fillAmount = 0;
            hpFill.fillAmount = 1;

            rollvalue = false;
        }
        else
        {
            anim.Weapon(0);
        }
        
        dissolve.enabled = false;
        state = State.Idle;
        isDie = false;
        isControll = true;
        StartCoroutine(Idle());
    }

    public bool ChangeState(State state)
    {
        if(this.state != state && this.state != State.Die && !rollvalue)
        {
            if(state == State.Idle)
            {
                this.state = state;
                StopAllCoroutines();
                if (sceneNum > 2)
                {
                    skillManager.UseSkill(-1);
                    roll.enabled = true;
                    attackArea.StopAttack();
                    skillManager.SetCrowdControl(CrowdControlType.None);
                    skillManager.ClearVector();
                    weapon.OffTrail();
                }
                isInvincibility = false;
                anim.Skill(0);
                anim.IsCombo(false);
                anim.Move(false);
                StartCoroutine(Idle());
                return true;
            }
            else if(this.state == State.CrowdControl)
            {
                return false;
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
                        break;
                    case State.Attack_Hammer:
                        weapon.OnTrail();
                        anim.IsCombo(false);
                        anim.Attack(true);
                        skillManager.SetCrowdControl(CrowdControlType.Stun);
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
                        isControll = false;
                        StartCoroutine(Dissolve());
                        break;
                }
                return true;
            }
        }
        else if(state == State.Die)
        {
            this.state = state;
            isControll = false;
            StartCoroutine(Dissolve());
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetMaxEXP()
    {
        if(level < 7)
        {
            maxEXP = 10 + 12*(level - 1);
        }
        else
        {
            maxEXP = 150 + 100 * (level - 7);
        }
    }

    public State GetCurrentState()
    {
        return state;
    }

    private IEnumerator Dissolve()
    {
        basePlayer.SetActive(false);
        dissolve.enabled = true;
        weapon.gameObject.SetActive(false);
        float speed = 5;
        float t = 0;
       
        float value = 0;
        for(t = 0; t < 1; t += 0.017f)
        {
            Material mat = dissolve.material;
            mat.SetFloat("_Cutoff", t);
            dissolve.material = mat;
            yield return null;
        }
        GameManager.Inst.menuManager.SetReward(false);
    }

    public void UseSkill(int skill_id)
    {
        anim.Skill(skill_id);
        skillID = skill_id;
        if (skill_id == 302)
        {
            StopAllCoroutines();
            StartCoroutine(APHE_Shoot());
        }
    }

    private void GetDirection()
    {
        if(state != State.Attack_Skill || skillID == 302)
        {
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.z = Input.GetAxisRaw("Vertical");
            direction += Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            direction.Normalize();
        }
    }


    private void Move()
    {
        if (!rollvalue)
        {
            anim.Move(true);
            rig.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        }
    }

    public void SetIdle()
    {
        ChangeState(State.Idle);
    }

    private IEnumerator Idle()
    {
        anim.Move(false);
        while (true)
        {
            if(sceneNum > 2 && weapon.Type == WeaponType.Gun)
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
        while (true)
        {
            if (sceneNum > 2 && weapon.Type == WeaponType.Gun)
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
                if(!rollvalue)
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
        if (state != State.Attack_Hammer)
        {
            ChangeState(State.Attack_Hammer);
        }
        else if (!anim.GetCombo() && anim.IsHammerAttack1())
        {
            anim.IsCombo(true);
            weapon.OnTrail();
        }
    }

    public void GunAttack()
    {
        ChangeState(State.Attack_Gun);
    }
    
    public void CounterCheck()
    {
        skillManager.CounterCheck();
    }


    private void ChangeRoll()
    {
        if (this.state != State.Attack_Skill && !rollvalue)
        {
            StartCoroutine(Roll());
        }
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
        if (!anim.isRoll())
        {
            GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Slide);
            rollBlock.enabled = true;
            roll.enabled = false;
            transform.LookAt(transform.position + direction);
            rollvalue = true;
            anim.Roll();
            for (int i = 0; i < 3; i++)
            {
                rig.MovePosition(transform.position + transform.forward * moveSpeed * 1.2f * Time.deltaTime);
                yield return null;
            }
            isInvincibility = true;
            for (int i = 0; i < 15; i++)
            {
                rig.MovePosition(transform.position + transform.forward * moveSpeed * 1.5f * Time.deltaTime);
                yield return null;
            }
            isInvincibility = false;
            for (int i = 0; i < 21; i++)
            {
                rig.MovePosition(transform.position + transform.forward * moveSpeed * 1.3f * Time.deltaTime);
                yield return null;
            }
            rollvalue = false;
            GetDirection();
            if (state == State.Idle && direction != Vector3.zero)
            {
                StopAllCoroutines();
                ChangeState(State.MoveForward);
                anim.Move(true);
            }
            else if (state == State.MoveForward && direction == Vector3.zero)
            {
                StopAllCoroutines();
                ChangeState(State.Idle);
                anim.Move(false);
            }
            else if(state == State.Attack_Hammer || state == State.Attack_Gun || state == State.Attack_Sword)
            {
                StopAllCoroutines();
                ChangeState(State.Idle);
                anim.Move(false);
            }
            rollBlock.enabled = false;
            roll.enabled = true;
        }
    }

    public void RollMove()
    {
        rollvalue = true;
    }


    public void MoveBack()
    {
        isInvincibility = true;
        LeanTween.move(gameObject, transform.position - transform.forward * rollSpeed, 0.4f).setEase(LeanTweenType.easeOutSine);
    }

    public void ApplyHP(float value)
    {
        Debug.Log("hp" + value);
        currentHP -= value;
        if(currentHP < 0)
        {
            currentHP = 0;
        }
        else if(currentHP > MaxHP)
        {
            currentHP = MaxHP;
        }

        SetHPUI();
    }

    public void GetEXP(float value)
    {
        currentEXP += value;
        if(currentEXP > maxEXP)
        {
            level++;
            SetMaxEXP();
            currentEXP = 0;
            GameManager.Inst.menuManager.SetLevelUpPopup(level);
        }
        expFill.fillAmount = currentEXP / maxEXP;
    }

    public void LookAttackArea()
    {
        transform.LookAt(attackArea.GetCenter() + transform.position);
    }

    public void LookAtVector()
    {
        if(skillManager.GetVectorCount() != 0)
        {
            Vector3 look = skillManager.PopVector();
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

    public void SetAttackAreaStop()
    {
        attackArea.StopAttack();
    }

    public bool IsAttack()
    {
        if(state == State.Attack_Skill || state == State.Attack_Sword || state == State.Attack_Hammer || state == State.Attack_Gun)
        {
            return true;
        }
        return false;
    }

    public bool CalculateDamage(AttackType attack, ITakeDamage hiter)
    {
        return false;
    }

    public bool CalculateDamage(AttackType attack, int key, ITakeDamage hiter)
    {
        return false;
    }

    public bool CalulateDamage(int creatueKey, int patternKey, ITakeDamage hiter)
    {
        if (!isInvincibility && !isDie)
        {
            float damage = hiter.TakeDamage(creatueKey, patternKey);
            Debug.Log(damage);
            if(damage > 0)
            {
                StartCoroutine(HitGlow());
                ApplyHP(damage);
                if (currentHP <= 0)
                {
                    isDie = true;
                    ChangeState(State.Die);
                }
                return true;
            }
        }
        return false;
    }

    private IEnumerator HitGlow()
    {
        inner.innerGlow = 1.4f;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        inner.innerGlow = 0f;
    }

    private void SetHPUI()
    {
        hpFill.fillAmount = currentHP / MaxHP;
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
        anim.SetKnockBack(true);
        yield return new WaitForSeconds(time);
        anim.SetKnockBack(false);
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
        anim.SetKnockDown(true);
        isInvincibility = true;
        Vector3 pos = transform.position;
        LeanTween.move(gameObject, pos + (Vector3.up * 3f), time / 2).setEase(LeanTweenType.easeOutCubic);
        yield return YieldInstructionCache.WaitForSeconds(time / 2);
        LeanTween.move(gameObject, pos, time / 2).setEase(LeanTweenType.easeInSine);
        yield return YieldInstructionCache.WaitForSeconds(time / 2);
        anim.SetKnockDown(false);
        isInvincibility = false;
        ChangeState(State.Idle);
        isControll = true;
    }

    public void Knockback(float distance)
    {
        if (isDie)
        {
            ChangeState(State.CrowdControl);
            isControll = false;
            anim.SetKnockBack(true);
            StartCoroutine(MoveKnockback(distance));
        }
    }

    private IEnumerator MoveKnockback(float distance)
    {
        LeanTween.move(gameObject, transform.position + Vector3.up - transform.forward * distance, distance * 0.01f).setEase(LeanTweenType.easeOutQuart);
        yield return YieldInstructionCache.WaitForSeconds(distance * 0.01f);
        anim.SetKnockBack(false);
        ChangeState(State.Idle);
    }

    public void Pulled(Vector3 center)
    {
        
    }

    public void Airback(float time, float distance)
    {

    }

}
