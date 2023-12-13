using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum SLOTTYPE
{
    None,
    ProductBuilding,
    NexusBuilding,
    Supply_Build,
    SupplyUnit,
    ArmySelect,
}

public struct SlotArg//슬롯에 전달할 이미지와 기능
{
    public Sprite[] spriteArr;
    public Action[] actionButtonArr;
    public int slotsCount;
    public SlotArg(Sprite[] spriteArr, Action[] actionArr)
    {
        this.spriteArr = spriteArr;
        this.actionButtonArr = actionArr;
        slotsCount = spriteArr.Length;//슬롯 실제 쓸 갯수
    }
}
public class SlotManager : SingleTon<SlotManager>
{
    private Dictionary<SLOTTYPE, SlotArg> slotsDic = new Dictionary<SLOTTYPE, SlotArg>();
    private ButtonSlot[] slotArr = new ButtonSlot[slotsCount];
    //9칸으로 구성된 배열이여야함
    //버튼 아이콘 스프라이트들
    public Sprite[] supply_BuildIconArr;
    public Sprite[] supplyUnitIconArr;
    public Sprite[] productBuildingIconArr;
    public Sprite[] nexusIconArr;
    public Sprite[] armyMoveModeIconArr;

    public const int slotsCount = 9;
    //각 슬롯에 맞는 기능과 이미지 전달
    [SerializeField]
    private SLOTTYPE slotType;

    public SLOTTYPE SlotType
    {
        get { return slotType; }
        set 
        {
            //다른슬롯으로 전환시에 초기화해줌
            for (int i = 0; i < slotsCount; i++)//스프라이트 전달
            {
                slotArr[i].imageCompo.sprite = null;//+1은 부모오브젝트 이미지 컴포넌트 제외하기위해
                slotArr[i].slotAction = null;
            }
            
            slotType = value;
            for (int i = 0; i < slotsCount; i++)//스프라이트 전달
            {
                if(slotsDic[slotType].spriteArr[i] != null)
                slotArr[i].imageCompo.sprite = slotsDic[slotType].spriteArr[i];//+1은 부모오브젝트 이미지 컴포넌트 제외하기위해
                if (slotsDic[slotType].actionButtonArr[i] != null)
                slotArr[i].slotAction = slotsDic[slotType].actionButtonArr[i];
            }
        }
    }
    private void Start()
    {
        Init();
    }
    public void Init()//딕셔너리 초기화와 이미지,기능 연결부
    {
        slotArr = GetComponentsInChildren<ButtonSlot>();
        slotsDic.Add(SLOTTYPE.None, new SlotArg(new Sprite[9], new Action[slotsCount]));//빈 유아이
        slotsDic.Add(SLOTTYPE.ProductBuilding, new SlotArg(productBuildingIconArr, new Action[slotsCount]));
        slotsDic.Add(SLOTTYPE.NexusBuilding, new SlotArg(nexusIconArr, new Action[slotsCount]));
        slotsDic.Add(SLOTTYPE.SupplyUnit, new SlotArg(supplyUnitIconArr, new Action[slotsCount]));
        slotsDic.Add(SLOTTYPE.Supply_Build, new SlotArg(supply_BuildIconArr, new Action[slotsCount]));
        slotsDic.Add(SLOTTYPE.ArmySelect, new SlotArg(armyMoveModeIconArr, new Action[slotsCount]));
        ActionInit();
    }

    public void ActionInit()
    {
        //생산유닛 행동 액션
        slotsDic[SLOTTYPE.SupplyUnit].actionButtonArr[0] += () => { SlotType = SLOTTYPE.Supply_Build; };
        //빌드모드 행동 액션
        for (int i = 0; i < 4; i++)//빌딩 갯수
        {
            int index = i;
            slotsDic[SLOTTYPE.Supply_Build].actionButtonArr[index] += () =>
            {
                slotArr[index].targetObj = GameManager.Instance.buildingObjectPool.Pop(index);
                slotArr[index].meshRenderer = slotArr[index].targetObj.GetComponent<MeshRenderer>();
            };
        }
        //뒤로가기 처음 메뉴로
        slotsDic[SLOTTYPE.Supply_Build].actionButtonArr[8] += () => { SlotType = SLOTTYPE.SupplyUnit; };
        //넥서스빌딩 행동 액션
        //정찰,생산유닛
        UnitProductAction(SLOTTYPE.NexusBuilding,0,5);
        UnitProductAction(SLOTTYPE.NexusBuilding,1,6);

        //생산빌딩 행동 액션
        for (int i = 0; i < 5; i++)//유닛 갯수 오브젝트풀에서 앞의 5개만
        {
            int index = i;
            UnitProductAction(SLOTTYPE.ProductBuilding, index, index);
        }
        //부대지정 행동 액션
        slotsDic[SLOTTYPE.ArmySelect].actionButtonArr[0] += () => { RTSController.armyMode = ARMYMOVEMODE.Horizontal; };
        slotsDic[SLOTTYPE.ArmySelect].actionButtonArr[1] += () => { RTSController.armyMode = ARMYMOVEMODE.Vertical; };
        slotsDic[SLOTTYPE.ArmySelect].actionButtonArr[2] += () => { RTSController.armyMode = ARMYMOVEMODE.Square; };

        //마지막 부분
        //게임시작에는 행동슬롯으로 고정
        SlotType = SLOTTYPE.None;
    }

    void UnitProductAction(SLOTTYPE slotType, int buttonIndex, int popIndex)
    {
        slotsDic[slotType].actionButtonArr[buttonIndex] += () =>
        {
            StartCoroutine(UnitCoolTimeCo(popIndex));
        };
    }

    IEnumerator UnitCoolTimeCo(int popIndex)
    {
        float cool = GameManager.Instance.unitObjectPool.Peek(popIndex).GetComponent<Unit>().coolTime;
        //쿨타임동안 다른걸 선택할수 있어 미리 위치받음
        Transform selectBuilding = GameManager.Instance.rtsController.SelectBuilding.gameObject.transform;
        TextMeshProUGUI[] buildInfoTexts  = UIManager.Instance.buildingModeUI.GetComponentsInChildren<TextMeshProUGUI>();
        //진행도
        buildInfoTexts[1].text = "건설중";
        //유닛생산 쿨타임
        while (cool > 0f)
        {
            cool -= Time.fixedDeltaTime;
            UIManager.Instance.buildProgressImg.fillAmount = (1 / cool) - cool/10;
            yield return new WaitForFixedUpdate();
        }
        buildInfoTexts[1].text = null;
        UIManager.Instance.buildProgressImg.fillAmount = 0;

        GameObject unit = GameManager.Instance.unitObjectPool.Pop(popIndex).gameObject;
        GameManager.Instance.rtsController.fieldUnitList.Add(unit.GetComponent<UnitController>());
        unit.transform.position = selectBuilding.position;
        unit.transform.Translate(new Vector3(0, 0, -6), Space.Self);
        unit.GetComponent<NavMeshAgent>().enabled = true;

        //유닛뽑고 겹치지 않기 위해 이동,
        unit.GetComponent<NavMeshAgent>().SetDestination(unit.transform.position + new Vector3(0, 0, -3));
        yield return null;
        
        unit.GetComponent<NavMeshAgent>().ResetPath();
    }
}
