using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ashe : Hero
{
    [SerializeField] int concentraction; // Q 스킬 스택
    [SerializeField] List<Skill> skillList;
    private void Start()
    {
        sm = new StateMachine<Character>(this);

        skillDic = new Dictionary<int, Skill>();
        skillDic.Add((int)SKILL_TYPE.QSkill, skillList[(int)SKILL_TYPE.QSkill]);
        skillDic.Add((int)SKILL_TYPE.WSkill, skillList[(int)SKILL_TYPE.WSkill]);
        skillDic.Add((int)SKILL_TYPE.ESkill, skillList[(int)SKILL_TYPE.ESkill]);
        skillDic.Add((int)SKILL_TYPE.RSkill, skillList[(int)SKILL_TYPE.RSkill]);
        InitStats();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            UseSkill(SKILL_TYPE.QSkill);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            UseSkill(SKILL_TYPE.WSkill);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            UseSkill(SKILL_TYPE.ESkill);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            UseSkill(SKILL_TYPE.RSkill);
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
        concentraction = 0;
    }
    public override void Attack(IHitAble target)
    {

    }

    public override void UseSkill(SKILL_TYPE skillType)
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
