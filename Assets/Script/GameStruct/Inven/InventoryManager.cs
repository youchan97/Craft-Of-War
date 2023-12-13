using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// inventory 와 inventoryManager가 따로 있어야 할까?
public class InventoryManager : SingleTon<InventoryManager>
{
    public Inventory inven;
    public InventoryItem waitForSelling = null;

    public void AddItem(GameObject item)
    {

        for (int i = 0; i < inven.slots.Length; i++)
        {
            if (inven.slots[i].item == null)
            {
                if (inven.curItemCount == inven.maxSlotCount) return;
                inven.slots[i].item = Instantiate(item, inven.slots[i].transform).GetComponent<InventoryItem>();
                inven.curItemCount++;
                break;
            }
            else
            {
                if (inven.slots[i].item.Stackable 
                    && inven.slots[i].item.ID == item.GetComponent<InventoryItem>().itemData.ID)
                {
                        inven.slots[i].item.Count++;
                        inven.slots[i].item.RefreshCount();

                    break;
                }
            }
        }
    }


}
