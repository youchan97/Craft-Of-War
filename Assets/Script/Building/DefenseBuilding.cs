using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBuilding : Building, IAttackAble
{
    private int atk;
    public int Atk 
    {
        get => atk;
        set
        {
            atk = value;
        }
    }
    public DetectiveComponent detectiveCompo;

    public override void Awake()
    {
        base.Awake();
        atk = 10;
        detectiveCompo = GetComponent<DetectiveComponent>();
    }

    private void Start()
    {
        StartCoroutine(CheckUnitRange());
    }
    public override void Die()
    {
    }

    public override void Hit()
    {
    }
    public IEnumerator CheckUnitRange()
    {
        IHitAble target;
        float coolTime = 1.5f;
        while (true) 
        {
            if(detectiveCompo.cols.Length > 0)
            {
                foreach (var col in detectiveCompo.cols)
                {
                    if (col.gameObject.TryGetComponent(out IHitAble tg))
                    {
                        Debug.LogError(detectiveCompo.cols.Length);
                        target = tg;
                        yield return new WaitForSeconds(coolTime);
                        Attack(target);
                        break;
                    }
                }
            }
            yield return null;
        }
    }

    public void Attack(IHitAble target)
    {
        target.Hp -= Atk;
    }
}
