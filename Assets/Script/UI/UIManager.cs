using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    [Header("모드 UI 오브젝트")]
    public GameObject aosUI;
    public GameObject rtsUI;
    [Header("모드 전환 오브젝트")]
    public ModTransitionSystem transitionSystem;
    [Header("자원 UI 오브젝트")]
    public TextMeshProUGUI treeText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI PopulationText;
    [Header("상점 관련 UI")]
    public GameObject shopUI;
    public GameObject shop;
    public GameObject leavingStore;
    public TextMeshProUGUI shopClosingTime;
    //빌드유닛 변수
    [Header("빌드 유닛 관련")]
    public Image buildProgressImg;
    public GameObject bottomInfoRTSUI;
    public GameObject buildingModeUI;

    public void ChangeMod()
    {
        if (transitionSystem != null)
        {
            if (transitionSystem.ModSwitch)
            {
                aosUI.SetActive(true);
                rtsUI.SetActive(false);
            }
            else
            {
                aosUI.SetActive(false);
                rtsUI.SetActive(true);
            }
        }
    }

    public void ResourcesUpdate()
    {
        treeText.text = GameManager.Instance.Tree.ToString();
        goldText.text = GameManager.Instance.Gold.ToString();
        PopulationText.text = GameManager.Instance.Population.ToString();
    }
    void Update()
    {
        ResourcesUpdate();
        ChangeMod();
        SetStoreUI();
    }

    public void SetStoreUI() // 상점 UI를 켜고 끄는 UI설정
    {
        if (shop.TryGetComponent(out ShopDetection shopDetection))
        {
            shopUI.SetActive(shopDetection.StoreUse);
            leavingStore.SetActive(!shopDetection.StoreUse);
            if (shop.TryGetComponent(out ShopController shopController))
                shopClosingTime.text = "남은시간 : " + shopController.CurCoolTime;
        }
    }
    public void CloseStore()
    {
        if (shop.TryGetComponent(out ShopDetection shopDetection))
        {
            shopDetection.StoreUse = false;
            leavingStore.SetActive(false);
            shopUI.SetActive(false);
        }
    }


    public void BottomRTSUISetActive(GameObject setTarget)
    {
        for (int i = 2; i < 6; i++)//4가지 경우 유아이 꺼주고 원하는것만 킬려고
        {
            bottomInfoRTSUI.transform.GetChild(i).gameObject.SetActive(false);
        }
        setTarget.SetActive(true);
    }

}
