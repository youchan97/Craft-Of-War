using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheRSkill : ActiveSkill
{
    [SerializeField] private GameObject skillEffect;
    
    
    public override void SkillInit()
    {
        base.SkillInit();
        CoolTime = 10f;
        durationTime = 5f;
        damage = 100;
    }
    public override void Active()
    {
        base.Active();
        owner.animator.Play("Attack_Ultimate");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            AsheRSkillEffect se = Instantiate(skillEffect, owner.transform.position + new Vector3(0,1.5f,0), Quaternion.identity).GetComponent<AsheRSkillEffect>();
            se.Direction = hit.point - owner.transform.position;
            se.transform.forward = se.Direction;
        }
       
        
    }
}
