using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectiveComponent : MonoBehaviourPunCallbacks
{
    public LayerMask targetLayer;
    [SerializeField] bool isRangeDetection;
    PhotonView pv;
    public int firstIndex;

    public bool isCutomTargetLayer;
    public Collider[] cols;

    
    [SerializeField] private float detectiveRange; // 감지 범위(시야보다 클 수 없음)

    public PriorityQueue<string, int> AdaptpriorityQueue;
    public IPrioxyQueue<string, int> priorityQueue;

    public Vector3 LastDetectivePos // 감지된 오브젝트 위치
    {  get; private set; }

    public float DetectiveRange { get => detectiveRange; set=> detectiveRange = value; }

    public bool IsRangeDetection { get { return isRangeDetection; } }

    private void Awake()
    {
        PriorityQueueInit();
        pv = GetComponent<PhotonView>();
        if(isCutomTargetLayer)
        {
            //빌딩,히어로,유닛 탐지
            targetLayer = (1 << 17) | (1 << 18);
        }
        else
        {
            pv.RPC("DetectLayer", RpcTarget.AllBuffered);
        }
    
    }


    private void Update()
    {
        cols = Physics.OverlapSphere(transform.position, detectiveRange, targetLayer);
        if (this.gameObject.GetComponent<BattleUnit>() != null && this.gameObject.GetComponent<BattleUnit>().unitType == BATTLE_UNIT.Healer)
            firstIndex = 1;
        else
            firstIndex = 0;

        isRangeDetection = (bool)(cols.Length > firstIndex);
        if (isRangeDetection)
        {
            RaycastHit hit;

            Vector3 dir = ((cols[firstIndex].transform.position) - transform.position).normalized;
            transform.forward = dir;
            if (Physics.Raycast(transform.position, dir, out hit, detectiveRange))
            {
                LastDetectivePos = hit.transform.position;
            }
        }
    }


    public void PriorityQueueInit()
    {
        AdaptpriorityQueue = new PriorityQueue<string, int>();
        priorityQueue = AdaptpriorityQueue;
    }

    public void AttackMethod()
    {

        StartCoroutine(AttackCO());
    }

    IEnumerator AttackCO()
    {
        yield return new WaitForSeconds(0.5f);

        if (cols.Length <= 0)
            yield break;

        List<GameObject> monsters = new List<GameObject>();
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.GetComponent<IHitAble>() != null)
            {
                priorityQueue.Enqueue(cols[i].gameObject.name, cols[i].gameObject.GetComponent<IHitAble>().Priority);
                Debug.LogError(cols[i].gameObject.name + " 디버그 " + cols[i].gameObject.GetComponent<IHitAble>().Priority);
                monsters.Add(cols[i].gameObject);
            }
        }
        string name = priorityQueue.Dequeue();
        foreach (GameObject monster in monsters)
        {
            if (monster.name == name)
            {
                monster.GetComponent<IHitAble>().Hp -= this.gameObject.GetComponent<IAttackAble>().Atk;
                break;
            }
        }
        Debug.LogError("흐름");
        priorityQueue.Clear();
    }
    
    public void HealMethod()
    {
        if (cols.Length <= 0)
            return;
        else
        {
            for(int i = 0; i< cols.Length; i++)
            {
                if (cols[i].GetComponent<Character>() != null && cols[i].GetComponent<Character>().Hp < 50)
                {
                    cols[i].GetComponent<Character>().Hp += this.gameObject.GetComponent<IAttackAble>().Atk;
                    Debug.Log(cols[0].name + cols[0].GetComponent<Character>().Hp + "힐");
                }
            }
        }
    }

    [PunRPC]
    public void DetectLayer()
    {
        if (this.gameObject.layer == 6)
        {
            if (this.gameObject.GetComponent<BattleUnit>() != null && this.gameObject.GetComponent<BattleUnit>().unitType == BATTLE_UNIT.Healer)
                targetLayer = 1 << 6;
            else
                targetLayer = (1 << 7) | (1 << 13) | (1 << 18);
        }
        else
        {
            if (this.gameObject.GetComponent<BattleUnit>() != null && this.gameObject.GetComponent<BattleUnit>().unitType == BATTLE_UNIT.Healer)
                targetLayer = 1 << 7;
            else
                targetLayer = (1 << 6) | (1 << 12) | (1 << 17);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectiveRange);
    }

}
