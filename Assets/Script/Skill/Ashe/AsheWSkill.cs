using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheWSkill : ActiveSkill
{
    public AsheWSkill(Hero owner) : base(owner)
    {
    }

    public override void Active()
    {
        base.Active();
        Debug.Log("Ashe W skill");
    }
}
