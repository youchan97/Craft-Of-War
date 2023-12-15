using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using System;

public enum ARMYMOVEMODE
{
    None,
    Horizontal,
    Vertical,
    Square,
}

public class RTSController : MonoBehaviour
{
    public Dictionary<ARMYMOVEMODE, Func<List<Vector3>,Vector3, List<Vector3>>> armyMoveModeDic;

    public static ARMYMOVEMODE armyMode;

    public List<UnitController> selectedUnitList; // 플레이어가 클릭 혹은 드래그로 선택한 유닛
    public List<UnitController> fieldUnitList = new List<UnitController>();
    private BuildingController selectBuilding;
    public BuildingController SelectBuilding
    {
        get => selectBuilding;
        set
        {
            selectBuilding = value;
            if (value == null)
            {
                UIManager.Instance.buildingModeUI.SetActive(false);
                return;
            }

            UIManager.Instance.BottomRTSUISetActive(UIManager.Instance.buildingModeUI);
            TextMeshProUGUI[] buildInfoTexts = UIManager.Instance.buildingModeUI.GetComponentsInChildren<TextMeshProUGUI>();
            //이름
            buildInfoTexts[0].text = selectBuilding.gameObject.GetComponent<Building>().buildingName;
            //건설중인지
            buildInfoTexts[1].text = null;
        }
    }
    UnitController hero;

    private void Awake()
    {
        selectedUnitList = new List<UnitController>();
    }
    private void Start()
    {
        hero = GameManager.Instance.PlayerHero.GetComponent<UnitController>();
        GameManager.Instance.rtsController.fieldUnitList.Add(hero);
        InitArmyDic();
        armyMode = ARMYMOVEMODE.Square;
    }

    public void ClickSelectUnit(UnitController newUnit)
    {
        DeselectAll();
        SelectUnit(newUnit);
    }
    public void ShiftClickSelectUnit(UnitController newUnit)
    {
        if(selectedUnitList.Contains(newUnit))
        {
            DeselectUnit(newUnit);
        }
        else
        {
            SelectUnit(newUnit);
        }
    }
    public void DragSelectUnit(UnitController newUnit)
    {
        if(!selectedUnitList.Contains(newUnit))
        {
            SelectUnit(newUnit);
        }
    }

    public void InitArmyDic()
    {
        armyMoveModeDic = new Dictionary<ARMYMOVEMODE, Func<List<Vector3>, Vector3, List<Vector3>>>();
        armyMoveModeDic.Add(ARMYMOVEMODE.Horizontal, new Func<List<Vector3>, Vector3, List<Vector3>>
        ((list, firstPoint) =>
        {
            for (int i = 0; i < selectedUnitList.Count; i++)
            {
                list.Add(firstPoint + new Vector3(i * 4, 0, 0));
            }
            return list;
        }));
        armyMoveModeDic.Add(ARMYMOVEMODE.Vertical, new Func<List<Vector3>, Vector3, List<Vector3>>
        ((list, firstPoint) =>
        {
            for (int i = 0; i < selectedUnitList.Count; i++)
            {
                list.Add(firstPoint + new Vector3(0, 0, i * 4));
            }
            return list;
        }));
        armyMoveModeDic.Add(ARMYMOVEMODE.Square, new Func<List<Vector3>, Vector3, List<Vector3>>
        ((list,firstPoint) => 
        {
            for (int i = 0; i < selectedUnitList.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    list.Add(firstPoint + new Vector3(i * 4, 0, j * 4));
                }
            }
            return list;
        }));
    }

    public void MoveSelectedUnits(RaycastHit hit)
    {
        Debug.Log("선택된 유닛 이동");

        List<Vector3> vecList = new List<Vector3>();

        vecList = armyMoveModeDic[armyMode](vecList,hit.point);

        for (int i = 0; i < selectedUnitList.Count; i++)
        {
            selectedUnitList[i].MoveTo(vecList[i]);
        }

        //크리스탈 닿았을때
        if (hit.collider.gameObject.layer == 15)
        {
            List<UnitController> supplyList = SelectedSupplyUnitList();
            for (int i = 0; i < supplyList.Count; i++)
            {
                supplyList[i].GetComponent<SupplyUnit>().IsMineClicked = true;
                supplyList[i].GetComponent<SupplyUnit>().mineTf = hit.point;
            }
        }

    }

    //선택유닛중 공급유닛만 뽑는코드
    List<UnitController> SelectedSupplyUnitList()
    {
        List<UnitController> list = new List<UnitController>();
        foreach (UnitController unit in selectedUnitList)
        {
            if (unit.TryGetComponent(out SupplyUnit supplyUnit))
                list.Add(unit);
        }
        return list;
    }

    private void SelectUnit(UnitController newUnit)
    {
        newUnit.SelectUnit();
        selectedUnitList.Add(newUnit);
    }
    private void DeselectUnit(UnitController newUnit)
    { 
        newUnit.DeselectUnit();
        selectedUnitList.Remove(newUnit);
    }
    public void DeselectAll()
    {
        for (int i = 0; i < selectedUnitList.Count; i++)
        {
            selectedUnitList[i].DeselectUnit();
        }
        selectedUnitList.Clear();

    }
    public void DeselctBuliding()
    {
        if (SelectBuilding == null)
            return;
        SelectBuilding.DeselectBuliding();
        SelectBuilding = null;
    }
}
