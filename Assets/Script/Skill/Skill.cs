using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    public int damage;
    public Hero owner;
    public Sprite img;
    [SerializeField]private float coolTime;
    public float range;
    public float speed;
    public float CoolTime { get => coolTime; set => coolTime = value; }
    public bool isActive;
    public float durationTime;


    private void Start()
    {
        isActive = false;
    }
    public void SetOwner(Hero owner)
    {
        this.owner = owner;
    }

    public abstract void SkillInit();
    public abstract void Active();
}





