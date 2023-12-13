using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ShopUIManager : SingleTon<ShopUIManager>
{
    public Button sellBtn;

    private void Start()
    {
        sellBtn.onClick.AddListener(() => { SellingItem(InventoryManager.Instance.waitForSelling); });
    }



    public void SellingItem(InventoryItem item)
    {
        if (item == null) return;

        if (item.Stackable && item.Count > 1)
        {
            item.Count--;
            item.RefreshCount();
        }
        else
        {
            item.GetComponentInParent<DragableSlot>().item = null;
            Destroy(item.gameObject);
            InventoryManager.Instance.inven.curItemCount--;
        }
        Debug.Log("판매 금액 돌려주기");
    }
}
