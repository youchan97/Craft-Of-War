using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    private float coolTime;
    private bool isCool;
    
    public bool IsCool { get { return isCool; } }
    public ActiveSkill(Hero owner) : base(owner)
    {
    }

    public override void Active()
    {
        Debug.Log("ActiveSkill Invoke!");
    }

    public override void SkillInit()
    {
        throw new System.NotImplementedException();
    }
}
