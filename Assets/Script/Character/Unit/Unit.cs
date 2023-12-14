using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public enum UNIT_STATE
{
    None,
    Idle,
    Move,
    Attack,
    Work,
    Die
}

public abstract class Unit : Character
{
    public float coolTime;
    public bool isDetect;
    public float detectRange;
    public LayerMask target;
    

    public new void Awake()
    {
        base.Awake();
        isDetect = false;
        InitSm();
    }
    public void Update()
    {
        Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, detectRange, target);
        sm.UpdateState();
        animator.SetInteger("State", sm.stateEnumInt);
        if(cols.Length > 0 )
        {
            isDetect = true;
        }
        else
        {
            isDetect = false;
        }
    }

    private void InitSm()
    {
        sm.AddState((int)UNIT_STATE.Idle, new UnitIdleState());
        sm.AddState((int)UNIT_STATE.Move, new UnitMoveState());
        sm.AddState((int)UNIT_STATE.Attack, new UnitAttackState());
        sm.AddState((int)UNIT_STATE.Work, new UnitMoveState());
        sm.AddState((int)UNIT_STATE.Die, new UnitDieState());
        //기본상태로 돌려놈
        sm.SetState((int)UNIT_STATE.Idle);
    }

    public override void Die()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).length > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        Destroy(this.gameObject);
    }
}
