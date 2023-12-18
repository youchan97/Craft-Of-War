using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroIdleState : HeroState
{
    public override void Enter()
    {
        hero.curState = HERO_STATE.IDLE;
        hero.animator.SetBool("IsMove", false);
    }

    public override void Update()
    {

        if(hero.Agent.velocity != Vector3.zero)
        {
            sm.SetState((int)HERO_STATE.MOVE);
        }
        
    }

}
