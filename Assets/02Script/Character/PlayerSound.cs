using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public void PlayAttractionField()
    {
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Attraction_Pick);
        StartCoroutine(PlayZoom());
    }

    private IEnumerator PlayZoom()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.08f);

        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Attraction_Zoom);
        StartCoroutine(PlayThunder());
    }

    private IEnumerator PlayThunder()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.6f);
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Attraction_Thunder);
    }

    public void PlaySwing()
    {
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Gordian_Swing2);
    }

    public void DragonSound()
    {
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.DragonHammer);
    }

    public void HammerAttack1()
    {
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Hammer_Attack1);
    }

    public void HammerAttack2()
    {
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.Hammer_Attack2);
    }

    public void PG_15()
    {
        GameManager.Inst.soundManager.PlaySKill(Skill_SFX.PG_15);
    }

}
