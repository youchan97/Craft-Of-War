using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventoryItem : Item, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private void Start()
    {
        initItemData();
        parentAfterDrag = transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root); // retrun topmost in hierarchy
        transform.SetAsLastSibling(); // move front
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero; // snapping, not use grid layout
        image.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount == 2)
        {
            InventoryManager.Instance.inven.UseItem(eventData.pointerClick.GetComponent<InventoryItem>());
        }

        if(eventData.clickCount == 1)
        {
            Debug.Log("되팔기할 준비" + eventData.pointerEnter.name);
            InventoryManager.Instance.waitForSelling = eventData.pointerClick.GetComponent<InventoryItem>();
        }
    }
}
