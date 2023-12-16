using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using static UnityEngine.UI.CanvasScaler;

public enum PLAY_MODE
{ RTS_MODE, AOS_MODE}


public class GameManager : SingleTon<GameManager>
{
    public PLAY_MODE playMode = PLAY_MODE.RTS_MODE;
    public Transform[] points= new Transform[2];
    public Transform buildSpawnPoint;
    public int index;

    public PhotonView pv;

    //옵저버
    public event Action onRoundStart;
    public event Action onRoundEnd;

    //자료구조
    public ObjectPool unitObjectPool;
    public ObjectPool buildingObjectPool;
    PriorityQueue<string, int> adapterPriorityQueue;
    IPrioxyQueue<string, int> priorityQueue;//우선순위 큐의 기능만 사용가능한 진짜 큐
    StateMachine<GameManager> stateMachine;

    //그래픽
    //public UniversalRendererData urData; //기능 봉인

    public RTSController rtsController;
    private int tree;
    public int Tree
    {
        get { return tree; }
        set { tree = value; }
    }
    private int gold;
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    private int population;

    public int Population
    {
        get { return population; }
        set { population = value; }
    }

    //Player 관련
    Hero playerHero;
    public Hero PlayerHero
    {
        get { return playerHero; } set { playerHero = value; }
    }
    protected override void Awake()
    {
        base.Awake();

        pv = GetComponent<PhotonView>();
        //Player 찾기
        //--- 몇가지 문제가 있어 보인다. 적이랑 같은 태그에 같은 스크립트일텐데 이렇게 찾는게 맞을까?
        playerHero = GameObject.FindGameObjectWithTag("PlayerHero").GetComponent<Hero>();
        playMode = PLAY_MODE.RTS_MODE;
        //캐릭터 출현 정보를 배열에 저장
        index = UnityEngine.Random.Range(0, points.Length);
        //캐릭터 생성
        PhotonNetwork.Instantiate(DropDownManager.selectHeroName, points[index].position, points[index].rotation, 0);
     //   if(pv.ViewID == 1)
        PhotonNetwork.Instantiate("Nexus", buildSpawnPoint.position, buildSpawnPoint.rotation, 0);
     //   else
     //       PhotonNetwork.Instantiate("Nexus", buildSpawnPoint.position, buildSpawnPoint.rotation, 0);
    }
    void Start()
    {
        StartCoroutine(PlayerInitCo());

        //우선큐
        adapterPriorityQueue = new PriorityQueue<string, int>();
        priorityQueue = adapterPriorityQueue;

        priorityQueue.Enqueue("궁병유닛", 1);
        priorityQueue.Enqueue("보병유닛", 2);
        priorityQueue.Enqueue("영웅", 3);
        priorityQueue.Enqueue("건물", 4);

        for (int i = 0; i < 4; i++)
        {
            //Debug.Log(priorityQueue.Dequeue());
        }

        //FSM 디버깅
        stateMachine = new StateMachine<GameManager>(this);
    }

    // Update is called once per frame
    void Update()
    {
        //게임매니저 상태머신 안쓸거같음
        //stateMachine.UpdateState();
    }

    //게임시작하고 플레이어 관련 초기화
    IEnumerator PlayerInitCo()
    {
        //플레이어 처음 진영 유닛과 넥서스 소환
        GameObject nexus =  buildingObjectPool.Pop(0);
        nexus.transform.position = new Vector3(20, 0, 480);//왼쪽 윗 진영 넥서스 스폰위치
        for (int i = 0; i < 5; i++)
        {
            GameObject supply = unitObjectPool.Pop(6);
            rtsController.fieldUnitList.Add(supply.GetComponent<UnitController>());
            supply.transform.position = new Vector3(20, 0, 470);
            supply.GetComponent<NavMeshAgent>().enabled = true;

            //가상의 경로지정하고 바로해제해야 찔금 움직여서 안겹침
            supply.GetComponent<NavMeshAgent>().SetDestination(supply.transform.position + new Vector3(0,0,-2));
            yield return null;
            supply.GetComponent<NavMeshAgent>().ResetPath();
        }
    }
}
