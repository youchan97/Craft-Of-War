using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    [SerializeField] private int level;
    [SerializeField] private bool isCool;
    private float coolTime;
    private Character target;
    private Vector3 dir;
    private int reqMp;
    
    
    public bool IsCool { get { return isCool; } set { isCool = value; } }
    public int Level { get { return level; } set { level = value; } }
    public int ReqMp { get => reqMp; set { reqMp = value; } }
    public float CoolTime { get;set; }

    public ActiveSkill(Hero owner) : base(owner)
    {
        Level = 1;
        IsCool = false;
    }

    public override void Active()
    {
        if (owner.CurMp < ReqMp || owner.curState == HERO_STATE.STUN || owner.curState == HERO_STATE.DIE || IsCool) return;



    }

    public override void SkillInit()
    {
        
    }

    IEnumerator CoolTimeCor()
    {
        float curTime = 0f;
        while (true)
        {
            curTime += Time.deltaTime;
            yield return null;
        }
    }
}
