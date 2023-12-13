using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public enum SKILL_TYPE
{ QSkill, WSkill, ESkill, RSkill}

public enum HERO_STATE
{ IDLE, MOVE, ATTACK, STUN, DIE}

public abstract class Hero : Character, IControllable
{
    [SerializeField] private int level;
    [SerializeField] private float curExp;
    [SerializeField] private float aimExp;
    public HERO_STATE curState;
    
    

    //Property 부분 변경시 포톤뷰를 통해 업데이트
    public int Level
    { get => level; set => level = value; }
    public float CurExp
    { get => curExp; set => curExp = value; }
    public float AimExp
    { get => aimExp; set => aimExp = value; }
    public float MoveSpeed
    { get => info.MoveSpeed; set => info.MoveSpeed = value; }
    public NavMeshAgent Agent
    { get => agent; set => agent = value; }

    private void Start()
    {
        sm = new StateMachine<Character>(this);
        InitStats();
    }

    public override void InitStats() 
    { 
        level = 1;
        curExp = 0;
        curState = HERO_STATE.IDLE;
    }

    /// <summary>
    /// Attack 메서드는 모든 영웅 공통으로 적용
    /// </summary>
    public override void Attack(IHitAble target)
    {
        // 공격 함수 호출되는 경우
        // 1. 직접 타겟 설정
        // 2. 어택 땅
        // 3. 자동 공격
        // 4. 공격 가능 대상
        //  3-1. 직접 타겟 설정의 경우 : 자기 자신을 제외한 모든 유닛, 건물, 영웅 공격 가능(아군 포함)
        //  3-2. 어택 땅의 경우 : 적 건물, 유닛, 영웅 공격 가능
    }

    public abstract void UseSkill(Skill skill);

    private void FixedUpdate()
    {
        // velocity 값이 변하면 run, 아니면 idle - 보람
        if (agent.velocity != Vector3.zero)
            animator.SetBool("IsMove", true);
        else
            animator.SetBool("IsMove", false);
    }
}
