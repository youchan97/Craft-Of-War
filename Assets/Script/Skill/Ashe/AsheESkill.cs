using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheESkill : ActiveSkill
{
    public override void Active()
    {
       base.Active();
    }

    public override void SkillInit()
    {
        CoolTime = 3f;
    }
}
