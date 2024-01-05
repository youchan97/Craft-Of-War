using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnitStrategy : MonoBehaviour
{
    protected BattleUnit owner;

    public BattleUnitStrategy(BattleUnit owner)
    {
        this.owner = owner;
    }

    public abstract void Proceed();//공격이나,힐

    public abstract void Init();//초기화
}

public class MeleeUnitStrategy : BattleUnitStrategy
{
    public MeleeUnitStrategy(BattleUnit owner) : base(owner)
    {
    }

    public override void Init()
    {
        owner.priority = 3;
    }

    public override void Proceed()
    {
        MeleeAttackMethod();
    }

    public void MeleeAttackMethod()
    {
        StartCoroutine(AttackCO());
    }

    IEnumerator AttackCO()
    {
        yield return new WaitForSeconds(0.5f);
        Collider[] cols = owner.detectCompo.targetCols;

        if (cols.Length <= 0)
            yield break;

        List<GameObject> targetList = new List<GameObject>();
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.GetComponent<IHitAble>() != null)
            {
                owner.priorityQueue.Enqueue(cols[i].gameObject.name, cols[i].gameObject.GetComponent<IHitAble>().Priority);
                targetList.Add(cols[i].gameObject);
            }
        }
        string name = owner.priorityQueue.Dequeue();
        foreach (GameObject target in targetList)
        {
            if (target.name == name)
            {
                target.GetComponent<IHitAble>().Hp -= this.gameObject.GetComponent<IAttackAble>().Atk;
                break;
            }
        }
        owner.priorityQueue.Clear();
    }
}

public class RangeUnitStrategy : BattleUnitStrategy
{
    public RangeUnitStrategy(BattleUnit owner) : base(owner)
    {
    }

    public override void Init()
    {
        owner.priority = 2;
    }

    public override void Proceed()
    {
        RangeAttackMethod();
    }
    public void RangeAttackMethod()
    {
        StartCoroutine(AttackCO());
    }

    IEnumerator AttackCO()
    {
        yield return new WaitForSeconds(0.5f);
        Collider[] cols = owner.detectCompo.targetCols;

        if (cols.Length <= 0)
            yield break;

        List<GameObject> targetList = new List<GameObject>();
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.GetComponent<IHitAble>() != null)
            {
                owner.priorityQueue.Enqueue(cols[i].gameObject.name, cols[i].gameObject.GetComponent<IHitAble>().Priority);
                targetList.Add(cols[i].gameObject);
            }
        }
        string name = owner.priorityQueue.Dequeue();
        foreach (GameObject target in targetList)
        {
            if (target.name == name)
            {
                target.GetComponent<IHitAble>().Hp -= this.gameObject.GetComponent<IAttackAble>().Atk;
                break;
            }
        }
        owner.priorityQueue.Clear();
    }
}

public class HealerStrategy : BattleUnitStrategy
{
    public HealerStrategy(BattleUnit owner) : base(owner)
    {
    }

    public override void Init()
    {
        owner.priority = 1;
    }

    public override void Proceed()
    {
        HealMethod();
    }
    public void HealMethod()
    {
        Collider[] cols = owner.detectCompo.targetCols;
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].GetComponent<Character>() != null)
            {
                cols[i].GetComponent<Character>().Hp += 5;
            }
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
    public BattleUnitStrategy battleStragy;
    public BATTLE_UNIT unitType;
    private Dictionary<BATTLE_UNIT, BattleUnitStrategy> stragyDic;

    //추가부
    public DetectiveComponent detectCompo;
    public PriorityQueue<string, int> AdaptpriorityQueue;
    public IPrioxyQueue<string, int> priorityQueue;
    private void Start()
    {
        detectCompo = GetComponent<DetectiveComponent>();
        PriorityQueueInit();
        StrategyInit();
    }

    public void Proceed()
    {
        battleStragy.Proceed();
    }

    void StrategyInit()//전략 연결
    {
        stragyDic = new Dictionary<BATTLE_UNIT, BattleUnitStrategy>()
        {
            { BATTLE_UNIT.Melee, new MeleeUnitStrategy (this) },
            { BATTLE_UNIT.Range, new RangeUnitStrategy (this) },
            { BATTLE_UNIT.Healer, new HealerStrategy (this) },
        };
        battleStragy = stragyDic[unitType];
        battleStragy.Init();
    }

    public void PriorityQueueInit()
    {
        AdaptpriorityQueue = new PriorityQueue<string, int>();
        priorityQueue = AdaptpriorityQueue;
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
