using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : Skill
{
    public override void Active()
    {
        Debug.Log("PassiveSkill Invoke!");
    }
}
