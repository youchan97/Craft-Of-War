using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitIdleState : UnitState
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (owner.agent.velocity != Vector3.zero)
            sm.SetState((int)UNIT_STATE.Move);
        if (owner.DetectiveComponent.IsRangeDetection)
            sm.SetState((int)UNIT_STATE.Attack);
        if (owner.Hp <= 0)
            sm.SetState((int)UNIT_STATE.Die);
    }
}
public class UnitMoveState : UnitState
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (owner.agent.velocity == Vector3.zero)
            sm.SetState((int)UNIT_STATE.Idle);
        if (owner.DetectiveComponent.IsRangeDetection)
            sm.SetState((int)UNIT_STATE.Attack);
        if (owner.Hp <= 0)
            sm.SetState((int)UNIT_STATE.Die);
    }
}
public class UnitAttackState : UnitState
{
    public override void Enter()
    {

    }

    public override void Exit()
    {
       
    }

    public override void Update()
    {
        if (!owner.DetectiveComponent.IsRangeDetection)
            sm.SetState((int)UNIT_STATE.Idle);
        if (owner.Hp <= 0)
            sm.SetState((int)UNIT_STATE.Die);
    }
}

public class UnitDieState : UnitState
{
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
public class UnitWorkState : UnitState
{
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




public abstract class UnitState : State 
{
    protected Unit owner;
    public override void Init(IStateMachine sm)
    {
        this.sm = sm;
        owner = (Unit)sm.GetOwner();
    }

}
