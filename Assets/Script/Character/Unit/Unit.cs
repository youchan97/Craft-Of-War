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
    Work
}

public abstract class Unit : Character
{
    public float coolTime;

    public new void Awake()
    {
        base.Awake();
        InitSm();
    }
    public void Update()
    {
        sm.UpdateState();
        animator.SetInteger("State", sm.stateEnumInt);
    }

    private void InitSm()
    {
        sm.AddState((int)UNIT_STATE.Idle, new UnitIdleState());
        sm.AddState((int)UNIT_STATE.Move, new UnitMoveState());
        sm.AddState((int)UNIT_STATE.Attack, new UnitAttackState());
        sm.AddState((int)UNIT_STATE.Work, new UnitMoveState());
        //기본상태로 돌려놈
        sm.SetState((int)UNIT_STATE.Idle);
    }
}
