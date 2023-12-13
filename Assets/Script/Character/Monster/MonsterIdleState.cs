using UnityEngine;

public class MonsterIdleState : MonsterState
{
    public override void Enter()
    {
        monster.monState = MONSTER_STATE.IDLE;
        monster.Anim.SetBool("IsMove", false);
    }

    public override void Update()
    {
        if (monster.monState == MONSTER_STATE.DIE) return;

        if (monster.transform.position != monster.OriginPos)
        {
            monster.Agent.SetDestination(monster.OriginPos);
        }

        if(monster.DetectiveComponent.IsRangeDetection)
        {
            sm.SetState((int)MONSTER_STATE.MOVE);
        }

    }
    public override void Exit()
    {
        
    }

}
