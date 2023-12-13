using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterAttackState : MonsterState
{
    List<string> attackList; 
    float curTime = 0f;
    IEnumerator attackCo;

    public override void Init(IStateMachine stateMachine)
    {
        base.Init(stateMachine);
        attackList = new List<string>();
        attackList.Add("Attack");
        attackList.Add("Attack04");
    }

    public override void Enter()
    {
        monster.monState = MONSTER_STATE.ATTACK;
        monster.Agent.isStopped = true;
        attackCo = AttackProcess();
        monster.StartCoroutine(attackCo);
    }

    public override void Update()
    {
        if (monster.monState == MONSTER_STATE.DIE) return;

        if (Vector3.Distance(monster.transform.position, monster.DetectiveComponent.LastDetectivePos) > monster.CharInfo.AtkRange)
        {
            sm.SetState((int)MONSTER_STATE.MOVE);
        }

        if (monster.DetectiveComponent.IsRangeDetection == false)
        {
            monster.Agent.SetDestination(monster.OriginPos);
            sm.SetState((int)MONSTER_STATE.IDLE);
        }

        

    }

    IEnumerator AttackProcess()
    {
        while (true)
        {
            curTime += Time.deltaTime;
            Debug.Log(curTime);
            if (curTime > monster.CharInfo.AtkSpeed)
            {
                monster.Anim.Play(attackList[0]);
                
            }
           
            if (curTime > monster.CharInfo.AtkSpeed * 2)
            {
                monster.Anim.Play(attackList[1]);                
                curTime = 0f;
            }
            
            yield return null;
        }
            
    }


    public override void Exit()
    {
        monster.Agent.isStopped = false;
        monster.StopCoroutine(attackCo);
    }
}
