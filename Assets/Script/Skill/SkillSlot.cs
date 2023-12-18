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
        skillCoolImg.type = Image.Type.Filled;
        skillCoolImg.fillAmount = 0;
    }

    public void TrySkillActive()
    {
        //if (skillCoolImg.fillAmount > 0) return;
        skill.Active();
        //StartCoroutine(skillCoolCo());
    }

    IEnumerator skillCoolCo()
    {
        float tick = 1f / skill.CoolTime;
        float t = 0;

        skillCoolImg.fillAmount = 1;

        while (skillCoolImg.fillAmount > 0)
        {
            skillCoolImg.fillAmount = Mathf.Lerp(1, 0, t);
            t += Time.deltaTime * tick;

            yield return null;
        }
    }
}
