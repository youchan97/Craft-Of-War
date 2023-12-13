using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MONSTER_STATE
{ IDLE = 0, MOVE, ATTACK, DIE }

public class Monster : Character
{
    StateMachine<Monster> stateMachine;
    DetectiveComponent detectiveComponent;
    Animator anim;
    [SerializeField] Vector3 originPos;
    

    public Vector3 OriginPos
    {
        get { return originPos; }
        set { originPos = value; }
    }

    public MONSTER_STATE monState;
    public Animator Anim { get { return anim; } }
    public CharacterInfo CharInfo { get { return info; }}
    public NavMeshAgent Agent { get { return agent; } }
    public DetectiveComponent DetectiveComponent { get {  return detectiveComponent; } }

    public override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        detectiveComponent = GetComponent<DetectiveComponent>();

        stateMachine = new StateMachine<Monster>(this);
        stateMachine.AddState((int)MONSTER_STATE.IDLE, new MonsterIdleState());
        stateMachine.AddState((int)MONSTER_STATE.MOVE, new MonsterMoveState());
        stateMachine.AddState((int)MONSTER_STATE.ATTACK, new MonsterAttackState());

        stateMachine.SetState((int)MONSTER_STATE.IDLE);
        monState = MONSTER_STATE.IDLE;
        OriginPos = transform.position;
        InitStats();
    }

    private void Update()
    {
        stateMachine.UpdateState();
    }


    public override void InitStats()
    {
        info.MaxHp = 500;
        info.CurentHp = info.MaxHp;
        info.Atk = 50;
        info.Def = 10;
        info.SightRange = 12f;
        info.AtkRange = 6.5f;
        info.AtkSpeed = 1.5f;
        info.Gold = 55;
        info.MoveSpeed = 5f;
    }
    public override void Attack(IHitAble target) // 연결 우찌하지 이거 
    {
        target.Hit(this);
    }

    public override void Hit(IAttackAble attacker)
    {
        int damage = attacker.Atk;
        Hp -= damage;
    }

    public override void Die()
    {
        monState = MONSTER_STATE.DIE;
        anim.SetTrigger("DeathTrigger");
    }


}
