using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using Unity.VisualScripting;

public class EffectManager : MonoBehaviour
{
    private SkillManager skillManager;
    PlayerController player;
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

    private Image fadeImg;
    private bool Dimension_Distortion_Trigger;
    private int frame;


    private Effect effect;
    List<Effect> effects = new List<Effect>();

    private void Awake()
    {
        Dimension_Distortion_Trigger = false;
        frame = 0;
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
            if(!GameObject.Find("White").TryGetComponent<Image>(out fadeImg))
            {
                Debug.Log("EffectManager - Awake - Image");
            }
            else
            {
                fadeImg.enabled = false;
            }
            player = transform.GetComponent<PlayerController>();
        }
        Transform trans = transform.Find("Charge1");
        if (trans != null && !trans.TryGetComponent<ParticleSystem>(out charge1))
        {
            Debug.Log("EffectManager - Awake - ParticleSystem");
        }
        else if(charge1 != null)
        {
            charge1.Stop();
        }
        trans = transform.Find("Charge2");
        if (trans != null && !trans.TryGetComponent<ParticleSystem>(out charge2))
        {
            Debug.Log("EffectManager - Awake - ParticleSystem");
        }
        else if (charge2 != null)
        {
            charge2.Stop();
        }
        trans = transform.Find("ChargeLight");
        if (trans != null && !trans.TryGetComponent<ParticleSystem>(out chargeLight))
        {
            Debug.Log("EffectManager - Awake - ParticleSystem");
        }
        else if(chargeLight != null)
        {
            chargeLight.Stop();
        }
        trans = transform.Find("AttackArea");
        if (trans != null && !trans.TryGetComponent<AttackArea>(out attackArea))
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
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Charging);
    }

    public void ChargeUp(int count)
    {
        if (count == 1)
        {
            charge1.Stop();
            charge2.Play();
            GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Charging);
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
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.PinPointDown);
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
        effect.InitFollow(EffectType.None, point.gameObject, 2f);
    }

    public void BoxCol(int key)
    {
        effect = skillManager.SpawnEffect(23);
        effect.InitFollow(EffectType.Once, point.gameObject, 0.13f);
        effect.Key = key;
    }

    public void NarakaEffect()
    {
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Naraka2);
        effect = skillManager.SpawnEffect(12);
        effect.Init(EffectType.None, point.position, 1);
        StartCoroutine(CameraShack());
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
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Samsara);
    }

    public void SetSlashCycle()
    {
        StopCharge();
        effect = skillManager.SpawnEffect(3);
        effect.Init(EffectType.None, transform.position, 1);
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Gordian_Swing1);
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
            GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Gordian_Wave);
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
        effect.InitFollow(EffectType.None, gameObject, 1.2f);
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
        foreach(Effect e  in effects)
        {
            e.ReturenEffect();
        }
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
            effects[i].transform.position = transform.position + transform.forward * i * Vector3.Distance(dirction,Vector3.zero);
            effects[i].SetRotation(transform.rotation);
        }
    }

    public void DropMissile()
    {
        StartCoroutine(StartDrop());
    }

    private IEnumerator StartDrop()
    {
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.DesignatedEliminate);
        effect = skillManager.SpawnEffect(22);
        effect.Init(EffectType.None, transform.position - transform.forward * 20 + Vector3.up*2 + Vector3.back, 1f);
        effect.SetRotation(transform.rotation);
        effect.MoveForward();
        yield return YieldInstructionCache.WaitForSeconds(0.2f);

        for (int i = 0; i < 5; i++)
        {
            effect = skillManager.SpawnEffect(20);
            effect.Init(EffectType.None, effects[i].transform.position + Vector3.up*50, 0.3f);
            effect.SetRotation(effects[i].transform.rotation);
            effect.MoveDown();
            yield return YieldInstructionCache.WaitForSeconds(0.3f);

            effect = skillManager.SpawnEffect(21);
            effect.Key = skillManager.GetCurrentKey();
            effect.Init(EffectType.Once, effects[i].transform.position, 0.5f);
            effect.SetRotation(effects[i].transform.rotation);
            effects[i].ReturenEffect();
        }
        effects.Clear();
        
        player.SetIdle();
    }
    /*
    private void Update()
    {
        if (Dimension_Distortion_Trigger)
        {
            if(frame == 0)
            {
                GameManager.Inst.PlayerIsController(false);
                RedTrail();
                Time.timeScale = 0f;
            }
            else if(frame == 30)
            {
                fadeImg.enabled = true;
                SetGray(false);

                fadeImg.raycastTarget = true;
            }
            else if(frame >= 31 && frame <= 60)
            {
                Color alpha = fadeImg.color;
                alpha.a = Mathf.Lerp(0, 1, (frame-31)/30);
                fadeImg.color = alpha;
            }
            else if (frame >= 61 && frame <= 90)
            {
                Color alpha = fadeImg.color;
                alpha.a = Mathf.Lerp(1, 0, (frame - 60) / 30);
                fadeImg.color = alpha;
            }
            else if(frame == 91)
            {
                fadeImg.raycastTarget = false; // 다른 UI 활성화
                Time.timeScale = 1;

                effect = skillManager.SpawnEffect(24);
                effect.Key = skillManager.GetCurrentKey();
                effect.Key = (effect.Key / 10) * 10 + 1;
                skillManager.SetCrowdControl(CrowdControlType.Stun);
                effect.Init(EffectType.Once, transform.position, 1f);
            }
            else if(frame == 92)
            {
                player.SetIdle();
                GameManager.Inst.PlayerIsController(true);
                Dimension_Distortion_Trigger = false;
            }
            frame++;
        }
    }
    */
    public void WhiteTrail()
    {
        SetColorInversion(0);
        SetGray(true);
        frame = 0;
        Dimension_Distortion_Trigger = true;
    }

    public void RedTrail()
    {

    }

    public void SetColorInversion(int use)
    {
        if (use > 0)
        {
            GameManager.Inst.PlayerIsController(false);
            postColor.gradingMode.Override(GradingMode.LowDefinitionRange);
        }
        else
        {
            postColor.gradingMode.Override(GradingMode.HighDefinitionRange);
        }
    }

    private void SetGray(bool gray)
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
