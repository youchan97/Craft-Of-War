using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnitStragy
{
    public Unit owner;
    public BattleUnitStragy(Unit owner)
    {
        this.owner = owner;
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
    }

    public override void Proceed()
    {
    }
}

public class RangeUnitStragy : BattleUnitStragy
{
    public RangeUnitStragy(Unit owner) : base(owner)
    {
    }

    public override void Init()
    {
    }

    public override void Proceed()
    {
    }
}

public class HealerStragy : BattleUnitStragy
{
    public HealerStragy(Unit owner) : base(owner)
    {
    }

    public override void Init()
    {
    }

    public override void Proceed()
    {
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
    BattleUnitStragy unitStragy;
    public BATTLE_UNIT unitType;
    public Dictionary<BATTLE_UNIT, BattleUnitStragy> stragyDic;

    private void Start()
    {
        Hp = 0;
        StragyInit();
    }

    public void Proceed()
    {
        unitStragy.Proceed();
    }

    void StragyInit()//전략 연결
    {
        stragyDic = new Dictionary<BATTLE_UNIT, BattleUnitStragy>()
        {
            { BATTLE_UNIT.Melee, new MeleeUnitStragy (this) },
            { BATTLE_UNIT.Range, new RangeUnitStragy (this) },
            { BATTLE_UNIT.Healer, new HealerStragy (this) },
        };
        unitStragy = stragyDic[unitType];
        unitStragy.Init();
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
        Destroy(this.gameObject);
    }
}
