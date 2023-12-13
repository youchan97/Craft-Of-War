using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour
{
    [SerializeField] private LayerMask layerUnit;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private LayerMask layerBuilding;
    private Camera mainCamera;
    private RTSController controller;
    public SlotManager behaviourSlotManager;

    private void Awake()
    {
        mainCamera = Camera.main;
        controller = GetComponent< RTSController>();
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
                        behaviourSlotManager.SlotType = SLOTTYPE.ProductBuilding;
                    }
                    //넥서스 선택했으면
                    if (hit.collider.gameObject.TryGetComponent(out NexusBuilding nexusBuilding))
                    {
                        behaviourSlotManager.SlotType = SLOTTYPE.NexusBuilding;
                    }
                }
            }
            else
            {
                controller.DeselctBuliding();
                behaviourSlotManager.SlotType = SLOTTYPE.None;
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
                        behaviourSlotManager.SlotType = SLOTTYPE.SupplyUnit;
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
