using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Inventory : MonoBehaviour
{
    public ClickMoveController owner;
    public DragableSlot[] slots;
    public int maxSlotCount = 6;
    public int curItemCount = 0;

    public void UseItem(InventoryItem item)
    {
        if (!item.Stackable) return;
        
        Debug.Log("사용 되었음");
        if(item.Count > 1)
        {
            item.Count--;
        }
        else
        {
            item.GetComponentInParent<DragableSlot>().item = null;
            Destroy(item.gameObject);
            curItemCount--;
        }
        //owner.SetStatus();
    }
}
