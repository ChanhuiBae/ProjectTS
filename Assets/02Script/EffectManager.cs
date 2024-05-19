using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class EffectManager : MonoBehaviour
{
    private SkillManager skillManager;
    private AttackArea attackArea;
    private Weapon weapon;
    private Transform point;

    private PostProcessLayer post;
    private FollowCamera cameraControl;

    private ParticleSystem charge1;
    private ParticleSystem charge2;
    private ParticleSystem chargeLight;
    private bool fullCharge;

    private Effect effect;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            if (!GameObject.Find("SkillManager").TryGetComponent<SkillManager>(out skillManager))
            {
                Debug.Log("EffectManager - Awake - SkillManager");
            }
            if(!Camera.main.TryGetComponent<PostProcessLayer>(out post))
            {
                Debug.Log("EffectManager - Awake - PostProcessLayer");
            }
            if(!Camera.main.TryGetComponent<FollowCamera>(out cameraControl))
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
    public void PinPointDown()
    {
        effect = skillManager.SpawnEffect(11);
        effect.Init(EffectType.Once, transform.position, 1.3f);
        effect.SetRotation(transform.rotation);
    }
    public void SetTrail()
    {
        effect = skillManager.SpawnEffect(4);
        effect.Init(EffectType.None, weapon.transform.position, 1);
        StartCoroutine(FollowWeaponPoint());
    }

    private IEnumerator FollowWeaponPoint()
    {
        int i = 0;
        while (i < 60)
        {
            yield return null;
            effect.transform.position = point.position;
            i++;
        }
    }

    public void NarakaEffect()
    {
        effect = skillManager.SpawnEffect(12);
        effect.Init(EffectType.None, point.position, 1);
        StartCoroutine(CameraShack());
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

    public void SetGray(bool gray)
    {

    }

    private IEnumerator CameraShack()
    {
        cameraControl.Shack = true;
        yield return YieldInstructionCache.WaitForSeconds(1);
        cameraControl.Shack = false;
    }

}
