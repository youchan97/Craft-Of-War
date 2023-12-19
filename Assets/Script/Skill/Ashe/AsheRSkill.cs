using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheRSkill : ActiveSkill
{
    [SerializeField] private GameObject skillEffect;


    public override void SkillInit()
    {
        base.SkillInit();
        CoolTime = 10f;
    }
    public override void Active()
    {
        owner.animator.Play("Attack_Ultimate");
        Instantiate(skillEffect, transform.position, transform.rotation);

    }
}
