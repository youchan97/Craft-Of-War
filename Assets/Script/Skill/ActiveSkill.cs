using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    public override void Active()
    {
        Debug.Log("ActiveSkill Invoke!");
    }
}
