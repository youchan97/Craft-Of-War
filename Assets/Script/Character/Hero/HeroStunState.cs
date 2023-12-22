using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStunState : HeroState
{

    public override void Enter()
    {
        hero.curState = HERO_STATE.STUN;
        hero.animator.SetBool("IsStun", true);
    }



    public override void Exit()
    {
        hero.animator.SetBool("IsStun", false);
    }
}
