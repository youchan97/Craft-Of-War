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
    public Image skillImg;
    public Image skillCoolImg;
    public TextMeshProUGUI coolTimeTxt;


    public void Init(Hero hero)
    {
        this.hero = hero;
        skill = hero.skillDic[(int)type];
        //skillImg = skill.img
    }

    public void TrySkillActive()
    {
        hero.UseSkill(type);
       // skill.Active();
    }
}
