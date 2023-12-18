using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class MouseClick : MonoBehaviourPunCallbacks
{
    private LayerMask layerUnit;
    [SerializeField] private LayerMask layerGround;
    private LayerMask layerBuilding;
    private Camera mainCamera;
    private RTSController controller;

    private void Awake()
    {
        mainCamera = Camera.main;
        controller = GetComponent< RTSController>();
        if(PhotonNetwork.IsMasterClient)
        {
            layerUnit = 1 << 6;
            layerBuilding = 1 << 12;
        }
        else
        {
            layerUnit = 1 << 7;
            layerBuilding = 1 << 13;
        }
    }

    private void Update()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        MouseControll();
    }

    void MouseControll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //빌딩 클릭할때
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerBuilding))
            {
                if (hit.transform.TryGetComponent(out BuildingController buildingCon))
                {
                    controller.DeselctBuliding();
                    controller.SelectBuilding = buildingCon;
                    buildingCon.SelectBuliding();
                    //생산빌딩 선택했으면
                    if (hit.collider.gameObject.TryGetComponent(out ProductBuilding productBuilding))
                    {
                        SlotManager.Instance.SlotType = SLOTTYPE.ProductBuilding;
                    }
                    //넥서스 선택했으면
                    if (hit.collider.gameObject.TryGetComponent(out NexusBuilding nexusBuilding))
                    {
                        SlotManager.Instance.SlotType = SLOTTYPE.NexusBuilding;
                    }
                }
            }
            else
            {
                controller.DeselctBuliding();
                SlotManager.Instance.SlotType = SLOTTYPE.None;
            }

            //유닛 클릭할떄
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
            {
                if (hit.transform.GetComponent<UnitController>() == null) return;

                //다중클릭
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    controller.ShiftClickSelectUnit(hit.transform.GetComponent<UnitController>());
                }
                else
                {
                    controller.ClickSelectUnit(hit.transform.GetComponent<UnitController>());
                    //생산유닛 클릭 했을때
                    if(hit.collider.gameObject.TryGetComponent(out SupplyUnit supplyUnit))
                    {
                        SlotManager.Instance.SlotType = SLOTTYPE.SupplyUnit;
                        SlotManager.Instance.selectedSupplyUnit = supplyUnit;
                    }
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    controller.DeselectAll();
                }
            }
        }
        //이동
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log("클릭");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 8)))//FOV 안찍히게 전달하기위함
            {
                Debug.Log("땅을 찍음");
                controller.MoveSelectedUnits(hit);
            }
        }

    }
}
