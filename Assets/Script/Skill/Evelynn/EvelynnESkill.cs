using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvelynnESkill : ActiveSkill
{
    Evelynn realOwner;
    bool isFinished;
    private void Start()
    {
        realOwner = (Evelynn)owner;
    }
    public override void Active()
    {
        Debug.Log("¿Ã∫Ì∏∞ E");
        Vector3 target = realOwner.transform.position + (realOwner.transform.forward * range);
        realOwner.agent.SetDestination(target);
        realOwner.eSkilldelayCo = StartCoroutine(realOwner.ESkillDelayCo());
    }
}
