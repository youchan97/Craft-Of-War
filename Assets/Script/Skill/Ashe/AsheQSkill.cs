using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheQSkill : ActiveSkill
{
    public override void Active()
    {
        base.Active();
        Debug.Log("Ashe Q skill");
    }
}
