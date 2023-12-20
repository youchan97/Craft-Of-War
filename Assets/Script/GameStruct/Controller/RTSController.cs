using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using System;
using Unity.VisualScripting;

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
                UIManager.Instance.unitProductModeUI.SetActive(false);
                return;
            }



            UIManager.Instance.BottomRTSUISetActive(UIManager.Instance.unitProductModeUI);
            TextMeshProUGUI[] buildInfoTexts = UIManager.Instance.unitProductModeUI.GetComponentsInChildren<TextMeshProUGUI>();
            buildInfoTexts[0].text = selectBuilding.gameObject.GetComponent<Building>().buildingName;
            //전정보 없애주기
            buildInfoTexts[1].text = null;
            UIManager.Instance.buildProgressCountText.text = null;
            UIManager.Instance.buildProgressFill.fillAmount = 0;
            SelectBuilding.GetComponent<Building>().UIMatch();


        }
    }
    UnitController hero;
    Hero heroStat;

    private void Awake()
    {
        selectedUnitList = new List<UnitController>();
    }
    private void Start()
    {
        heroStat = GameManager.Instance.PlayerHero.GetComponent<Hero>(); 
        hero = GameManager.Instance.PlayerHero.GetComponent<UnitController>();
        GameManager.Instance.rtsController.fieldUnitList.Add(hero);
        InitArmyDic();
        armyMode = ARMYMOVEMODE.Square;
    }
    private void Update()
    {
        SelectUnitUI();
    }

    public void SelectUnitUI()
    {
        if (selectedUnitList.Count == 1)
        {
            if (selectedUnitList[0].gameObject == GameManager.Instance.PlayerHero.gameObject)
            {
                UIManager.Instance.heroHp.fillAmount = (float)heroStat.info.CurentHp/ (float)heroStat.info.MaxHp;
                UIManager.Instance.heroHpText.text = heroStat.info.curentHp +"/" + heroStat.info.MaxHp;
                UIManager.Instance.heroMp.fillAmount = (float)heroStat.CurMp / (float)heroStat.MaxMp;
                UIManager.Instance.heroMpText.text = heroStat.CurMp +"/" + heroStat.MaxMp;
                UIManager.Instance.heroExp.fillAmount = heroStat.CurExp / heroStat.AimExp;
                UIManager.Instance.heroExpText.text = heroStat.CurExp +"/" + heroStat.AimExp;
                UIManager.Instance.heroLvText.text = heroStat.Level.ToString();
                UIManager.Instance.heroAtkText.text = heroStat.info.Atk.ToString();
                UIManager.Instance.heroShieldText.text = heroStat.info.Def.ToString();
            }
            else
            {
                UnitController selectedUnit = selectedUnitList[0];
                UIManager.Instance.unitHp.fillAmount = (float)selectedUnit.unit.Hp / (float)100;
                UIManager.Instance.unitHpText.text = selectedUnitList[0].unit.Hp + "/" + 100;
                UIManager.Instance.unitAtkText.text = selectedUnitList[0].unit.info.Atk.ToString();
                UIManager.Instance.unitShieldText.text = selectedUnitList[0].unit.info.Def.ToString();
            }
        }
        else
        {
            for (int i = 0; i < selectedUnitList.Count; i++)
            {
                if (selectedUnitList[i] != null)
                {
                    if (selectedUnitList[0].gameObject == GameManager.Instance.PlayerHero.gameObject)
                    {
                        UIManager.Instance.characterSlotHp[0].fillAmount = (float)heroStat.info.CurentHp / (float)heroStat.info.MaxHp;
                    }
                    else
                    {
                        UIManager.Instance.characterSlotHp[i].fillAmount = (float)selectedUnitList[i].unit.Hp / (float)100;
                    }
                }
            }
        }
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
                supplyList[i].GetComponent<SupplyUnit>().isMineClicked = true;
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
        if(selectedUnitList.Count < 12)
        {
            newUnit.SelectUnit();
            selectedUnitList.Add(newUnit);
        }
    }
    private void DeselectUnit(UnitController newUnit)
    { 
        newUnit.DeselectUnit();
        selectedUnitList.Remove(newUnit);
        UIManager.Instance.characterSlot.gameObject.SetActive(false);
    }
    public void DeselectAll()
    {
        for (int i = 0; i < selectedUnitList.Count; i++)
        {
            UIManager.Instance.characterSlot.faceSlot[i].SetActive(false);
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
