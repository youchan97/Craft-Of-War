using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterAttackState : MonsterState
{
    float curTime = 0f;
    IEnumerator attackCo;

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
        curTime = 1f;
        while (true)
        {
            curTime += Time.deltaTime;

            if (curTime > monster.CharInfo.AtkSpeed)
            {
                monster.Anim.Play("Attack");
                curTime = 0;
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
