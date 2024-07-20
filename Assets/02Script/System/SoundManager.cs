using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public enum BGM_Type
{
    BGM_Play = 0,
}

public enum SFX_Type
{
    SFX_EXP = 0,
    SFX_Hit = 1,
    SFX_LevelUp = 2,
}

public enum Skill_SFX
{
    Hammer_Attack1 = 0,
    Hammer_Attack2 = 1,
    Charging = 2,
    Gordian_Swing1 = 3,
    Gordian_Swing2 = 4,
    Gordian_Wave = 5,
    DragonHammer = 6,
    Attraction_Pick = 7,
    Attraction_Zoom = 8,
    Attraction_Thunder = 9,
    Samsara = 10,
    Naraka1 = 11,
    Naraka2 = 12,
}

public class SoundManager : MonoBehaviour
{
    private static int max = 0;
    private static int min = -40;
    [SerializeField]
    private AudioMixer masterMixer;

    [SerializeField]
    private AudioSource bgmAudio;
    [SerializeField]
    private List<AudioClip> bgmList;
    private float current;
    private float percent;

    private int curser = 0;
    [SerializeField]
    private List<AudioSource> sfxPlayers;
    [SerializeField]
    private List<AudioClip> sfxList;
    private int skillCurser = 0;
    [SerializeField]
    private List<AudioSource> skillPlayers;
    [SerializeField]
    private List<AudioClip> skillSFXList;

    private void Start()
    {
        SettingData data = GameManager.Inst.GetSettinggData;
        SetVolumBGM(data.bgm);
        SetVolumSFX(data.sfx);
    }

    public void SetVolumBGM(float volum)
    {
        if(volum < min)
        {
            masterMixer.SetFloat("BGM", -80);
        }
        else
        {
            masterMixer.SetFloat("BGM", volum);
        }
    }

    public void SetVolumSFX(float volum)
    {
        if (volum < min)
        {
            masterMixer.SetFloat("SFX", -80);
            masterMixer.SetFloat("Skill", -80);
        }
        else
        {
            masterMixer.SetFloat("SFX", volum);
            masterMixer.SetFloat("Skill", volum);
        }
    }

    public void ChangeBGM(BGM_Type newBGM)
    {
        StartCoroutine(ChangeBGMClip(bgmList[(int)newBGM]));
    }


    IEnumerator ChangeBGMClip(AudioClip audioClip)
    {
        current = 0;
        percent = 0;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 1f;
            bgmAudio.volume = Mathf.Lerp(1f, 0f, percent);
            yield return null;
        }

        bgmAudio.clip = audioClip;
        bgmAudio.Play();
        current = 0;
        percent = 0;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 1f;
            bgmAudio.volume = Mathf.Lerp(0f, 1f, percent);
            yield return null;
        }
    }


    public void PlaySFX(SFX_Type SFX)
    {
        sfxPlayers[curser].clip = sfxList[(int)SFX];
        sfxPlayers[curser].Play();

        curser++;
        if (curser > sfxPlayers.Count - 1)
        {
            curser = 0;
        }
    }

    public void PlaySKill(Skill_SFX skill)
    {
        skillPlayers[skillCurser].clip = skillSFXList[(int)skill];
        skillPlayers[skillCurser].Play();

        skillCurser++;
        if (skillCurser > skillPlayers.Count - 1)
        {
            skillCurser = 0;
        }
    }

    private static SoundManager instance;
    public static SoundManager Inst
    {
        get { return instance; }
    }
}
