using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class CanvasFront : MonoBehaviour
{
    Camera mainCamera;
    Unit unit = null;
    Hero hero = null;
    Monster monster = null;
    Building building = null;
    Image unitHp;
    private void Start()
    {
        mainCamera = Camera.main;
        if(TryGetComponent(out Unit unit))
            this.unit = unit;
        if(TryGetComponent(out Hero hero))
            this.hero = hero;
        if(TryGetComponent(out Monster monster))
            this.monster = monster;
        if(TryGetComponent(out Building building))
            this.building = building;
        unitHp = GetComponentInChildren<Image>();
    }
    private void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation* Vector3.back,
        mainCamera.transform.rotation* Vector3.up );
        if(unit != null)
            unitHp.fillAmount = (float)unit.Hp / (float)100; // 烙矫 贸规
        if (hero != null)
            unitHp.fillAmount = (float)hero.info.curentHp / (float)hero.info.MaxHp;
        if(monster != null)
            unitHp.fillAmount = (float)monster.info.curentHp / (float)monster.info.MaxHp;
        if (building != null)
            unitHp.fillAmount = (float)building.Hp / (float)100; // 烙矫 贸规
    }
}
