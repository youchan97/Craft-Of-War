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
    public bool stackable = true;
}

public enum EItemType
{
    CONSUMABLE,
    EQUIPABLE,
    END,
}

