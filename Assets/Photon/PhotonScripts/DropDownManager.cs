using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DropDownManager : MonoBehaviourPunCallbacks
{
    TMP_Dropdown select;
    TMP_Dropdown unitSelect;
    public static string selectHeroName;
    public static string selectTribe;
    public TextMeshProUGUI heroName;
    public TextMeshProUGUI tribeName;

    void Start()
    {
        select = GetComponent<TMP_Dropdown>();
        unitSelect = GetComponent<TMP_Dropdown>();
        selectHeroName = "Morgana";
        selectTribe = "Human";

        select.onValueChanged.AddListener((int data) => { HeroText(heroName); });
        unitSelect.onValueChanged.AddListener((int data) => { SelectUnit(tribeName); });
    }

    void SelectUnit(TextMeshProUGUI unitTribe)
    {
        selectTribe = unitTribe.text;
    }

    void HeroText(TextMeshProUGUI heroName)
    {
        selectHeroName = heroName.text;
    }

}
