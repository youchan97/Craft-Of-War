using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitIdleState : UnitState
{
    public override void Enter()
    {
        Debug.Log("IDle Enter");
    }

    public override void Exit()
    {
        Debug.Log("IDle Exit");
    }

    public override void Update()
    {
        Debug.Log(owner.agent.velocity);
        if (owner.agent.velocity != Vector3.zero)
            sm.SetState((int)UNIT_STATE.Move);
        if (owner.isDetect)
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
        if (owner.isDetect)
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
        if (owner.agent.velocity != Vector3.zero)
            sm.SetState((int)UNIT_STATE.Move);
        if (owner.isDetect == false)
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
        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            GameManager.Instance.unitObjectPool.ReturnPool(owner.gameObject, owner.obpId);//¿Œµ¶Ω∫πŸ≤„æﬂ«‘
        }
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
