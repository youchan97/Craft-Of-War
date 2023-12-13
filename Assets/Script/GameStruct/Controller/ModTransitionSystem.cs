using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModTransitionSystem : MonoBehaviour
{
    public GameObject rtsControlSystem;
    UnitController unitController;
    ClickMoveController aosHeroController;
    bool modSwitch;

    public bool ModSwitch
    {
        get { return modSwitch; }
        set { modSwitch = value; }  
    }

    private void Start()
    {
        unitController = GameManager.Instance.PlayerHero.GetComponent<UnitController>();
        aosHeroController = GameManager.Instance.PlayerHero.GetComponent<ClickMoveController>();

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)) 
        {
            ChangeMod();
        }
    }

    public void ChangeMod()
    {
        rtsControlSystem.SetActive(ModSwitch);
        unitController.enabled = ModSwitch;
        aosHeroController.enabled = !ModSwitch;
        ModSwitch = !ModSwitch;


        // ChangeMod 메서드 호출 시 Play 모드 변경 (회민)
        // 두 플레이 모드에 따라 전투 로직이 달라지므로 나누어 둠
        // 어떤 상호작용을 할 때 현재 모드에 따라 가능할지 불가능할지 결정되기에 변수로 설정
        if (GameManager.Instance.playMode == PLAY_MODE.AOS_MODE)
        {
            GameManager.Instance.playMode = PLAY_MODE.RTS_MODE;
            aosHeroController.lineRenderer.enabled = false; //맵에 선 생기는 부분 예외처리
        }
        else
        {
            GameManager.Instance.playMode = PLAY_MODE.AOS_MODE;
            GameManager.Instance.rtsController.DeselectAll();
            GameManager.Instance.rtsController.DeselctBuliding();
        }

    }
}
