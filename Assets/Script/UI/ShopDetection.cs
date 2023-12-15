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
    private bool shopAvailability; // 상점을 이용가는한지 여부
    public bool ShopAvailability
    {
        get => shopAvailability;
        set 
        { 
            shopAvailability = value; 
            if(!shopAvailability)
            {
                UIManager.Instance.leavingShop.SetActive(true);
            }
        }
    }
    private bool shopUse;
    public bool ShopUse
    {
        get { return shopUse; }
        set 
        { 
            shopUse = value;
            UIManager.Instance.shopUI.SetActive(shopUse);
        }
    }

    public ShopController shopController;
    private void Start()
    {
        shopController =  GetComponent<ShopController>();
    }
    private void Update()
    {
        Debug.Log("상점 이용가능" + ShopAvailability);
        ShopAvailability = (IsDetection() && shopController.ShopStop && GameManager.Instance.playMode == PLAY_MODE.AOS_MODE);
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
