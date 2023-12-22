using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnitStragy
{
    public Unit owner;
    public bool isAttack;
    public bool isHeal;
    public BattleUnitStragy(Unit owner)
    {
        this.owner = owner;
        isAttack = false;
        isHeal = false;
    }

    public abstract void Proceed();//공격이나,힐

    public abstract void Init();//초기화
}

public class MeleeUnitStragy : BattleUnitStragy
{
    public MeleeUnitStragy(Unit owner) : base(owner)
    {


    }

    public override void Init()
    {
        owner.priority = 3;
    }

    public override void Proceed()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f && owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && isAttack == false)
        {
            owner.DetectiveComponent.AttackMethod();
            isAttack = true;
        }
        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && isAttack == true)
        {
            isAttack = false;
        }
    }
}

public class RangeUnitStragy : BattleUnitStragy
{
    public RangeUnitStragy(Unit owner) : base(owner)
    {
    }

    public override void Init()
    {
        owner.priority = 2;
    }

    public override void Proceed()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f && owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && isAttack == false)
        {
            owner.DetectiveComponent.AttackMethod();
            isAttack = true;
        }
        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && isAttack == true)
        {
            isAttack = false;
        }
    }
}

public class HealerStragy : BattleUnitStragy
{
    public HealerStragy(Unit owner) : base(owner)
    {
    }

    public override void Init()
    {
        owner.priority = 1;
    }

    public override void Proceed()
    {
        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f && owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Heal") && isHeal == false)
        {
            owner.DetectiveComponent.HealMethod();
            isHeal = true;
        }
        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Heal") && isHeal == true)
        {
            isHeal = false;
        }
    }
}

public enum BATTLE_UNIT
{
    None,
    Melee,
    Range,
    Healer
}

public class BattleUnit : Unit
{
    public BattleUnitStragy battleStragy;
    public BATTLE_UNIT unitType;
    public Dictionary<BATTLE_UNIT, BattleUnitStragy> stragyDic;

    private void Start()
    {
        StragyInit();
    }

    public void Proceed()
    {
        battleStragy.Proceed();
    }

    void StragyInit()//전략 연결
    {
        stragyDic = new Dictionary<BATTLE_UNIT, BattleUnitStragy>()
        {
            { BATTLE_UNIT.Melee, new MeleeUnitStragy (this) },
            { BATTLE_UNIT.Range, new RangeUnitStragy (this) },
            { BATTLE_UNIT.Healer, new HealerStragy (this) },
        };
        battleStragy = stragyDic[unitType];
        battleStragy.Init();
    }

    public override void InitStats()
    {
        throw new System.NotImplementedException();
    }

    public override void Attack(IHitAble target)
    {
        throw new System.NotImplementedException();
    }

    public override void Hit(IAttackAble attacker)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        
    }
}
