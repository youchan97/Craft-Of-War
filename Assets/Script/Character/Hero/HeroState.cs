using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroState : State
{
    protected Hero hero;

    public override void Init(IStateMachine sm)
    {
        base.Init(sm);
        hero = (Hero)sm.GetOwner();
    }
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}


