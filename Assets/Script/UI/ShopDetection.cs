using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDetection : MonoBehaviour
{
    [SerializeField]
    private float detectiveRadius; // 히어로 탐지 범위
    [SerializeField]
    private LayerMask masterHeroLayer; // 유닛 레이어
    [SerializeField]
    private LayerMask userHeroLayer;
    [SerializeField]
    private bool shopAvailability; // 상점을 이용가는한지 여부
    public bool ShopAvailability
    {
        get => shopAvailability;
        set 
        { 
            shopAvailability = value;
            UIManager.Instance.shopAvailability = ShopAvailability;
            if (ShopUse && ShopAvailability == false)
            {
                UIManager.Instance.shopMessage.SetActive(true);
                UIManager.Instance.shopMessageText.text = "상점이 떠났습니다.";
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
            if (ShopUse)
            {
                UIManager.Instance.shopUI.SetActive(true);
            }
        }
    }

    public ShopController shopController;
    private void Start()
    {
        shopController =  GetComponent<ShopController>();
    }
    private void Update()
    {
        ShopAvailability = (IsDetection() && shopController.ShopStop && GameManager.Instance.playMode == PLAY_MODE.AOS_MODE);
        Debug.Log("게임모드"+GameManager.Instance.playMode);
        Debug.Log("상점이 멈춤"+shopController.ShopStop);
        Debug.Log("상점이용" + ShopAvailability);
    }
    public bool IsDetection()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, detectiveRadius, masterHeroLayer | userHeroLayer);
        if (cols.Length > 0)
        {
            Debug.Log("영웅 발견");
            return true;
        }

        else
        {
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectiveRadius);
    }
}
