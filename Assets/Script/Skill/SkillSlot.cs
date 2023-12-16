using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Hero hero;
    public SKILL_TYPE type;
    public Skill skill;

    [Header("스킬 슬롯 구성")]
    public Image skillImg;
    public Image skillCoolImg;
    public TextMeshProUGUI coolTimeTxt;

    private void Start()
    {
        Init(hero);
    }

    public void Init(Hero hero)
    {
        this.hero = hero;
        skill = hero.skillDic[(int)type];
        skillImg.sprite = skill.img;
        skillCoolImg.sprite = skill.img;
        coolTimeTxt.enabled = false;
    }

    public void TrySkillActive()
    {
        hero.UseSkill(type);
       // skill.Active();
    }
}
