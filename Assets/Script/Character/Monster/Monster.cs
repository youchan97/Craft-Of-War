using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public enum MONSTER_STATE
{ IDLE = 0, MOVE, ATTACK, DIE }

public class Monster : Character
{
    public int dropGold;
    public int dropExp;

    StateMachine<Monster> stateMachine;
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

    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        stateMachine = new StateMachine<Monster>(this);
        stateMachine.AddState((int)MONSTER_STATE.IDLE, new MonsterIdleState());
        stateMachine.AddState((int)MONSTER_STATE.MOVE, new MonsterMoveState());
        stateMachine.AddState((int)MONSTER_STATE.ATTACK, new MonsterAttackState());

        stateMachine.SetState((int)MONSTER_STATE.IDLE);
        monState = MONSTER_STATE.IDLE;
        OriginPos = transform.position;
        pv.RPC("InitStats", RpcTarget.AllBuffered);
        
    }

    private void Update()
    {
        stateMachine.UpdateState();

    }

    [PunRPC]
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
        if(Hp < 0)
        {
            ((Hero)attacker).info.Gold += dropGold;
            ((Hero)attacker).CurExp += dropExp;
            Die();
        }

        int damage = attacker.Atk;
        Hp -= damage;
    }

    public override void Die()
    {

        monState = MONSTER_STATE.DIE;
        anim.SetTrigger("DeathTrigger");
    }

    [PunRPC]
    public void RPCSetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    [PunRPC]
    public void Enable(int index)
    {
        this.gameObject.transform.position = GameManager.Instance.monsterSpawnPoints[index].position;
        OriginPos = GameManager.Instance.monsterSpawnPoints[index].position;
        this.gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }
}
