using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : Skill
{
    public PassiveSkill(Hero owner) : base(owner)
    {
    }

    public override void Active()
    {
        Debug.Log("PassiveSkill Invoke!");
    }

    public override void SkillInit()
    {
        throw new System.NotImplementedException();
    }
}
