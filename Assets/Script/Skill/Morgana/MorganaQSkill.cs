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
        durationTime = 3f;
        range = 20f;
    }
    private void Update()
    {
        if(Vector3.Distance(owner.transform.position, skillEffect.transform.position) > range)
        {
            //Destroy(skillEffect);
        }
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
