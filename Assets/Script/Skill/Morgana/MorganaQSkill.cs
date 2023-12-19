using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganaQSkill : ActiveSkill
{
    [SerializeField] GameObject skillEffect;
    [SerializeField] Transform skillPosition;

    public override void Active()
    {
        base.Active();
        GameObject temp = Instantiate(skillEffect,skillPosition);
    }
}
