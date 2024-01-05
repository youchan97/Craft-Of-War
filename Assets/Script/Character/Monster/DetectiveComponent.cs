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
    public Collider[] targetCols;

    
    [SerializeField] private float detectiveRange; // 감지 범위(시야보다 클 수 없음)

    public Vector3 LastDetectivePos // 감지된 오브젝트 위치
    {  get; private set; }

    public float DetectiveRange { get => detectiveRange; set=> detectiveRange = value; }

    public bool IsRangeDetection { get { return isRangeDetection; } }

    private void Awake()
    {
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
        Detect();
    }

    void Detect()
    {
        targetCols = Physics.OverlapSphere(transform.position, detectiveRange, targetLayer);
        if (this.gameObject.GetComponent<BattleUnit>() != null && this.gameObject.GetComponent<BattleUnit>().unitType == BATTLE_UNIT.Healer)
            firstIndex = 1;
        else
            firstIndex = 0;

        isRangeDetection = (bool)(targetCols.Length > firstIndex);
        if (isRangeDetection)
        {
            RaycastHit hit;
            Vector3 dir = ((targetCols[firstIndex].transform.position) - transform.position).normalized;

            if (TryGetComponent(out DefenseBuilding defense) == false)
            {
                transform.forward = dir;
            }
            if (Physics.Raycast(transform.position, dir, out hit, detectiveRange))
            {
                LastDetectivePos = hit.transform.position;
            }
        }
    }
    [PunRPC]
    public void DetectLayer()
    {
        if(TryGetComponent(out DefenseBuilding def))
        {
            if(PhotonNetwork.IsMasterClient)
            {
                if (pv.IsMine)
                    targetLayer = 1 << 7;
                else
                    targetLayer = 1 << 6;
            }
            else
            {
                if (pv.IsMine)
                    targetLayer = 1 << 6;
                else
                    targetLayer = 1 << 7;
            }
            return;
        }

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
