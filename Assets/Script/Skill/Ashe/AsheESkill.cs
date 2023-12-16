using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheESkill : ActiveSkill
{
    public AsheESkill(Hero owner) : base(owner)
    {
    }

    public override void Active()
    {
       base.Active();
    }

    public override void SkillInit()
    {
        CoolTime = 3f;
    }
}
