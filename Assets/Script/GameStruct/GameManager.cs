using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.CanvasScaler;

public enum PLAY_MODE
{ RTS_MODE, AOS_MODE}

public enum Tribe
{ HUMAN, ORC}

public class GameManager : SingleTon<GameManager>
{
    public PLAY_MODE playMode = PLAY_MODE.RTS_MODE;
    public Transform[] heroPoints = new Transform[2];
    public Transform[] buildPoints = new Transform[2];
    public GameObject[] monster = new GameObject[9];
    public Transform[] monsterSpawnPoints = new Transform[9];
    public Tribe tribe;
    public Transform shopPoint;

    public PhotonView pv;

    //옵저버
    public event Action onRoundStart;
    public event Action onRoundEnd;

    //자료구조
    public ObjectPool unitObjectPool;
    public ObjectPool buildingObjectPool;
    public ObjectPool monsterObjectPool;

    public GameManager(ObjectPool buildingOjbectPool)
    {
        this.buildingObjectPool = buildingOjbectPool;
    }
    //그래픽
    //public UniversalRendererData urData; //기능 봉인

    public RTSController rtsController;
    [SerializeField]
    private int mine;
    public int Mine
    {
        get { return mine; }
        set 
        { 
            mine = value;
            UIManager.Instance.mineText.text = mine.ToString();
        }
    }
    [SerializeField]
    private int gold;
    public int Gold
    {
        get { return gold; }
        set 
        {
            gold = value;
            UIManager.Instance.goldText.text = gold.ToString();
        }
    }
    [SerializeField]
    private int population;

    public int Population
    {
        get { return population; }
        set 
        {
            population = value;
            UIManager.Instance.PopulationText.text = (population + " / " + maxPopulation);
        }
    }
    [SerializeField]
    private int maxPopulation;
    public int MaxPopulation
    {
        get { return maxPopulation; }
        set
        {
            maxPopulation = value;
            UIManager.Instance.PopulationText.text = (population + " / " + maxPopulation);
        }
    }

    //Player 관련
    public Hero playerHero;
    public Hero PlayerHero
    {
        get { return playerHero; } set { playerHero = value; }
    }

    //씬관련
    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            onRoundStart();
        }
        if (scene.name == "MainMenu")
        {
            onRoundEnd();
        }
    }

    public void EventInit()
    {
        //초기 세팅비용 게임들어갈때,
        onRoundStart += () =>
        {
            Mine = 1000;//디버깅용
            Population = 0;
            Gold = 0;
            MaxPopulation = 5;
        };
    }
    protected override void Awake()
    {
        base.Awake();
        if (DropDownManager.selectTribe == "Human")
            tribe = Tribe.HUMAN;
        else if (DropDownManager.selectTribe == "Orc")
            tribe = Tribe.ORC;

        pv = GetComponent<PhotonView>();
        //Player 찾기
        //--- 몇가지 문제가 있어 보인다. 적이랑 같은 태그에 같은 스크립트일텐데 이렇게 찾는게 맞을까?
        //playerHero = Instance. new Ashe();
        playMode = PLAY_MODE.RTS_MODE;
        //캐릭터 출현 정보를 배열에 저장
        //캐릭터, 넥서스 랜덤 포인트에 생성

    }
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject playerObj = PhotonNetwork.Instantiate(DropDownManager.selectHeroName, heroPoints[MatchManager.masterIndexPoint].position, heroPoints[MatchManager.masterIndexPoint].rotation, 0);
            playerHero = playerObj.GetComponent<Hero>();

            GameObject firstNexus = this.buildingObjectPool.Pop();
            firstNexus.transform.position = buildPoints[MatchManager.masterIndexPoint].position;
            Mine = 1000;//디버깅용
            Population = 0;
            Gold = 0;
            MaxPopulation = 5;

            for (int i = 0; i < monsterSpawnPoints.Length; i++)
            {
                if(i >= 5)
                {
                    monster[i] = this.monsterObjectPool.Pop(1);
                }
                else
                {
                    monster[i] = this.monsterObjectPool.Pop(0);
                }
                monster[i].transform.position = monsterSpawnPoints[i].position;
                pv.RPC("OriginPos", RpcTarget.AllBuffered);
            }

            //PhotonNetwork.Instantiate("ShopObj", shopPoint.position, shopPoint.rotation); //상점 생정 
            //PhotonNetwork.Instantiate("Nexus", buildPoints[MatchManager.masterIndexPoint].position, buildPoints[MatchManager.masterIndexPoint].rotation, 0);
        }
        else
        {
            Mine = 1000;//디버깅용
            Population = 0;
            Gold = 0;
            MaxPopulation = 5;
            GameObject playerObj = PhotonNetwork.Instantiate(DropDownManager.selectHeroName, heroPoints[MatchManager.userIndexPoint].position, heroPoints[MatchManager.userIndexPoint].rotation, 0);
            playerHero = playerObj.GetComponent<Hero>();

            GameObject firstNexus = this.buildingObjectPool.Pop();
            firstNexus.transform.position = buildPoints[MatchManager.userIndexPoint].position;
            //PhotonNetwork.Instantiate("Nexus", buildPoints[MatchManager.userIndexPoint].position, buildPoints[MatchManager.userIndexPoint].rotation, 0);
        }
        //StartCoroutine(PlayerInitCo());


        for (int i = 0; i < 4; i++)
        {
            //Debug.Log(priorityQueue.Dequeue());
        }

        SceneManager.sceneLoaded += LoadedsceneEvent;
        //게임매니저를 메인메뉴로 빼야하는데
        //이미 많은분들이 하드참조를 해놔서
        //씬이동시에 초기화가 힘들어보임 
        //그래서 게임매니저를 메인게임에 그냥 둘거임
        EventInit();
        onRoundStart();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
    }

    [PunRPC]
    public void OriginPos()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for(int i = 0; i< monsterSpawnPoints.Length; i++)
            {
                monster[i].GetComponent<Monster>().OriginPos = monsterSpawnPoints[i].position;
                monster[i].GetComponent<PhotonView>().RPC("NavMeshEnable", RpcTarget.AllBuffered);
            }
           
        }

    }

}
