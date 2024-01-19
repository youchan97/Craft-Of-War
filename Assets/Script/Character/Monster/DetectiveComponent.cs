using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectiveComponent : MonoBehaviourPunCallbacks
{
    public LayerMask targetLayer; //Ÿ���� ���̾ ��´�
    [SerializeField] bool isRangeDetection; // Ž�� ��Ȳ
    PhotonView pv;
    public int firstIndex; // Ž���� ����
    public bool isCutomTargetLayer; //���ʹ� ���ְ� �ٸ� ���̾��ũ�� ����
    public Collider[] targetCols; // ������ �°� Ž���� �ݶ��̴���
    [SerializeField] private float detectiveRange; // ���� ����(�þߺ��� Ŭ �� ����)

    public Vector3 LastDetectivePos // ������ ������Ʈ ��ġ
    {  get; private set; }

    public float DetectiveRange { get => detectiveRange; set=> detectiveRange = value; }

    public bool IsRangeDetection { get { return isRangeDetection; } }

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if(isCutomTargetLayer) //����
        {
            //����,�����,���� Ž��
            targetLayer = Layers.HeroLayer;
        }
        else
        {
            //RPC�� ���� ������ Ÿ�� ���̾� ������ �����Ѵ�
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
            firstIndex = 1;  //������ �ڱ� �ڽ��� ������ �ٸ� ������ �־����
        else
            firstIndex = 0;

        isRangeDetection = (bool)(targetCols.Length > firstIndex); //Ž�� ����
        if (isRangeDetection)
        {
            RaycastHit hit;
            // ó�� Ž���� ������ �þ� ���� ����
            Vector3 dir = ((targetCols[firstIndex].transform.position) - transform.position).normalized;

            if (TryGetComponent(out DefenseBuilding defense) == false)
            {
                //Ž���� ���� ���Ÿ���� �ƴ� �� �þ� ����
                transform.forward = dir;
            }
            if (Physics.Raycast(transform.position, dir, out hit, detectiveRange))
            {
                //������ �̵��� ���� ��ġ���� ���
                LastDetectivePos = hit.transform.position;
            }
        }
    }
    [PunRPC]
    public void DetectLayer()
    {
        if(TryGetComponent(out DefenseBuilding def)) // ��� Ÿ�� Ÿ�ٷ��̾�
        { 
            if(PhotonNetwork.IsMasterClient)
            {
                if (pv.IsMine)
                    targetLayer = Layers.userUnit;
                else
                    targetLayer = Layers.masterUnit;
            }
            else
            {
                if (pv.IsMine)
                    targetLayer = Layers.masterUnit;
                else
                    targetLayer = Layers.userUnit;
            }
            return;
        }
        if (this.gameObject.layer == 6) // ������ �� �Ϲ� ������ Ÿ�ٷ��̾� ���� (����� ������)
        {
            if (TryGetComponent(out BattleUnit battleUnit) && battleUnit.unitType == BATTLE_UNIT.Healer) //������ �ڽ��� ���ָ� Ž���Ѵ�.
                targetLayer = Layers.masterUnit;
            else
                targetLayer = Layers.userLayer;
        }
        else
        {
            if (TryGetComponent(out BattleUnit battleUnit) && battleUnit.unitType == BATTLE_UNIT.Healer)
                targetLayer = Layers.userUnit;
            else
                targetLayer = Layers.masterLayer;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectiveRange);
    }

}
