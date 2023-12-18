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
    public GameObject leavingShop;
    public TextMeshProUGUI shopClosingTime;
    [Header("캐릭터 슬룻 UI")]
    public CharacterSlot characterSlot;

    //빌드유닛 변수
    [Header("빌드 유닛 관련")]
    public Image buildProgressFill;
    public TextMeshProUGUI buildProgressCountText;
    public GameObject bottomInfoRTSUI;
    public GameObject unitProductModeUI;

    [Header("영웅 AOS모드 UI")]
    public SkillSlot[] skillSlots;
    public Image heroImg;

    [Header("종민// 유닛 여럿생산 관련 아이콘/배열")]
    public SlotManager slotManager;
    public UnitFaceListUI unitFaceUI;

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
        if (GameManager.Instance.playMode == PLAY_MODE.AOS_MODE)
            heroImg.sprite = GameManager.Instance.PlayerHero.HeroImage;
    }
    public void CloseStore()
    {
        leavingShop.SetActive(false);
        shopUI.SetActive(false);
    }


    public void BottomRTSUISetActive(GameObject setTarget)
    {
        for (int i = 2; i < 6; i++)//4가지 경우 유아이 꺼주고 원하는것만 킬려고
        {
            bottomInfoRTSUI.transform.GetChild(i).gameObject.SetActive(false);
        }
        setTarget.SetActive(true);
    }

    public void OnClickSkillSlot(int index)
    {
        skillSlots[index].TrySkillActive();
    }

}