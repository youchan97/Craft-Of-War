using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectiveComponent : MonoBehaviour
{
    LayerMask targetLayer;
    [SerializeField] bool isRangeDetection;

    public bool isCutomTargetLayer;
    public Collider[] cols;

    
    [SerializeField] private float detectiveRange; // 감지 범위(시야보다 클 수 없음)

    public Vector3 LastDetectivePos // 감지된 오브젝트 위치
    {  get; private set; }

    public float DetectiveRange { get => detectiveRange; set=> detectiveRange = value; }

    public bool IsRangeDetection { get { return isRangeDetection; } }

    private void Awake()
    {
        if(isCutomTargetLayer)
        {
            targetLayer = (1 << 17) | (1 << 18);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                targetLayer = (1 << 7) | (1 << 13) | (1 << 18);
            }
            else
                targetLayer = (1 << 6) | (1 << 12) | (1 << 17);
        }
    
    }


    private void Update()
    {
        cols = Physics.OverlapSphere(transform.position, detectiveRange, targetLayer);
        isRangeDetection = (bool)(cols.Length > 0);

        if(isRangeDetection)
        {
            RaycastHit hit;
            int index = 0;

            while (cols[index] == null)
            {
                index++;
                continue;
            }    

            Vector3 dir = ((cols[index].transform.position) - transform.position).normalized;
            
            if(Physics.Raycast(transform.position,dir,out hit,detectiveRange))
            {
                LastDetectivePos = hit.transform.position;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectiveRange);
    }

}
