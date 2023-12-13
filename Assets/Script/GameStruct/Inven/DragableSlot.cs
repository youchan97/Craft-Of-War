using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableSlot : MonoBehaviour, IDropHandler
{
    public InventoryItem item = null;

    private void Start()
    {
        if (item == null)
            item = GetComponentInChildren<InventoryItem>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem temp = item;
        item = eventData.pointerDrag.GetComponent<InventoryItem>();
        
        if (transform.childCount != 0) 
        {
            temp.transform.SetParent(item.parentAfterDrag); 
            temp.transform.parent.GetComponent<DragableSlot>().item = temp;
            temp.transform.localPosition = Vector3.zero;
        }

        item.parentAfterDrag = transform;
    }
}
