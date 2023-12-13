using UnityEngine.AI;
public interface IControllable
{
    public float MoveSpeed { get; set; }
    public NavMeshAgent Agent { get; set; }
}

public interface IHitAble
{
    int Hp
    { get; set; }
    void Hit(IAttackAble attacker);
    void Die();
}

public interface IAttackAble
{
    int Atk
    { get; set; }
    void Attack(IHitAble target);
}

public interface IProductAble
{
    void Production();
}
