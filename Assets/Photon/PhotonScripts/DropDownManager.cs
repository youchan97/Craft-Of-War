using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DropDownManager : MonoBehaviourPunCallbacks
{
    TMP_Dropdown hero;
    public static string selectHeroName;
    public TextMeshProUGUI heroName;
    void Start()
    {
        hero = GetComponent<TMP_Dropdown>();
        selectHeroName = "Morgana";

        hero.onValueChanged.AddListener((int data) => { HeroText(heroName); }) ;
    }

    void HeroText(TextMeshProUGUI heroName)
    {
        Debug.Log(heroName.text);
        selectHeroName = heroName.text;
        
    }

}
