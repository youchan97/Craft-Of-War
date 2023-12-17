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
        //for (int i = 0; i < inven.slots.Length; i++)
        //{
        //    if (inven.slots[i].item == null)
        //    {
        //        if (inven.curItemCount == inven.maxSlotCount) return;
        //        inven.slots[i].item = Instantiate(item, inven.slots[i].transform).GetComponent<InventoryItem>();
        //        inven.curItemCount++;
        //        break;
        //    }
        //    else
        //    {
        //        if (inven.slots[i].item.Stackable 
        //            && inven.slots[i].item.ID == item.GetComponent<InventoryItem>().itemData.ID)
        //        {
        //                inven.slots[i].item.Count++;
        //                inven.slots[i].item.RefreshCount();

        //            break;
        //        }
        //    }
        //}
        if (inven.curItemCount == inven.maxSlotCount)   //아이템 슬룻이 꽉차 있을때
            return;
        if (inven.curItemCount == 0)                    //아이템 슬룻에 아이템이 없을때
        {
            inven.slots[0].item = Instantiate(item, inven.slots[0].transform).GetComponent<InventoryItem>();
            inven.curItemCount++;
            return;
        }
        else                                            //아이템 슬룻에 아이템이 있을때
        {
            for (int k = 0; k < inven.slots.Length; k++)//아이템 슬룻에 스택이 가능할때
            {
                if (inven.slots[k].item != null)
                {
                    if (inven.slots[k].item.Stackable && inven.slots[k].item.ID == item.GetComponent<InventoryItem>().itemData.ID)
                    {   //같은 아이템이 있을경우
                        inven.slots[k].item.Count++;
                        inven.slots[k].item.RefreshCount();
                        return;
                    }
                }
            }
            for (int s = 0; s < inven.slots.Length; s++)
            {           //같은 아이템이 없을경우
                if (inven.slots[s].item == null)
                {
                    inven.slots[s].item = Instantiate(item, inven.slots[s].transform).GetComponent<InventoryItem>();
                    inven.curItemCount++;
                    return;
                }
            }        
        }
    }



}