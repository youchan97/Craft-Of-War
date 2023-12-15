using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheQSkill : ToggleSkill
{

    public AsheQSkill(Hero owner) : base(owner)
    {
    }

    public override void Active()
    {
        

        base.Active();
        Debug.Log("Ashe Q skill");
    }
}
