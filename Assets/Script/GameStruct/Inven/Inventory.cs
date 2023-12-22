using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Inventory : MonoBehaviour
{
    public DragableSlot[] slots;
    public int maxSlotCount = 6;
    public int curItemCount = 0;

    private void Start()
    {
        slots = GetComponentsInChildren<DragableSlot>();
    }
    public void UseItem(InventoryItem item)
    {
        if (!item.Stackable) return;

        if (item.itemData.ID == 1)
            GameManager.Instance.PlayerHero.Hp += (int)item.itemData.value;
        if (item.itemData.ID == 2)
            GameManager.Instance.PlayerHero.CurMp += (int)item.itemData.value;
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
