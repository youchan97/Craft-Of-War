using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;


public enum BuildType
{
    None,
    Product,
    Defends,
    Population,
    Nexus
}

public class ButtonSlot : MonoBehaviour
{
    //오브젝트풀 꺼내서
    //포지션맞추기
    public GameObject targetObj;

    public Image imageCompo;//슬롯 이미지 
    public Action slotAction;//슬롯 기능
    public BuildType buildType;

    public SlotManager slotManager;

    public bool isBuildClicked;


    //그래픽
    public MeshRenderer meshRenderer;
    MaterialPropertyBlock mpb;
    public void BTN()//빌딩생산 먼저 테스트, 지정자
    {
        if (slotAction != null)
            slotAction();
    }

    //그래픽
    private void SetMPB(string propertyName, Color color)
    {
        meshRenderer.GetPropertyBlock(mpb);
        mpb.SetColor(propertyName, color);
        meshRenderer.SetPropertyBlock(mpb);
    }
    //
    private void Start()
    {
        //그래픽
        mpb = new MaterialPropertyBlock();
        //
        slotManager = GetComponentInParent<SlotManager>();
    }
    private void Update()
    {
        BuildCheck();
    }

    void BuildCheck()
    {
        if (isBuildClicked)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 150f, ~(1 << 13 | 1 << 8 | 1 << 12)))//빌딩을 제외한 레이어
            {
                targetObj.transform.position = hitInfo.point;
                targetObj.GetComponent<NavMeshObstacle>().enabled = false;

                //그래픽
                SetMPB("_InstallColor", Color.green);
                //targetObj.GetComponent<MeshRenderer>().material.SetColor("_InstallColor", Color.green);

                targetObj.GetComponent<FieldOfView>().fov.GetComponent<MeshRenderer>().enabled = false;
                //유아이 지점에서는 클릭안되게
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    targetObj.GetComponent<NavMeshObstacle>().enabled = true;

                    //targetObj.GetComponent<MeshRenderer>().material.SetColor("_InstallColor", Color.black);
                    targetObj.GetComponent<FieldOfView>().fov.GetComponent<MeshRenderer>().enabled = true;
                    StartCoroutine(BuildProgressCo(hitInfo.point));
                    GameManager.Instance.rtsController.DeselctBuliding();
                    isBuildClicked = false;
                }

                //빌드 취소 우클릭 미완
                if(Input.GetMouseButtonDown(1))
                {
                    //대상 오브젝트를 돌려줘야함
                    GameManager.Instance.buildingObjectPool.ReturnPool(targetObj);
                    targetObj = null;
                }
            }
        }
    }


    //빌딩 짓는 진행과정 코루틴
    IEnumerator BuildProgressCo(Vector3 hitpoint)
    {
        float coolTime = 6f;
        slotManager.selectedSupplyUnit.agent.SetDestination(hitpoint);

        if (targetObj.TryGetComponent(out PopulationBuilding populationBuilding))
        {
            GameManager.Instance.MaxPopulation += populationBuilding.addPopulation;
        }
        targetObj.gameObject.SetActive(false);


        GameObject temp = Instantiate(slotManager.buildingProgressprefab,targetObj.transform.position,targetObj.transform.rotation);
        TimelineController tc = temp.GetComponent<TimelineController>();
        TCPlay(tc, 1);

        //일꾼 도착까지 기다림
        //매번 남은 거리가 0이되는 이유 찾음
        //코루틴에서는 이동에 있어 다른 업데이트로 넘겨주지 않으면
        //남은거리가 요원을 근거하기에 갱신이 안됨
        yield return null;
        while (slotManager.selectedSupplyUnit.agent.remainingDistance >= 0.05f)
        {
            //Debug.Log("일꾼 거리" + slotManager.selectedSupplyUnit.agent.remainingDistance);
            yield return new WaitForEndOfFrame();
        }
        slotManager.selectedSupplyUnit.sm.SetState((int)UNIT_STATE.Work);

        //일꾼이 도착하면 다음단계
        TCPlay(tc, 2);
        yield return new WaitForSeconds(coolTime / 2);
        TCPlay(tc, 3);
        yield return new WaitForSeconds(coolTime / 2);

        slotManager.selectedSupplyUnit.sm.SetState((int)UNIT_STATE.Move);

        Destroy(temp.gameObject);
        Instantiate(slotManager.buildingEFTprefab,targetObj.transform);
        //그래픽
        SetMPB("_InstallColor", Color.black);
        targetObj.gameObject.SetActive(true);
        targetObj = null;
    }

    void TCPlay(TimelineController tc, int boolIndex)
    {
        tc.track[0] = tc.timeline.GetRootTrack(0);
        tc.track[1] = tc.timeline.GetRootTrack(1);
        tc.track[2] = tc.timeline.GetRootTrack(2);
        switch (boolIndex)
        {
            case 1:
                tc.track[0].muted = false;
                tc.track[1].muted = true;
                tc.track[2].muted = true;
                tc.playable.Play();
                break;
            case 2:
                tc.track[0].muted = true;
                tc.track[1].muted = false;
                tc.track[2].muted = true;
                tc.playable.Play();
                break;
            case 3:
                tc.track[0].muted = true;
                tc.track[1].muted = true;
                tc.track[2].muted = false;
                tc.playable.Play();
                break;

            default:
                break;
        }
    }
}