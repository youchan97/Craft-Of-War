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
}
