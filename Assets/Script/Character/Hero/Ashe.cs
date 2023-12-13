using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ashe : Hero
{
    public Skill skills;

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void Hit(IAttackAble attacker)
    {
        throw new System.NotImplementedException();
    }

    public override void InitStats()
    {
        base.InitStats();
        info.MaxHp = 200;
        info.CurentHp = info.MaxHp;
        info.Atk = 10;
        info.Def = 10;
        MoveSpeed = 6f;
        info.AtkSpeed = 1f;
        info.AtkRange = 10f;
        Agent.speed = MoveSpeed;
        Agent.angularSpeed = 300f;
    }

    public override void UseSkill(Skill skill)
    {
        
    }
}
