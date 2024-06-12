using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public enum BGM_Type
{
    BGM_Home = 0,
    BGM_Stage = 1,
    BGM_Boss = 2,
}

public enum SFX_Type
{
    SFX_ChangeWeapon = 0,
    SFX_OnehandAttack = 1,
    SFX_Ranged = 2,
    SFX_Hit = 3,
    SFX_Coin = 4,
    SFX_Item = 5,
    SFX_Drink = 6,
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
        }
        else
        {
            masterMixer.SetFloat("SFX", volum);
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

    private static SoundManager instance;
    public static SoundManager Inst
    {
        get { return instance; }
    }
}
