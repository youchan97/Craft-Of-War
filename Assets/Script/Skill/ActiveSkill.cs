using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    [SerializeField] private int level;
   
    private Character target;
    
    private int reqMp;
    
    public int Level { get { return level; } set { level = value; } }
    public int ReqMp { get => reqMp; set { reqMp = value; } }
    


    private void Start()
    {
        SkillInit();
    }

    public override void Active()
    {
        
        if (owner.CurMp < ReqMp || owner.curState == HERO_STATE.STUN || owner.curState == HERO_STATE.DIE) return;
        
    }

    public override void SkillInit()
    {
        Level = 1;
    }

}
