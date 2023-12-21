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
    DetectiveComponent detectiveCompo;

    public override void Awake()
    {
        base.Awake();
        atk = 10;
        detectiveCompo = new DetectiveComponent();
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
        while (true) 
        {
            yield return null;

            if (detectiveCompo.cols[0] == null)
                continue;

            foreach (var col in detectiveCompo.cols)
            {
                if (col.gameObject.TryGetComponent(out IHitAble tg) && col.gameObject.GetComponent<PhotonView>().IsMine == false)
                {
                    target = tg;
                    yield return new WaitForSeconds(3f);
                    Attack(target);
                    break;
                }
            }
        }
    }

    public void Attack(IHitAble target)
    {
        target.Hp -= Atk;
    }
}
