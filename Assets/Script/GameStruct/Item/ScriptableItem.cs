using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Scriptable Item")]
public class ScriptableItem : ScriptableObject
{
    public int ID;
    public Sprite image;
    public EItemType type;
    public EquipableItemType equipableItemType;
    public bool stackable = true;
    public float value;
    public int price;
}

public enum EItemType
{
    CONSUMABLE,
    EQUIPABLE,
    END,
}
public enum EquipableItemType
{
    NONE,
    BOTTOM,
    SHOOSE,
    HAT,
    EARRING,
    HAND,
    TOP,
    RING,
    NECKLE
}

