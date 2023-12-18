using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    public Hero owner;
    public Sprite img;
    private float coolTime;
    public float range;
    public float speed;
    public float CoolTime { get => coolTime; set => coolTime = value; }


    public void SetOwner(Hero owner)
    {
        this.owner = owner;
    }

    public abstract void SkillInit();
    public abstract void Active();
}





