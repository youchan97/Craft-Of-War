using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheRSkill : ActiveSkill
{
    public override void Active()
    {
        base.Active();
        Debug.Log("Ashe R skill");
    }
}
