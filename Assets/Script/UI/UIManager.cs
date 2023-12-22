using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    [Header("결과 UI")]
    public GameObject result;
    public GameObject win;
    public GameObject defeat;
    [Header("모드 UI 오브젝트")]
    public GameObject aosUI;
    public GameObject rtsUI;
    [Header("모드 전환 오브젝트")]
    public ModTransitionSystem transitionSystem;
    [Header("자원 UI 오브젝트")]
    public TextMeshProUGUI mineText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI PopulationText;
    [Header("상점 관련 UI")]
    public GameObject shopUI;
    public GameObject shopMessage;
    public TextMeshProUGUI shopMessageText;
    public TextMeshProUGUI shopClosingTime;
    public bool shopAvailability;
    [Header("캐릭터 슬룻 UI")]
    public CharacterSlot characterSlot;
    public Image[] characterSlotHp;
    [Header("일반 유닛 정보 UI")]
    public GameObject unitStatUI;
    public Image unitHp;
    public TextMeshProUGUI unitHpText;
    public TextMeshProUGUI unitNameText;
    public TextMeshProUGUI unitAtkText;
    public TextMeshProUGUI unitShieldText;
    [Header("영웅 유닛 정보 UI")]
    public GameObject heroStatUI;
    public Image heroHp; // 최소 HP/최대 HP
    public TextMeshProUGUI heroHpText;
    public Image heroMp; // 최소 MP/최대MP
    public TextMeshProUGUI heroMpText;
    public Image heroExp;
    public TextMeshProUGUI heroLvText;
    public TextMeshProUGUI heroExpText; //현재 Exp / 최대 Exp
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI heroAtkText;
    public TextMeshProUGUI heroShieldText;
    [Header("AOS 영웅 유닛 정보UI")]
    public Image aosHeroHp;
    public TextMeshProUGUI aosHeroHpText;
    public Image aosHeroMp;
    public TextMeshProUGUI aosHeroMpText;


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
    void Update()
    {
        ChangeMod();
        if (GameManager.Instance.playMode == PLAY_MODE.AOS_MODE)
            heroImg.sprite = GameManager.Instance.PlayerHero.HeroImage;
        AosHeroInfo();
    }
    public void AosHeroInfo()
    {
        if(GameManager.Instance.playMode == PLAY_MODE.AOS_MODE)
        {
            aosHeroHp.fillAmount = (float)GameManager.Instance.PlayerHero.info.CurentHp / (float)GameManager.Instance.PlayerHero.info.MaxHp;
        }
    }

    public void ResultExit()
    {
        //결과 창 버튼
    }

    public void CloseShop()
    {
        if(shopAvailability == false)
        {
            shopMessage.SetActive(false);
            shopUI.SetActive(false);
        }
        else
        {
            shopMessage.SetActive(false);
        }
    }
    public void ExitShop()
    {
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

   

    
   // 1개만 선택시 에는 추가 해야됨. 
    public void SelectUnitUIChange(RTSController controller)
    {
        if (controller.selectedUnitList.Count == 1)
        {
            if (controller.selectedUnitList[0].gameObject == GameManager.Instance.PlayerHero.gameObject)
            {
                heroStatUI.SetActive(true);
                unitStatUI.SetActive(false);
                Hero hero = controller.selectedUnitList[0].GetComponent<Hero>();
                heroNameText.text = hero.name;                            
            }
            else
            {
                heroStatUI.SetActive(false);
                unitStatUI.SetActive(true); 
                unitNameText.text = controller.selectedUnitList[0].name;
                unitAtkText.text = controller.selectedUnitList[0].unit.info.Atk.ToString();
                unitShieldText.text = controller.selectedUnitList[0].unit.info.Def.ToString();
            }
        }
        else
        {
            heroStatUI.SetActive(false);
            unitStatUI.SetActive(false);
            for (int i = 0; i < controller.selectedUnitList.Count; i++)
            {
                if(controller.selectedUnitList[i].gameObject == GameManager.Instance.PlayerHero.gameObject)
                {
                    Swap(controller, 0, i);
                }
            }
            for (int i = 0; i < controller.selectedUnitList.Count; i++)
            {
                characterSlot.faceSlot[i].SetActive(true);
                if (controller.selectedUnitList[0].gameObject == GameManager.Instance.PlayerHero.gameObject)
                {
                    Hero hero = controller.selectedUnitList[0].GetComponent<Hero>();
                    characterSlot.faceImage[0].sprite = hero.HeroImage;
                }
                else
                {
                    characterSlot.faceImage[i].sprite = controller.selectedUnitList[i].unit.faceSprite;
                }
            }        
        }
    }
    public void Swap(RTSController controller, int aValue, int bValue)
    {
        UnitController temp = controller.selectedUnitList[aValue];
        controller.selectedUnitList[aValue] = controller.selectedUnitList[bValue];
        controller.selectedUnitList[bValue] = temp;
    }
    
}