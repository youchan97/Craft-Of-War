using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    Hero owner;
    int level;

    public Skill(Hero owner)
    {
        this.owner = owner;
        this.level = 1;
    }

    public abstract void SkillInit();

    public abstract void Active();

    
}





