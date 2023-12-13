using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Item : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI countText;
    public ScriptableItem itemData;

    public EItemType type;
    bool stackable = false;

    int count = 1;
    int id;
    [HideInInspector] public Transform parentAfterDrag;

    protected Action initItemData = null;

    public int Count
    {
        get { return count; }
        set
        {
            count = value;
            RefreshCount();
        }
    }

    public bool Stackable {get => stackable; }
    public int ID { get => id; }

    private void Awake()
    {
        initItemData += () => { InitialiseItem(itemData); };
        initItemData += RefreshCount;
    }

    public void InitialiseItem(ScriptableItem itemData)
    {
        image = GetComponent<Image>();

        type = itemData.type;
        stackable = itemData.stackable;
        image.sprite = itemData.image;
        id = itemData.ID;
    }

    public void RefreshCount()
    {
        bool isActive = count > 1;
        countText.gameObject.SetActive(isActive);
        countText.text = count.ToString();

    }
}
