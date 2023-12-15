using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheQSkill : PassiveSkill
{
    private float lastTime;


    public AsheQSkill(Hero owner) : base(owner)
    {
    }

    public override void Active()
    {
        

        base.Active();
        Debug.Log("Ashe Q skill");
    }
}
