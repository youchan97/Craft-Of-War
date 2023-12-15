using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Skill skill;
    public Image skillImg;
    public Image skillCoolImg;
    public TextMeshProUGUI coolTimeTxt;


    public void TrySkillActive()
    {
        skill.Active();
    }
}
