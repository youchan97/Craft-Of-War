using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDetection : MonoBehaviour
{
    [SerializeField]
    private float detectiveRadius; // 히어로 탐지 범위
    [SerializeField]
    private LayerMask unitLayer; // 유닛 레이어
    [SerializeField]
    private bool storeAvailability; // 상점을 이용가는한지 여부
    public bool StoreAvailability
    {
        get => storeAvailability;
        set { storeAvailability = value; }
    }
    private bool shopStop;
    private void Start()
    {
        shopStop =  GetComponent<ShopController>().ShopStop;
    }
    private void Update()
    {
        StoreAvailability = (IsDetection() && shopStop && GameManager.Instance.playMode == PLAY_MODE.AOS_MODE);
    }
    public bool IsDetection()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, detectiveRadius, unitLayer);
        if (cols.Length > 0)
        {
            if (cols[0].gameObject.tag == ("PlayerHero"))
                return true;
            else
                return false;
        }
        else
            return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectiveRadius);
    }
}
