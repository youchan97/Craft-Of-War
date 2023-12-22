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
        ItemDeApplication(item);
        GameManager.Instance.Gold += item.itemData.price * (7/10); 
    }

    public void ItemDeApplication(Item item)
    {
        if (item.itemData.type == EItemType.EQUIPABLE)
        {
            switch (item.itemData.equipableItemType)
            {
                case EquipableItemType.BOTTOM:
                    GameManager.Instance.playerHero.info.Def -= (int)item.itemData.value;
                    break;
                case EquipableItemType.TOP:
                    GameManager.Instance.playerHero.info.MaxHp -= (int)item.itemData.value;
                    break;
                case EquipableItemType.SHOOSE:
                    GameManager.Instance.playerHero.MoveSpeed -= (int)item.itemData.value;
                    break;
                case EquipableItemType.HAT:
                    GameManager.Instance.playerHero.info.SightRange -= (int)item.itemData.value;
                    break;
                case EquipableItemType.HAND:
                    GameManager.Instance.playerHero.info.Atk -= (int)item.itemData.value;
                    break;
                case EquipableItemType.EARRING:
                    GameManager.Instance.playerHero.MaxMp -= (int)item.itemData.value;
                    break;
                case EquipableItemType.RING:
                    GameManager.Instance.playerHero.info.AtkSpeed -= (int)item.itemData.value;
                    break;
                case EquipableItemType.NECKLE:
                    GameManager.Instance.playerHero.info.AtkRange -= (int)item.itemData.value;
                    break;
            }
        }
    }
}
