using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : MonoBehaviour
{
    protected Hero owner;
    public Sprite img;

    public Skill(Hero owner)
    {
        this.owner = owner;
    }

    public abstract void SkillInit();

    public abstract void Active();

    
}





