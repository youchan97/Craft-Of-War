using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganaQSkill : ActiveSkill
{
    [SerializeField] GameObject skillEffect;

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
            MorganaQSkillEffect effect = Instantiate(skillEffect, owner.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity).GetComponent<MorganaQSkillEffect>();
            effect.Direction = hit.point - owner.transform.position;
            effect.transform.forward = effect.Direction;
        }
    }
}
