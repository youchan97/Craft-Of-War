using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NexusBuilding : Building, IProductAble
{
    //커스텀 자료구조
    public UnitList spawnList;
    //대기열 리스트
    public List<IEnumerator> unitCoolTimeCos;
    //대기열 관리 코루틴
    public IEnumerator unitProductManagerCo;
    public override void Die()
    {
        if (photonView.IsMine)
        {
            UIManager.Instance.result.SetActive(true);
            UIManager.Instance.defeat.SetActive(true);
        }
        else
        {
            UIManager.Instance.result.SetActive(true);
            UIManager.Instance.win.SetActive(true);
        }
    }

    public override void Hit()
    {
    }

    public new void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(unitProductManagerCo);
        GameManager.Instance.NexusCount++;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(unitProductManagerCo);
        GameManager.Instance.NexusCount--;
    }
    public override void Awake()
    {
        base.Awake();
        Init();
    }
    private void Init()
    {
        spawnList = new UnitList();
        spawnList.OnAddUnit += (Unit unit) => ProductUIMatch();
        spawnList.OnRemoveUnit += (int index) => ProductUIMatch();
        unitCoolTimeCos = new List<IEnumerator>();
        unitProductManagerCo = UnitProductManagerCo();
    }
    //호출이 될때 실질적인 생산을 하는 인터페이스 함수
    public void Production(int popIndex, Transform selectBuildingTf, Unit targetUnit)
    {
        //5개의 대기열일때
        if (unitCoolTimeCos.Count >= 5)
            return;
        //비용 모자랄때
        if (targetUnit.cost > GameManager.Instance.Mine)
            return;
        //인구수 초과 했을때
        if (GameManager.Instance.MaxPopulation <= GameManager.Instance.Population)
            return;

        spawnList.Add(targetUnit);
        unitCoolTimeCos.Add(UnitCoolTimeCo(popIndex, selectBuildingTf, this));
        GameManager.Instance.Mine -= targetUnit.cost;
        GameManager.Instance.Population++;
    }

    public IEnumerator UnitProductManagerCo()
    {
        while (true)
        {
            if (unitCoolTimeCos.Count > 0)
            {
                IEnumerator currentCo = unitCoolTimeCos[0];
                yield return StartCoroutine(currentCo);
                unitCoolTimeCos.RemoveAt(0);
                spawnList.RemoveAt(0);
            }
            yield return null;
        }
    }
    public IEnumerator UnitCoolTimeCo(int popIndex, Transform selectBuildingTf, Building building)
    {
        float cool = GameManager.Instance.unitObjectPool.Peek(popIndex).GetComponent<Unit>().coolTime;

        //쿨타임동안 다른걸 선택할수 있어 미리 위치받음
        TextMeshProUGUI[] buildInfoTexts = UIManager.Instance.unitProductModeUI.GetComponentsInChildren<TextMeshProUGUI>();
        //유닛생산 쿨타임
        while (cool > 0f)
        {
            cool -= Time.fixedDeltaTime;

            if (GameManager.Instance.rtsController.SelectBuilding != null)
            {
                if (building == GameManager.Instance.rtsController.SelectBuilding.GetComponent<Building>())
                {
                    buildInfoTexts[1].text = "생산중";
                    UIManager.Instance.buildProgressCountText.text = cool.ToString();
                    UIManager.Instance.buildProgressFill.fillAmount = (1 / cool) - cool / 10;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        buildInfoTexts[1].text = null;
        UIManager.Instance.buildProgressCountText.text = null;
        UIManager.Instance.buildProgressFill.fillAmount = 0;

        //생산부 유닛
        GameObject unit = GameManager.Instance.unitObjectPool.Pop(popIndex).gameObject;
        GameManager.Instance.rtsController.fieldUnitList.Add(unit.GetComponent<UnitController>());
        unit.transform.position = selectBuildingTf.position;
        unit.transform.Translate(new Vector3(0, 0, -6), Space.Self);
        unit.GetComponent<NavMeshAgent>().enabled = true;


        //유닛뽑고 겹치지 않기 위해 이동,
        unit.GetComponent<NavMeshAgent>().SetDestination(unit.transform.position + new Vector3(0, 0, -3));
        yield return null;

        unit.GetComponent<NavMeshAgent>().ResetPath();

    }
    public void ProductUIMatch()
    {
        for (int i = 0; i < SlotManager.Instance.unitProductProgressFaceSlots.Count; i++)
        {
            if (GameManager.Instance.rtsController.SelectBuilding != null)
            {
                if (this == GameManager.Instance.rtsController.SelectBuilding.GetComponent<Building>())
                {
                    if (i < spawnList.Count)
                    {
                        SlotManager.Instance.unitProductProgressFaceSlots[i].SetActive(true);
                        SlotManager.Instance.unitProductProgressFaceSlots[i].GetComponent<Image>().sprite = spawnList[i].faceSprite;
                    }
                    else
                    {
                        SlotManager.Instance.unitProductProgressFaceSlots[i].SetActive(false);
                        SlotManager.Instance.unitProductProgressFaceSlots[i].GetComponent<Image>().sprite = null;
                    }
                }
            }
        }
    }
}
