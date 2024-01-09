using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectiveComponent : MonoBehaviourPunCallbacks
{
    public LayerMask targetLayer; //타겟의 레이어를 담는다
    [SerializeField] bool isRangeDetection; // 탐지 현황
    PhotonView pv;
    public int firstIndex; // 탐지의 조건
    public bool isCutomTargetLayer; //몬스터는 유닛과 다른 레이어마스크를 담음
    public Collider[] targetCols; // 조건의 맞게 탐지된 콜라이더들
    [SerializeField] private float detectiveRange; // 감지 범위(시야보다 클 수 없음)

    public Vector3 LastDetectivePos // 감지된 오브젝트 위치
    {  get; private set; }

    public float DetectiveRange { get => detectiveRange; set=> detectiveRange = value; }

    public bool IsRangeDetection { get { return isRangeDetection; } }

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if(isCutomTargetLayer) //몬스터
        {
            //빌딩,히어로,유닛 탐지
            targetLayer = (1 << 17) | (1 << 18);
        }
        else
        {
            //RPC를 통해 서로의 타겟 레이어 정보를 전달한다
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
        if (this.gameObject.GetComponent<BattleUnit>() != null && 
            this.gameObject.GetComponent<BattleUnit>().unitType == BATTLE_UNIT.Healer)
            firstIndex = 1;  //힐러는 자기 자신을 제외한 다른 유닛이 있어야함
        else
            firstIndex = 0;

        isRangeDetection = (bool)(targetCols.Length > firstIndex); //탐지 유무
        if (isRangeDetection)
        {
            RaycastHit hit;
            // 처음 탐지된 곳으로 시야 방향 설정
            Vector3 dir = ((targetCols[firstIndex].transform.position) - transform.position).normalized;

            if (TryGetComponent(out DefenseBuilding defense) == false)
            {
                //탐지된 것이 방어타워가 아닐 시 시야 설정
                transform.forward = dir;
            }
            if (Physics.Raycast(transform.position, dir, out hit, detectiveRange))
            {
                //몬스터의 이동을 위한 위치정보 담기
                LastDetectivePos = hit.transform.position;
            }
        }
    }
    [PunRPC]
    public void DetectLayer()
    {
        if(TryGetComponent(out DefenseBuilding def)) // 방어 타워 타겟레이어
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
        if (this.gameObject.layer == 6) // 마스터 와 일반 유저의 타겟레이어 설정 (상대의 소유들)
        {
            if (this.gameObject.GetComponent<BattleUnit>() != null && 
                this.gameObject.GetComponent<BattleUnit>().unitType == BATTLE_UNIT.Healer) //힐러는 자신의 유닛만 탐지한다.
                targetLayer = 1 << 6;
            else
                targetLayer = (1 << 7) | (1 << 13) | (1 << 18);
        }
        else
        {
            if (this.gameObject.GetComponent<BattleUnit>() != null && 
                this.gameObject.GetComponent<BattleUnit>().unitType == BATTLE_UNIT.Healer)
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
