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
    Image unitHp;
    private void Start()
    {
        mainCamera = Camera.main;

        if (GetComponentInParent<Unit>() != null)
        {
            Unit unit = GetComponentInParent<Unit>();
            this.unit = unit;
        }

        if (GetComponentInParent<Hero>() != null)
        {
            Hero hero = GetComponentInParent<Hero>();
            this.hero = hero;
        }

        if (GetComponentInParent<Monster>() != null)
        {
            Monster monster = GetComponentInParent<Monster>();
            this.monster = monster;
        }

        unitHp = GetComponentInChildren<Image>();
    }
    private void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation* Vector3.back,
        mainCamera.transform.rotation* Vector3.up );
        if(unit != null)
            unitHp.fillAmount = (float)unit.Hp / (float)100; // 임시 처방
        if(hero != null)
            unitHp.fillAmount = (float)hero.info.curentHp / (float)hero.info.MaxHp;
        if(monster != null)
            unitHp.fillAmount = (float)monster.info.curentHp / (float)monster.info.MaxHp;
    }
}
