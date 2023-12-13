using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ashe : Hero
{

    private void Start()
    {
        sm = new StateMachine<Character>(this);
        skillDic = new Dictionary<int, Skill>();
        skillDic.Add((int)SKILL_TYPE.QSkill, new AsheQSkill());
        InitStats();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            UseSkill(SKILL_TYPE.QSkill, KeyCode.Q);
        }
    }
    public override void InitStats()
    {
        base.InitStats();
        info.MaxHp = 200;
        info.CurentHp = info.MaxHp;
        info.Atk = 10;
        info.Def = 10;
        MoveSpeed = 10f;
        info.AtkSpeed = 1f;
        info.AtkRange = 10f;
        Agent.speed = MoveSpeed;
        Agent.angularSpeed = 1200f;
    }


    public override void UseSkill(SKILL_TYPE skillType, KeyCode keyCode)
    {
        skillDic[(int)skillType].Active();
    }


    public override void Die()
    {
        
    }

    public override void Hit(IAttackAble attacker)
    {
        
    }
}
