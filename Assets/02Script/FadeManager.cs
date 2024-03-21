using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    private Image fadeImg;

    public void Fade_InOut(bool isIn)
    {
        if (isIn)
            StartCoroutine(Fade(1f, 0f, 0.25f));
        else
            StartCoroutine(Fade(0f, 1f, 0.25f));
    }

    IEnumerator Fade(float start, float end, float fadeTime)
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        //GameManager.Inst.PlayerIsController(false);
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
            fadeImg.raycastTarget = false; // �ٸ� UI Ȱ��ȭ
        else
            fadeImg.raycastTarget = true;

       // GameManager.Inst.PlayerIsController(true);
    }
}