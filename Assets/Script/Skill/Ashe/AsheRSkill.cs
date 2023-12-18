using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheRSkill : ActiveSkill
{
    [SerializeField] private GameObject skillEffect;
    public AsheRSkill(Hero owner) : base(owner)
    {

    }

    public override void Active()
    {
        Instantiate(skillEffect,transform.position,transform.rotation);
        base.Active();
        Debug.Log("Ashe R skill");
        
    }
}
