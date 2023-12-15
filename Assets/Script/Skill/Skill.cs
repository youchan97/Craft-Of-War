using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    Hero owner;
    private float coolTime;
    [SerializeField] private bool isCool;

    public Skill(Hero owner)
    {
        this.owner = owner;
    }


    public abstract void Active();
}





