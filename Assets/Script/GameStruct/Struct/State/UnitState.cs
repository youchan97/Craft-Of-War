using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Photon.Pun;

public class UnitIdleState : UnitState //유닛의 기본 상태
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (owner.agent.velocity != Vector3.zero) //유닛의 velocity가 0이아닌 즉 움직인다면
            sm.SetState((int)UNIT_STATE.Move); //Move 상태로 변경
        if (owner.DetectiveComponent.IsRangeDetection) //유닛이 적을 탐지했다면 (힐러는 아군)
        {
            if(owner.gameObject.GetComponent<BattleUnit>()?.unitType == BATTLE_UNIT.Healer)
            {
                //힐러일 경우 일정 Hp이하인 팀 유닛이 탐지될 경우 공격(힐)상태로 변경
                for(int i = 0; i< owner.DetectiveComponent.targetCols.Length; i++)
                {
                    if (owner.DetectiveComponent.targetCols[i].gameObject.GetComponent<Character>().Hp < 50)
                        sm.SetState((int)UNIT_STATE.Attack);
                }
            }
            else
                sm.SetState((int)UNIT_STATE.Attack); // 나머지 유닛들은 공격상태
        }
        if (owner.Hp <= 0) //유닛의 Hp가 0이 된다면 죽음 상태
            sm.SetState((int)UNIT_STATE.Die);
    }
}
public class UnitMoveState : UnitState //유닛의 움직이는 상태
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (owner.agent.velocity == Vector3.zero) //유닛이 움직이고 있지 않다면
            sm.SetState((int)UNIT_STATE.Idle); // 기본상태로 전환
        if (owner.DetectiveComponent.IsRangeDetection) //유닛이 적을 탐지했다면 (힐러는 아군)
        {
            //힐러 유닛의 경우 기본 상태에서만 힐이 가능하게 해 놓음
            if (owner.gameObject.GetComponent<BattleUnit>()?.unitType == BATTLE_UNIT.Healer)
            {
                if (owner.agent.velocity == Vector3.zero) //움직임이 없다면
                    sm.SetState((int)UNIT_STATE.Idle); //기본 상태로 전환
                else
                    sm.SetState((int)UNIT_STATE.Move); // 아니라면 계속 움직이는 상태
            }
            else //힐러 유닛이 아닌 다른유닛의 경우
                sm.SetState((int)UNIT_STATE.Attack); // 공격 상태로 전환
        }
        if (owner.Hp <= 0) // 유닛의 Hp가 0이 된다면 죽는 상태로 전환
            sm.SetState((int)UNIT_STATE.Die);
    }
}
public class UnitAttackState : UnitState // 유닛의 공격 상태
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (owner.gameObject.GetComponent<BattleUnit>()?.unitType == BATTLE_UNIT.Healer) //힐러 유닛의 경우
        {
            if (owner.DetectiveComponent.IsRangeDetection == false) //탐지된 유닛이 없다면
                sm.SetState((int)UNIT_STATE.Idle); // 기본 상태 전환
            else
            {
                int unitCount = 0; //유닛 수
                for (int i = 0; i < owner.DetectiveComponent.targetCols.Length; i++)
                {   //탐지된 유닛들의 Hp가 80을 넘는지 검사한다.
                    if (owner.DetectiveComponent.targetCols[i].gameObject.GetComponent<Character>().Hp > 80)
                    { 
                        unitCount++; //탐지된 유닛의 체력이 80이 넘을 때마다 카운트 증가
                        if (unitCount == owner.DetectiveComponent.targetCols.Length) //모든 유닛의 Hp가 80초과일 경우
                            sm.SetState((int)UNIT_STATE.Idle); //기본 상태 전환
                    }
                }
            }
        }
        if (owner.DetectiveComponent.IsRangeDetection == false)
        {   
            sm.SetState((int)UNIT_STATE.Idle); //탐지된 캐릭터가 없을 경우 기본 상태 전환
        }
        if (owner.Hp <= 0) //유닛의 Hp가 0일 경우 죽는 상태 전환
            sm.SetState((int)UNIT_STATE.Die);
    }
}

public class UnitDieState : UnitState // 유닛의 죽는 상태
{
    public override void Enter() //상태에 들어간 순간
    {
        owner.GetComponent<Collider>().enabled = false; //유닛의 콜라이더를 꺼 피격이 불가능하게 만듬
    }
    public override void Exit()
    {

    }
    public override void Update()
    {
        //유닛의 'Die'애니메이션이 실행되고 특정 구간이 지난 순간
        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f &&
            owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            GameManager.Instance.Population--; //현재 인구수 1 감소
            GameManager.Instance.unitObjectPool.ReturnPool(owner.gameObject, owner.obpId); // 오브젝트 풀에 비활성화 상태로 다시 돌아감
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
