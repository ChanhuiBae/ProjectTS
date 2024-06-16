using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    private Image fadeImg;
    
    public void FadeInOut(float time)
    {
        StartCoroutine(Fading(time));
    }

    public void FadeOut(float time)
    {
        StartCoroutine(Fade(1f, 0f, time));
    }

    private IEnumerator Fading(float time)
    {
        time /= 2;
        StartCoroutine(Fade(0f, 1f, time));
        yield return YieldInstructionCache.WaitForSeconds(time);
        StartCoroutine(Fade(1f, 0f, time));
    }

    IEnumerator Fade(float start, float end, float fadeTime)
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        GameManager.Inst.PlayerIsController(false);

        fadeImg.raycastTarget = true;
        fadeTime = Mathf.Clamp(fadeTime, 0.1f, 1f);
        float percent = 0f;
        float current = 0f;

        Color alpha = fadeImg.color;
        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            alpha.a = Mathf.Lerp(start, end, percent);
            fadeImg.color = alpha;
            yield return null;
        }

        if (end < 0.1f)
            fadeImg.raycastTarget = false; // 다른 UI 활성화
        else
            fadeImg.raycastTarget = true;

        GameManager.Inst.PlayerIsController(true);
    }
}