using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheRSkill : ActiveSkill
{
    [SerializeField] private GameObject skillEffect;


    public override void Active()
    {
        owner.animator.Play("Attack_Ultimate");
        Instantiate(skillEffect,transform.position,transform.rotation).GetComponent<AsheRSkillEffect>().owner = (Ashe)owner;

    }
}
