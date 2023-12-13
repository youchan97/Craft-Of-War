using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SupplyUnit : Unit
{
    float resourseCool = 3f;
    public Vector3 resourseTf = Vector3.zero;//시작, 클릭될때 자원클릭되면 rts컨트롤러해서 벡터를 넣어줌
    public Vector3 nexusTf = Vector3.zero;//끝


    private bool isResourseClicked;
    public bool IsResourseClicked
    {
        get { return isResourseClicked; }
        set { isResourseClicked = value; }
    }
    public new void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        StartCoroutine(ResourseCo());
    }



    public class WaitForClickedTarget : CustomYieldInstruction
    {
        public SupplyUnit supplyUnit;
        public WaitForClickedTarget(SupplyUnit supplyUnit)
        {
            this.supplyUnit = supplyUnit;
        }
        public override bool keepWaiting
        {
            get
            {
                return !supplyUnit.IsResourseClicked;
            }
        }
    }

    public IEnumerator ResourseCo()
    {
        float curCool = resourseCool;

        while (true)
        {
            yield return null;
            //자원옮기면서 순환하는 코드
            
            Debug.LogWarning("sdsdsd");
            yield return new WaitForClickedTarget(this);


            Collider[] cols = Physics.OverlapSphere(resourseTf, 50f);
            //주변 넥서스 찾기, 범위를 넓게 줘야함
            foreach (var targetBuilding in cols)
            {
                if (targetBuilding.TryGetComponent(out NexusBuilding nexusBuilding))
                {
                    nexusTf = nexusBuilding.transform.position;
                }
            }

            agent.SetDestination(resourseTf);

            while (true)
            {
                //리소스에서 도착했을때
                if (Vector3.Distance(transform.position, resourseTf) <= 0.5f)
                {
                    curCool -= Time.fixedDeltaTime;
                    if (curCool <= 0)
                    {
                        agent.SetDestination(nexusTf);
                        curCool = resourseCool;
                    }
                }

                //넥서스에 도착했을때
                if (Vector3.Distance(transform.position, nexusTf) <= 0.5f)
                {
                    agent.SetDestination(resourseTf);
                    GameManager.Instance.Gold += 10;
                    //디버깅용
                }
                yield return null;
            }

        }
    }

    public override void InitStats()
    {
        throw new NotImplementedException();
    }

    public override void Attack(IHitAble target)
    {
        throw new NotImplementedException();
    }

    public override void Hit(IAttackAble attacker)
    {
        throw new NotImplementedException();
    }

    public override void Die()
    {
        throw new NotImplementedException();
    }
}

