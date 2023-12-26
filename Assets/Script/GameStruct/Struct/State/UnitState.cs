using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Photon.Pun;

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
      //  Debug.Log(owner.agent.velocity);
        if (owner.agent.velocity != Vector3.zero)
            sm.SetState((int)UNIT_STATE.Move);
        if (owner.DetectiveComponent.IsRangeDetection)
        {
            if(owner.gameObject.GetComponent<BattleUnit>()?.unitType == BATTLE_UNIT.Healer)
            {
                for(int i = 0; i< owner.DetectiveComponent.cols.Length; i++)
                {
                    if (owner.DetectiveComponent.cols[i].gameObject.GetComponent<Character>().Hp < 50)
                        sm.SetState((int)UNIT_STATE.Attack);
                }
            }
            else
                sm.SetState((int)UNIT_STATE.Attack);
        }
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
        {
            if (owner.gameObject.GetComponent<BattleUnit>()?.unitType == BATTLE_UNIT.Healer)
            {
                if (owner.agent.velocity == Vector3.zero)
                    sm.SetState((int)UNIT_STATE.Idle);
                else
                    sm.SetState((int)UNIT_STATE.Move);
            }
            else
                sm.SetState((int)UNIT_STATE.Attack);
        }
        if (owner.Hp <= 0)
            sm.SetState((int)UNIT_STATE.Die);
    }
}
public class UnitAttackState : UnitState
{
    //bool isAttack = false;
    public override void Enter()
    {
    }

    public override void Exit()
    {
        //isAttack = false;
    }

    public override void Update()
    {
        //owner.transform.forward = (owner.DetectiveComponent..transform.position - owner.transform.position).normalized;
        /*if (owner.agent.velocity != Vector3.zero)
            sm.SetState((int)UNIT_STATE.Move);*/
        //((BattleUnit)owner).battleStragy.Proceed();
        if (owner.gameObject.GetComponent<BattleUnit>()?.unitType == BATTLE_UNIT.Healer)
        {
            if (owner.DetectiveComponent.IsRangeDetection == false)
                sm.SetState((int)UNIT_STATE.Idle);
            else
            {
                int unitCount = 0;
                for (int i = 0; i < owner.DetectiveComponent.cols.Length; i++)
                {
                    if (owner.DetectiveComponent.cols[i].gameObject.GetComponent<Character>().Hp > 80)
                    {
                        unitCount++;
                        if (unitCount == owner.DetectiveComponent.cols.Length)
                            sm.SetState((int)UNIT_STATE.Idle);
                    }
                }
            }
        }
        if (owner.DetectiveComponent.IsRangeDetection == false)
        {   
            sm.SetState((int)UNIT_STATE.Idle);
        }
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
        if(owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f && owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            owner.GetComponent<Collider>().enabled = false;
        }
        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            GameManager.Instance.Population--;
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
        if (owner.agent.velocity != Vector3.zero)
            sm.SetState((int)UNIT_STATE.Move);
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
