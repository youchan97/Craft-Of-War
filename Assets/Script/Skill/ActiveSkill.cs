using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    public ActiveSkill(Hero owner) : base(owner)
    {
    }

    public override void Active()
    {
        Debug.Log("ActiveSkill Invoke!");
    }
}
