using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public void PlayAttractionField()
    {
        GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_Attraction_Pick);
        StartCoroutine(PlayZoom());
    }

    private IEnumerator PlayZoom()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.08f);

        GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_Attraction_Zoom);
        StartCoroutine(PlayThunder());
    }

    private IEnumerator PlayThunder()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.6f);
        GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_Attraction_Thunder);
    }

    public void PlaySwing()
    {
        GameManager.Inst.soundManager.PlaySFX(SFX_Type.SFX_Gordian_Swing2);
    }
}
