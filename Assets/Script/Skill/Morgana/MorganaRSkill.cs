using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganaRSkill : ActiveSkill
{
    [SerializeField] GameObject skillEffect;
    [SerializeField] LayerMask targetLayer;


    public override void SkillInit()
    {
        base.SkillInit();
        CoolTime = 10;
        range = 10;
    }

    public override void Active()
    {
        base.Active();
        owner.animator.Play("SpellCast");
        Collider[] cols = Physics.OverlapSphere(owner.transform.position, range,targetLayer);
        if(cols.Length > 0 )
        {

        }
    }

}
