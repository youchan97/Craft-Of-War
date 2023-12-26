using UnityEngine.AI;
using UnityEngine;
using Photon.Pun;

public class MonsterMoveState : MonsterState
{
    public override void Enter()
    {
        monster.Anim.SetBool("IsMove", true);
        monster.monState = MONSTER_STATE.MOVE;
    }

    public override void Update()
    {
        if (monster.monState == MONSTER_STATE.DIE) return;

        if (monster.DetectiveComponent.IsRangeDetection == false)
        {
            monster.Agent.SetDestination(monster.OriginPos);
            sm.SetState((int)MONSTER_STATE.IDLE);
        }

        if(monster.Agent.remainingDistance <= monster.CharInfo.AtkRange)
        {
            sm.SetState((int)MONSTER_STATE.ATTACK);
        }

        monster.Agent.SetDestination(monster.DetectiveComponent.LastDetectivePos);
    }


    public override void Exit()
    {
        monster.Anim.SetBool("IsMove", false);
    }

        
}
