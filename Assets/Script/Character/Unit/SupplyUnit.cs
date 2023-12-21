using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SupplyUnit : Unit
{
    //행동트리
    SequenceNode btRootNode;
    ////////
    float miningCool = 5f;
    public Vector3 mineTf = Vector3.zero;//시작, 클릭될때 자원클릭되면 rts컨트롤러해서 벡터를 넣어줌
    public Vector3 nexusTf = Vector3.zero;//끝

    bool isMineEnd = false;
    public bool isRunMineCoroutine;
    public bool isMineClicked;
    public IEnumerator mineCo;
    public new void Awake()
    {
        base.Awake();
        MiningBTInit();
        mineCo = MiningCo();
    }
    private void Start()
    {
    }

    public new void Update()
    {
        base.Update();
        btRootNode.Evaluate();
    }

    //채굴루틴 행동트리
    public void MiningBTInit()
    {

        btRootNode = new SequenceNode();

        ActionNode miningReadyCheckAction = new ActionNode();
        ActionNode miningAction = new ActionNode();
        btRootNode.Add(miningReadyCheckAction);
        btRootNode.Add(miningAction);
        
        //액션 구현부
        miningReadyCheckAction.action = () =>
        {
            if (isMineClicked)
            {
                return BTNode.State.SUCCESS;
            }
            return BTNode.State.FAIL;
        };

        //한번만 들어오고 루틴이 끝날때까지 반복안해줘야함
        miningAction.action = () =>
        {
            isMineClicked = false;
            Collider[] cols = Physics.OverlapSphere(mineTf, 100f);
            //주변 넥서스 찾기, 범위를 넓게 줘야함
            foreach (var targetBuilding in cols)
            {
                if (targetBuilding.TryGetComponent(out NexusBuilding nexusBuilding))
                {
                    nexusTf = nexusBuilding.transform.position;
                }
            }
            isMineEnd = false;
            agent.SetDestination(mineTf);
            StartCoroutine(mineCo);
            return BTNode.State.RUN;
        };

    }

    public IEnumerator MiningCo()
    {

        //isRunMineCoroutine = true;
        float curCool = miningCool;

        //여기 루틴 수정해야됌
        while (true)
        {
            //리소스에서 도착했을때
            if (Vector3.Distance(transform.position, mineTf) <= 3f && isMineEnd == false)
            {
                agent.ResetPath();
                sm.SetState((int)UNIT_STATE.Work);
                curCool -= Time.deltaTime;
                if (curCool <= 0)
                {
                    agent.SetDestination(nexusTf);
                    isMineEnd = true;
                    curCool = miningCool;
                }
                yield return null;
            }

            //넥서스에 도착했을때
            if (Vector3.Distance(transform.position, nexusTf) <= 8f && isMineEnd == true)
            {
                agent.ResetPath();
                agent.SetDestination(mineTf);
                GameManager.Instance.Mine += 10;
                isMineEnd = false;
                //디버깅용
            }

            ////목적지가 둘다가 아니게 되면
            //if (agent.pathEndPosition != nexusTf && agent.pathEndPosition != mineTf)
            //{
            //    sm.SetState((int)UNIT_STATE.Move);
            //    isRunMineCoroutine = false;
            //    break;
            //}
            yield return null;
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

