using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheESkill : ActiveSkill
{
    public override void Active()
    {
        Ashe realOwner = (Ashe)owner;
        realOwner.animator.SetBool("QSkill", true);
        realOwner.transform.forward = (realOwner.clickPos - realOwner.transform.position).normalized;

            GameObject temp = realOwner.InstantiateVFX("fx_butterfly", realOwner.defaultTrans, true);
            temp.transform.SetParent(null);
            temp.transform.forward = (realOwner.clickPos - realOwner.transform.position).normalized;
        realOwner.eSkilldelayCo = StartCoroutine(realOwner.ESkillDelayCo());
    }
    public override void SkillInit()
    {
        CoolTime = 5f;
    }
}
