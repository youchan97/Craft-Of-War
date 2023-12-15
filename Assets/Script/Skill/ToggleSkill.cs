using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSkill : Skill
{
    public ToggleSkill(Hero owner) : base(owner)
    {
    }

    public override void Active()
    {
        Debug.Log("ToggleSkill Invoke!");
    }
}
