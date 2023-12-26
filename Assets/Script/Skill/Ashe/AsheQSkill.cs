using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheQSkill : ToggleSkill
{
    public override void Active()
    {
        Ashe realOwner = (Ashe)owner;
        realOwner.animator.SetBool("QSkill", true);
        if(realOwner.clickTarget != null)
        {
            realOwner.transform.forward = (realOwner.clickTarget.transform.position - realOwner.transform.position).normalized;
            for (int i = 0; i < realOwner.qSkillTrans.Length; i++)
            {
                GameObject temp = realOwner.InstantiateVFX("fx_small_arrow", realOwner.qSkillTrans[i], true);
                temp.transform.SetParent(null);
                temp.transform.forward = (realOwner.clickTarget.transform.position - realOwner.qSkillTrans[i].position).normalized;
            }
            realOwner.qSkilldelayCo = StartCoroutine(realOwner.QSkillDelayCo());
        }
        if (realOwner.clickBuilding != null)
        {
            realOwner.transform.forward = (realOwner.clickBuilding.transform.position - realOwner.transform.position).normalized;
            for (int i = 0; i < realOwner.qSkillTrans.Length; i++)
            {
                GameObject temp = realOwner.InstantiateVFX("fx_small_arrow", realOwner.qSkillTrans[i], true);
                temp.transform.SetParent(null);
                temp.transform.forward = (realOwner.clickBuilding.transform.position - realOwner.qSkillTrans[i].position).normalized;
            }
            realOwner.qSkilldelayCo = StartCoroutine(realOwner.QSkillDelayCo());
        }
    }
}
