using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour
{
    private SkillManager skillManager;
    private AttackArea attackArea;
    private Weapon weapon;
    private Transform point;

    private PostProcessVolume post;
    private ColorGrading postColor;

    private FollowCamera cameraControl;

    private ParticleSystem charge1;
    private ParticleSystem charge2;
    private ParticleSystem chargeLight;
    private bool fullCharge;

    private Effect effect;
    List<Effect> effects = new List<Effect>();

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
            {
                Debug.Log("EffectManager - Awake - SkillManager");
            }
            if (!GameObject.Find("Post").TryGetComponent<PostProcessVolume>(out post))
            {
                Debug.Log("Effect - Awake - PostProcessVolume");
            }
            if (!post.profile.TryGetSettings<ColorGrading>(out postColor))
            {
                Debug.Log("Effect - Awake - ColorGranding");
            }
            if (!Camera.main.TryGetComponent<FollowCamera>(out cameraControl))
            {
                Debug.Log("EffectManager - Awake - FollowCamera");
            }
        }
        if (!transform.Find("Charge1").TryGetComponent<ParticleSystem>(out charge1))
        {
            Debug.Log("EffectManager - Awake - ParticleSystem");
        }
        else
        {
            charge1.Stop();
        }
        if (!transform.Find("Charge2").TryGetComponent<ParticleSystem>(out charge2))
        {
            Debug.Log("EffectManager - Awake - ParticleSystem");
        }
        else
        {
            charge2.Stop();
        }
        if (!transform.Find("ChargeLight").TryGetComponent<ParticleSystem>(out chargeLight))
        {
            Debug.Log("EffectManager - Awake - ParticleSystem");
        }
        else
        {
            chargeLight.Stop();
        }
        if (!transform.Find("AttackArea").TryGetComponent<AttackArea>(out attackArea))
        {
            Debug.Log("EffectManager - Awake - AttackArea");
        }
    }

    public void Init(Weapon weapon)
    {
        this.weapon = weapon;
        if(weapon.Type == WeaponType.Hammer)
        {
            point = weapon.transform.Find("HammerHead").transform;
        }
        else if(weapon.Type == WeaponType.Gun)
        {
            point = weapon.transform.Find("Muzzle").transform;
        }
        fullCharge = false;
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
        if (count == 1)
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
    public void PinPointDown(Vector3 position)
    {
        effect = skillManager.SpawnEffect(16);
        effect.Init(EffectType.None, point.position, 1f);
        effect.SetRotation(point.rotation);

        effect = skillManager.SpawnEffect(11);
        effect.Init(EffectType.Once, position, 3f);
        effect.SetRotation(Quaternion.identity);
    }
    public void SetTrail()
    {
        effect = skillManager.SpawnEffect(4);
        effect.InitFollow(EffectType.None, point.gameObject, 1f);
    }

    public void NarakaEffect()
    {
        effect = skillManager.SpawnEffect(12);
        effect.Init(EffectType.None, point.position, 1);
        StartCoroutine(CameraShack());

        effect = skillManager.SpawnEffect(15);
        effect.Init(EffectType.None, point.position+ new Vector3(0,0.5f,0), 12f);
    }

    public void NarakaPlayerEffect()
    {
        float time = 1.5f;
        effect = skillManager.SpawnEffect(17);
        effect.InitFollow(EffectType.None, gameObject, time);
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
            effect.Init(EffectType.None, transform.position + Vector3.up, 2f);
            effect.SetRotation(transform.rotation);
            effect.Powerwave(0.5f, 8f);
            attackArea.AttackInAngle();
        }
    }
    public void SpawnDrone()
    {
        //old version
        effect = skillManager.SpawnEffect(9);
        effect.Init(EffectType.Multiple, transform.position + Vector3.up, 600 * 0.017f);
        effect.SetRotation(transform.rotation);
        effect.Key = skillManager.GetCurrentKey();
        effect.StayCount(600, weapon.Attack_Speed);
    }


    public void TC_Effect()
    {
        effect = skillManager.SpawnEffect(14);
        effect.Init(EffectType.None, transform.position, 1.2f);
    }

    public void SetPull()
    {
        skillManager.SetCrowdControl(CrowdControlType.Pulled);
        skillManager.SetPulledPoint(transform.position);
        effect = skillManager.SpawnEffect(5);
        effect.Init(EffectType.None, transform.position, 4f);
    }

    public void SetAttackField()
    {
        effect = skillManager.SpawnEffect(6);
        effect.Init(EffectType.None, transform.position, 1f);
    }

    public void SetBoxArea()
    { 
        effects.Clear();
        for(int i = 0; i < 5; i++)
        {
            effect = skillManager.SpawnEffect(19);
            effect.InitNotTime(EffectType.None, transform.position + transform.forward * (i+1));
            effect.SetRotation(transform.rotation);
            effects.Add(effect);
        }
    }

    public void MoveBoxes(Vector3 dirction)
    {
        dirction /= 18;
        for(int i = 0;i < 5;i++)
        {
            effects[i].transform.position = transform.position + dirction * i + transform.forward;
            effects[i].SetRotation(transform.rotation);
        }
    }

    public void DropMissile()
    {
        StartCoroutine(StartDrop());
    }

    private IEnumerator StartDrop()
    {
        effect = skillManager.SpawnEffect(22);
        effect.Init(EffectType.None, transform.position - transform.forward * 50 + Vector3.up*10, 5f);
        effect.SetRotation(transform.rotation);
        effect.MoveForward();
        yield return YieldInstructionCache.WaitForSeconds(1f);

        for (int i = 0; i < 5; i++)
        {
            effect = skillManager.SpawnEffect(20);
            effect.Init(EffectType.None, effects[i].transform.position + Vector3.up, 1f);
            effect.SetRotation(effects[i].transform.rotation);
            yield return YieldInstructionCache.WaitForSeconds(0.5f);

            effect = skillManager.SpawnEffect(21);
            effect.Init(EffectType.Once, effects[i].transform.position, 1f);
            effect.SetRotation(effects[i].transform.rotation);
            effects[i].ReturenEffect();
            yield return YieldInstructionCache.WaitForSeconds(1f);
        }
        effects.Clear();
        PlayerController player = transform.GetComponent<PlayerController>();
        player.SetIdle();
    }

    public void SetColorInversion(bool use)
    {
        if (use)
        {
            postColor.gradingMode.Override(GradingMode.LowDefinitionRange);
        }
        else
        {
            postColor.gradingMode.Override(GradingMode.HighDefinitionRange);
        }
    }

    public void SetGray(bool gray)
    {
        if (gray)
        {
            postColor.saturation.value = -100f;
        }
        else
        {
            postColor.saturation.value = 0f;
        }

    }

    private IEnumerator CameraShack()
    {
        cameraControl.Shack = true;
        yield return YieldInstructionCache.WaitForSeconds(1);
        cameraControl.Shack = false;
    }

    private IEnumerator SpawnDent(Vector3 position)
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
    }

}
