using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

/// <summary>
/// Basic Infomation of Characters.
/// </summary>
public struct CharacterInfo
{
    private int maxHp;
    public int curentHp;
    private int atk;
    private int def;
    private float moveSpeed;
    private float atkSpeed;
    private float sightRange;
    private float atkRange;
    private int gold;

    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int CurentHp { get => curentHp; set => curentHp = value; }
    public int Atk { get => atk; set => atk = value; }
    public int Def { get => def; set => def = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AtkSpeed { get => atkSpeed; set => atkSpeed = value; }
    public float SightRange { get => sightRange; set => sightRange = value; }
    public float AtkRange { get => atkRange; set => atkRange = value; }
    public int Gold { get => gold; set => gold = value; }

}


public abstract class Character : MonoBehaviour, IAttackAble, IHitAble
{
    protected CharacterInfo info;
    public StateMachine<Character> sm;
    public Animator animator;
    public NavMeshAgent agent;
    public virtual void Awake()
    {
        sm = new StateMachine<Character>(this);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    public int Atk { get => info.Atk; set => info.Atk = value; }
    public int Hp { get => info.CurentHp; set => info.CurentHp = value; }
    public abstract void InitStats();
    public abstract void Attack(IHitAble target);
    public abstract void Hit(IAttackAble attacker);
    public abstract void Die();
}