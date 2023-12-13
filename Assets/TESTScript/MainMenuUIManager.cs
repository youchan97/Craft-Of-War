using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("메인메뉴 오브젝트")]
    public GameObject matchingMenu;
    public GameObject setting;

    [Header("매칭메뉴 오브젝트")]
    public GameObject pl1Ready;


    public void MatchingMenuEnter()
    {
        matchingMenu.SetActive(true);  //메인 메뉴에 매칭 버튼
    }
    public void MatchingMenuExit()
    {
        matchingMenu.SetActive(false);
    }

    public void SettingEnter() // 설정 
    {
        setting.SetActive(true);
    }
    public void SettingExit()
    {
        setting.SetActive(false);
    }
    
  

    public void Exit()
    {
        Application.Quit();
    }
    //플레이어 영웅 정보와 종족 정보는 Dropdown.value에 접근하여 알아내야됩니다.
}
