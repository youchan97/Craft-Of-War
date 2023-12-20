using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganaWSkill : ActiveSkill
{
    [SerializeField] GameObject skillEffect;
    [SerializeField] float skillRange; // 스킬 범위

    public override void SkillInit()
    {
        base.SkillInit();
        CoolTime = 5f;
    }

    public override void Active()
    {
        base.Active();
        owner.animator.Play("SpellCast");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if (Physics.OverlapSphere(hit.point, skillRange).Length > 0)
            {

            }
        }

        

    }

}
